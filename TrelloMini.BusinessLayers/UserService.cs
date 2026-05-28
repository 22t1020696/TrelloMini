using TrelloMini.DataLayers;
using TrelloMini.Models;

namespace TrelloMini.BusinessLayers
{
    public class UserService
    {
        private readonly IUserDAL _userDB;

        public UserService(IUserDAL userDB)
        {
            _userDB = userDB;
        }

        // =========================
        // REGISTER
        // =========================
        public bool Register(User data)
        {
            return _userDB.Register(data);
        }

        // =========================
        // LOGIN
        // =========================
        public User? Login(string username, string password)
        {
            var user = _userDB.GetUser(username);

            // Không tồn tại user
            if (user == null)
                return null;

            // Sai mật khẩu
            if (user.Password != password)
                return null;

            return user;
        }
    }
}
