using Project.BLL.DTO;
using Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.BLL.Mapper
{
    public static class MapperUser
    {
        public static AppUser ToAppUser(DTOAppUser _DTOAppUser)
        {
            AppUser _AppUser = new AppUser();

            _AppUser.Email = _DTOAppUser.Email;
            _AppUser.NameSurName = _DTOAppUser.NameSurName;
            _AppUser.UserName = _DTOAppUser.UserName;
            _AppUser.Id = _DTOAppUser.Id; 

            return _AppUser;
        }

        public static List<AppUser> ToListAppUser(List<DTOAppUser> _ListDTOAppUser)
        {
            List<AppUser> _ListAppUser = new List<AppUser>();
            foreach (DTOAppUser item in _ListDTOAppUser)
            {
                _ListAppUser.Add(ToAppUser(item));
            }
            return _ListAppUser;
        }

        public static DTOAppUser ToDTOAppUser(AppUser _ApppUser)
        {
            
            DTOAppUser _DTOAppUser = new DTOAppUser();
   
            _DTOAppUser.UserName = _ApppUser.UserName;
            _DTOAppUser.Id = _ApppUser.Id;
            _DTOAppUser.NameSurName = _ApppUser.NameSurName;
            _DTOAppUser.Email = _ApppUser.Email;
            _DTOAppUser.Assignee = _ApppUser.Assignee;
            _DTOAppUser.Role = _ApppUser.Role;
            return _DTOAppUser;
        }

        public static List<DTOAppUser> ToListDTOAppUser(List<AppUser> _ListAppUser)
        {
            List<DTOAppUser> _ListDTOAppUser = new List<DTOAppUser>();
            foreach (AppUser item in _ListAppUser)
            {
               _ListDTOAppUser.Add(ToDTOAppUser(item));
            }
            return _ListDTOAppUser;
        }
    }
}
