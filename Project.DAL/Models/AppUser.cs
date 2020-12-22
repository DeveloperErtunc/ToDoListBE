using Microsoft.AspNetCore.Identity;


namespace Project.DAL.Models
{
   public class AppUser:IdentityUser<int>
    {
        public string NameSurName { get; set; }
        public long Assignee { get; set; }

        public string Role { get; set; }

    }
}
