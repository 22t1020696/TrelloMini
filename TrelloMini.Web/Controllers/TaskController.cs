using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

using TrelloMini.BusinessLayers;
using TrelloMini.Web.AppCodes;

namespace TrelloMini.Web.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        // =========================
        // INDEX
        // =========================
        public IActionResult Index()
        {
            var userData = User.GetUserData();

            if (userData == null)
            {
                return RedirectToAction(
                    "Login",
                    "Auth"
                );
            }

            int userId =
                Convert.ToInt32(userData.UserID);

            var tasks =
                _taskService.List(userId);

            return View(tasks);
        }

        // =========================
        // CREATE
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            TrelloMini.Models.Task model)
        {
            var userData = User.GetUserData();

            if (userData == null)
            {
                return RedirectToAction(
                    "Login",
                    "Auth"
                );
            }

            model.UserID =
                Convert.ToInt32(userData.UserID);

            model.Status = "TODO";

            _taskService.Add(model);

            return RedirectToAction("Index");
        }

        // =========================
        // CHANGE STATUS
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStatus(
            int id,
            string status)
        {
            var userData = User.GetUserData();

            if (userData == null)
            {
                return RedirectToAction(
                    "Login",
                    "Auth"
                );
            }

            int userId =
                Convert.ToInt32(userData.UserID);

            var tasks =
                _taskService.List(userId);

            var task =
                tasks.FirstOrDefault(
                    x => x.TaskID == id
                );

            if (task != null)
            {
                task.Status = status;

                _taskService.Update(task);
            }

            return RedirectToAction("Index");
        }
    }
}

