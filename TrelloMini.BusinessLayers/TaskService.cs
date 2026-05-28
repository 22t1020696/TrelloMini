using System.Collections.Generic;
using TrelloMini.DataLayers;

namespace TrelloMini.BusinessLayers
{
    public class TaskService
    {
        private readonly ITaskDAL _taskDB;

        public TaskService(ITaskDAL taskDB)
        {
            _taskDB = taskDB;
        }

        // =========================
        // LIST THEO USER
        // =========================
        public List<TrelloMini.Models.Task> List(int userId)
        {
            return _taskDB.List(userId);
        }

        // =========================
        // ADD
        // =========================
        public bool Add(TrelloMini.Models.Task data)
        {
            return _taskDB.Add(data);
        }

        // =========================
        // UPDATE
        // =========================
        public bool Update(TrelloMini.Models.Task data)
        {
            return _taskDB.Update(data);
        }

        // =========================
        // DELETE
        // =========================
        public bool Delete(int id)
        {
            return _taskDB.Delete(id);
        }
    }
}
