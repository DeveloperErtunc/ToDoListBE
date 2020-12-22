using Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.BLL.APIModels
{
    public class ToDoTaskResponse:ToDoTaskBase
    {
        public ToDoTaskResponse()
        {
            due = new due_date();
            TaskDetail = new TaskDetail();
        }

        public string created { get; set; }
        public Boolean completed { get; set; }
        
        public due_date due { get; set; }
        public class due_date
        {
            public string date { get; set; }
        }

        TaskDetail TaskDetail = new TaskDetail();
    }
}
