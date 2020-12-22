using System;
using System.Collections.Generic;
using System.Text;

namespace Project.BLL.ResponseClasses
{
    public class UserResponse
    {
        public UserResponse()
        {
            Erorrs = new List<string>();
        }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Erorrs { get; set; }
        public List<string> ClaimRole { get; set; }
    }
}
