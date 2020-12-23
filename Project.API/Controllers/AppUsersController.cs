using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTO;
using Project.BLL.Services;
using Project.BLL.ResponseClasses;
using Project.DAL.Models;
using Project.DAL.Context;
using Project.BLL.Mapper;
using Microsoft.AspNetCore.Authorization;

namespace Project.API.Controllers
{
    
    [ApiController]
    [Route("api/users")]
    public class AppUsersController : ControllerBase
    {
        SignInManager<AppUser> _signInManager;

        IServicesAppUser _servicesAppUser;
        MyContext _database;
        public AppUsersController(IServicesAppUser servicesAppUser, MyContext database,SignInManager<AppUser> signInManager)
        {
            _servicesAppUser = servicesAppUser;
            _database = database;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<UserResponse> Register(DTOAppUser dTOAppUser)
        {
            return (await _servicesAppUser.RegisterUserAsync(dTOAppUser));
        }

        [HttpGet]
        [Route("users/id")]
        public List<DTOAppUser> GetUsers(int id)
        {
            List<DTOAppUser> appUser = new List<DTOAppUser>();
            foreach(var  item  in _database.Users)
            {
                if(item.Id != id)
                {
                    appUser.Add(MapperUser.ToDTOAppUser(item));
                }
            }
            return appUser;
        }

        [HttpGet]
        [Route("id")]
        public DTOAppUser UserDetail(int id)
        
        {
          
            foreach (var item in _database.Users)
            {
                if (item.Id == id)
                {
                  return  MapperUser.ToDTOAppUser(item);
                }
            }
            return null;
        }

         [HttpPost]
         [Route("login")]
         public async Task<UserResponse> Login(DTOAppUser dTOAppUser)
         {

             return(await _servicesAppUser.LoginUserAsync(dTOAppUser));
         }

        [HttpPost]
        [Route("toadmin")]
        public async Task<UserResponse> ToAdmin(DTOAppUser dTOAppUser)
        {
            return (await _servicesAppUser.ToAdmin(dTOAppUser));

        }
    }
}
