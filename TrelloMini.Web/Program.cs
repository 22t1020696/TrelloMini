using TrelloMini.Web.AppCodes.API;
using TrelloMini.DataLayers;
using TrelloMini.DataLayers.SQLServer;
using TrelloMini.BusinessLayers;

using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// ADD MVC
// =====================================================
builder.Services.AddControllersWithViews();

// =====================================================
// API URL
// Lấy URL API từ appsettings.json
// =====================================================
var apiUrl =
    builder.Configuration["ApiSettings:BaseUrl"]
    ?? "https://localhost:7250/";

// =====================================================
// HTTP CLIENT
// =====================================================

// Auth API
builder.Services.AddHttpClient<AuthApiService>(
    client =>
    {
        client.BaseAddress = new Uri(apiUrl);
    }
);

// Task API
builder.Services.AddHttpClient<TaskApiService>(
    client =>
    {
        client.BaseAddress = new Uri(apiUrl);
    }
);

// =====================================================
// COOKIE AUTHENTICATION
// =====================================================
builder.Services
    .AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme
    )
    .AddCookie(options =>
    {
        // Tên cookie
        options.Cookie.Name =
            "TrelloMini.Session";

        // Nếu chưa login
        options.LoginPath =
            "/Auth/Login";

        // Logout
        options.LogoutPath =
            "/Auth/Logout";

        // Nếu không có quyền
        options.AccessDeniedPath =
            "/Auth/Login";

        // Thời gian hết hạn
        options.ExpireTimeSpan =
            TimeSpan.FromMinutes(60);

        // Tự gia hạn cookie khi hoạt động
        options.SlidingExpiration = true;
    });

// =====================================================
// CONNECTION STRING
// =====================================================
string connectionString =
    builder.Configuration.GetConnectionString(
        "DefaultConnection"
    ) ?? "";

// =====================================================
// DATA LAYERS
// =====================================================
builder.Services.AddScoped<ITaskDAL>(
    x => new TaskDAL(connectionString)
);

builder.Services.AddScoped<IUserDAL>(
    x => new UserDAL(connectionString)
);

// =====================================================
// BUSINESS LAYERS
// =====================================================
builder.Services.AddScoped<TaskService>();

builder.Services.AddScoped<UserService>();

var app = builder.Build();

// =====================================================
// MIDDLEWARE
// =====================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// =====================================================
// AUTHENTICATION
// =====================================================
app.UseAuthentication();

app.UseAuthorization();

// =====================================================
// ROUTE
// =====================================================
app.MapControllerRoute(
    name: "default",
    pattern:
    "{controller=Auth}/{action=Login}/{id?}"
);

app.Run();

