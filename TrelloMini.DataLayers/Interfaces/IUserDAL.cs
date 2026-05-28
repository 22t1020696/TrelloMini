using TrelloMini.Models;

namespace TrelloMini.DataLayers
{
    public interface IUserDAL
    {
        User? GetUser(string username);

        bool Register(User data);
    }
}