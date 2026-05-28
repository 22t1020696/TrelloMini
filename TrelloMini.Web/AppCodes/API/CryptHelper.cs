using System.Security.Cryptography;
using System.Text;

namespace TrelloMini.Web.AppCodes
{
    /// <summary>
    /// Lớp cung cấp các hàm tiện ích sử dụng cho việc mã hóa bảo mật thông tin tài khoản TrelloMini
    /// </summary>
    public static class CryptHelper
    {
        /// <summary>
        /// Mã hóa MD5 chuỗi mật khẩu thô của User thành chuỗi Hex 32 ký tự
        /// </summary>
        /// <param name="input">Mật khẩu nhập từ giao diện (chưa mã hóa)</param>
        /// <returns>Chuỗi mật khẩu đã băm MD5</returns>
        public static string HashMD5(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // Định dạng hex viết thường
                }
                return sb.ToString();
            }
        }
    }
}