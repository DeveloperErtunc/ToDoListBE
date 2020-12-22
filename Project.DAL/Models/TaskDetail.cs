using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DAL.Models
{
    public class TaskDetail
    {
        public int ID { get; set; }

        public long IDTask { get; set; }

        public bool Status { get; set; }

        public string Title { get; set; }
    }
}
