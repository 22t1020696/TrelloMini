using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrelloMini.Models
{
    public class Task
    {
        public int TaskID { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public string Status { get; set; } = "";

        public int UserID { get; set; }
    }
}