using System;
using Microsoft.Data.SqlClient;
using TrelloMini.Models;

namespace TrelloMini.DataLayers.SQLServer
{
    public class UserDAL : IUserDAL
    {
        private readonly string _connectionString;

        public UserDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User? GetUser(string username)
        {
            User? data = null;

            using (SqlConnection cn =
                new SqlConnection(_connectionString))
            {
                cn.Open();

                string sql = @"SELECT * FROM Users
                               WHERE Username = @Username";

                SqlCommand cmd = new SqlCommand(sql, cn);

                cmd.Parameters.AddWithValue("@Username", username);

                SqlDataReader dbReader = cmd.ExecuteReader();

                if (dbReader.Read())
                {
                    data = new User()
                    {
                        UserID = Convert.ToInt32(dbReader["UserID"]),
                        Username = dbReader["Username"]?.ToString() ?? "",
                        Password = dbReader["Password"]?.ToString() ?? ""
                    };
                }

                dbReader.Close();
            }

            return data;
        }

        public bool Register(User data)
        {
            bool result = false;

            using (SqlConnection cn =
                new SqlConnection(_connectionString))
            {
                cn.Open();

                string sql = @"
                    INSERT INTO Users(Username, Password)
                    VALUES(@Username, @Password)
                ";

                SqlCommand cmd = new SqlCommand(sql, cn);

                cmd.Parameters.AddWithValue("@Username", data.Username);

                cmd.Parameters.AddWithValue("@Password", data.Password);

                result = cmd.ExecuteNonQuery() > 0;
            }

            return result;
        }
    }
}