using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.BLL.APIModels
{
    public class ToDoTaskBase
    {
        public long id { get; set; }
        public long assignee  { get; set; }
        public long project_id { get; set; }
        public string content { get; set; }
    }
}

