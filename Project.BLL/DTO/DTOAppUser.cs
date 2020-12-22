using System.Collections.Generic;
namespace Project.BLL.DTO
{
    public class DTOAppUser
    {
        public DTOAppUser()
        {
        }
        public int Id { get; set; }

        public string UserName { get; set; }

        public string NameSurName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public long Assignee { get; set; }

        public string Role { get; set; }

    }
}
