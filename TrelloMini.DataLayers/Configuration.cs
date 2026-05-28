using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrelloMini.DataLayers
{
    public static class Configuration
    {
        /// <summary>
        /// Chuỗi kết nối đến cơ sở dữ liệu SQL Server.
        /// Sẽ được gán giá trị một lần duy nhất khi ứng dụng Web khởi chạy (Startup).
        /// </summary>
        public static string ConnectionString { get; set; } = string.Empty;
    }
}
