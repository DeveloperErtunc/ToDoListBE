using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.BLL.APIModels
{
    public class ToDoTaskRequest:ToDoTaskBase
    {
        public string due_date { get; set; }

    }
}
