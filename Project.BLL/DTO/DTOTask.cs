using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.BLL.DTO
{
    public class DTOTask
    {

        public long id { get; set; }
  
        public long project_id { get; set; }

        public string content { get; set; }

        public bool Status { get; set; }

        public string Title { get; set; }

        public string due_date { get; set; }

        public string date { get; set; }

        public long assignee { get; set; }
    }
}
