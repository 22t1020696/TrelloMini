using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace TrelloMini.DataLayers.SQLServer
{
    public class TaskDAL : ITaskDAL
    {
        private readonly string _connectionString;

        public TaskDAL(string connectionString)
        {
            _connectionString = connectionString;
        }
public List<TrelloMini.Models.Task> List(int userId)
        {
            List<TrelloMini.Models.Task> data =
                new List<TrelloMini.Models.Task>();

            using (SqlConnection cn =
                new SqlConnection(_connectionString))
            {
                cn.Open();

                string sql = @"
            SELECT *
            FROM Tasks
            WHERE UserID = @UserID
        ";

                SqlCommand cmd =
                    new SqlCommand(sql, cn);

                cmd.Parameters.AddWithValue(
                    "@UserID",
                    userId
                );

                SqlDataReader dbReader =
                    cmd.ExecuteReader();

                while (dbReader.Read())
                {
                    data.Add(new TrelloMini.Models.Task()
                    {
                        TaskID =
                            Convert.ToInt32(
                                dbReader["TaskID"]
                            ),

                        Title =
                            dbReader["Title"]?.ToString() ?? "",

                        Description =
                            dbReader["Description"]?.ToString() ?? "",

                        Status =
                            dbReader["Status"]?.ToString() ?? "",

                        UserID =
                            Convert.ToInt32(
                                dbReader["UserID"]
                            )
                    });
                }

                dbReader.Close();
            }

            return data;
        }



        public bool Add(TrelloMini.Models.Task data)
        {
            using (SqlConnection cn =
                new SqlConnection(_connectionString))
            {
                cn.Open();

                string sql = @"
            INSERT INTO Tasks
            (
                Title,
                Description,
                Status,
                UserID
            )
            VALUES
            (
                @Title,
                @Description,
                @Status,
                @UserID
            )";

                SqlCommand cmd =
                    new SqlCommand(sql, cn);

                cmd.Parameters.AddWithValue(
                    "@Title",
                    data.Title);

                cmd.Parameters.AddWithValue(
                    "@Description",
                    data.Description);

                cmd.Parameters.AddWithValue(
                    "@Status",
                    data.Status);

                cmd.Parameters.AddWithValue(
                    "@UserID",
                    data.UserID);

                int result =
                    cmd.ExecuteNonQuery();

                return result > 0;
            }
        }

        public bool Update(TrelloMini.Models.Task data)
        {
            using (SqlConnection cn =
                new SqlConnection(_connectionString))
            {
                cn.Open();

                string sql = @"
            UPDATE Tasks
            SET
                Title = @Title,
                Description = @Description,
                Status = @Status,
                UserID = @UserID
            WHERE TaskID = @TaskID
        ";

                SqlCommand cmd =
                    new SqlCommand(sql, cn);

                cmd.Parameters.AddWithValue("@TaskID", data.TaskID);
                cmd.Parameters.AddWithValue("@Title", data.Title);
                cmd.Parameters.AddWithValue("@Description", data.Description);
                cmd.Parameters.AddWithValue("@Status", data.Status);
                cmd.Parameters.AddWithValue("@UserID", data.UserID);

                int result = cmd.ExecuteNonQuery();

                return result > 0;
            }
        }

        public bool Delete(int id)
        {
            return true;
        }
    }
}