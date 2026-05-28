using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrelloMini.Models;

namespace TrelloMini.Web.AppCodes.API
{
    public class TaskApiService
    {
        private readonly HttpClient _httpClient;

        // ĐÃ SỬA: Thay HttpClient bằng IHttpClientFactory để rút cấu hình mang tên "API" từ Program.cs
        public TaskApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        // ==========================================
        // GET: Lấy danh sách công việc
        // ==========================================
        public async System.Threading.Tasks.Task<List<TrelloMini.Models.Task>> GetTasks()
        {
            try
            {
                // Đã sửa đường dẫn sang "/api/tasks" cho chuẩn cấu hình route thông thường của Web API
                var result = await _httpClient.GetFromJsonAsync<List<TrelloMini.Models.Task>>("/api/tasks");
                return result ?? new List<TrelloMini.Models.Task>();
            }
            catch
            {
                // Trả về danh sách rỗng thay vì làm sập ứng dụng nếu API Server chưa bật
                return new List<TrelloMini.Models.Task>();
            }
        }

        // ==========================================
        // POST: Tạo mới công việc
        // ==========================================
        public async System.Threading.Tasks.Task<bool> CreateTask(TrelloMini.Models.Task model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/tasks", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // ==========================================
        // PUT: Cập nhật công việc theo ID
        // ==========================================
        public async System.Threading.Tasks.Task<bool> UpdateTask(TrelloMini.Models.Task model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/tasks/{model.TaskID}", model);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // ==========================================
        // DELETE: Xóa công việc theo ID
        // ==========================================
        public async System.Threading.Tasks.Task<bool> DeleteTask(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/tasks/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}