using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;

namespace TrelloMini.Web.AppCodes
{
    /// <summary>
    /// Thông tin tài khoản người dùng được lưu trong phiên đăng nhập (Cookie) của TrelloMini
    /// </summary>
    public class WebUserData
    {
        // Chuyển thành cấu trúc lưu trữ phù hợp với Model User (UserID, Username)
        public string? UserID { get; set; }
        public string? Username { get; set; }
        public string? DisplayName { get; set; }
        public List<string>? Roles { get; set; }

        /// <summary>
        /// Lấy danh sách các Claim chứa thông tin của User để nhúng vào Cookie
        /// </summary>
        private List<Claim> Claims
        {
            get
            {
                List<Claim> claims = new List<Claim>()
                {
                    // Đồng bộ key theo tên thuộc tính của Model User trong dự án
                    new Claim(nameof(UserID), UserID ?? ""),
                    new Claim(nameof(Username), Username ?? ""),
                    new Claim(nameof(DisplayName), DisplayName ?? "")
                };

                if (Roles != null)
                {
                    foreach (var role in Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }
                return claims;
            }
        }

        /// <summary>
        /// Tạo Principal dựa trên thông tin định danh của thành viên
        /// </summary>
        public ClaimsPrincipal CreatePrincipal()
        {
            var claimIdentity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
            return claimPrincipal;
        }
    }

    /// <summary>
    /// Định nghĩa các quyền hạn cơ bản trong hệ thống quản lý công việc TrelloMini
    /// </summary>
    public class WebUserRoles
    {
        /// <summary>
        /// Quản trị viên hệ thống (Có toàn quyền)
        /// </summary>
        public const string Administrator = "admin";

        /// <summary>
        /// Người dùng thông thường (Chỉ quản lý Task cá nhân)
        /// </summary>
        public const string User = "user";
    }

    /// <summary>
    /// Bộ mở rộng giúp nhanh chóng lấy thông tin User đang đăng nhập từ HttpContext (User.GetUserData())
    /// </summary>
    public static class WebUserExtensions
    {
        public static WebUserData? GetUserData(this ClaimsPrincipal principal)
        {
            try
            {
                if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
                    return null;

                var userData = new WebUserData();

                // Đọc ngược dữ liệu từ các Claim trong Cookie ra object WebUserData
                userData.UserID = principal.FindFirstValue(nameof(userData.UserID));
                userData.Username = principal.FindFirstValue(nameof(userData.Username));
                userData.DisplayName = principal.FindFirstValue(nameof(userData.DisplayName));

                userData.Roles = new List<string>();
                foreach (var claim in principal.FindAll(ClaimTypes.Role))
                {
                    userData.Roles.Add(claim.Value);
                }

                return userData;
            }
            catch
            {
                return null;
            }
        }
    }
}