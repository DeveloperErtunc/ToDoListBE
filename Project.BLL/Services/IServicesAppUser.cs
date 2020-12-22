using Microsoft.AspNetCore.Identity;
using Project.BLL.DTO;
using Project.BLL.Mapper;
using Project.BLL.ResponseClasses;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Project.DAL.Context;
using Project.DAL.Models;
using System.Collections.Generic;
using static Project.BLL.DTO.DTOAppUser;

namespace Project.BLL.Services
{
    public interface IServicesAppUser
    {

        Task<UserResponse> RegisterUserAsync(DTOAppUser _DTOAppUser);
        Task<UserResponse> LoginUserAsync(DTOAppUser dtoAppUser);
        Task<UserResponse> ToAdmin(DTOAppUser dtoAppUser);

        public class ServicesAppUser : IServicesAppUser
        {
            #region Variable
            private UserManager<AppUser> _userManager;
            private IConfiguration _configuration;
            private readonly MyContext _dataBase;
            #endregion

            public ServicesAppUser(MyContext dataBase, UserManager<AppUser> userManager, IConfiguration configuration)
            {
                _dataBase = dataBase;
                _configuration = configuration;
                _userManager = userManager;
            }

            public async Task<UserResponse> RegisterUserAsync(DTOAppUser _DTOAppUser)
            {
                #region Validation
                if (_dataBase.Users.Where(x => x.Email == _DTOAppUser.Email).ToList().Count != 0)
                {
                    return new UserResponse
                    {
                        Message = "Already " + _DTOAppUser.Email + " is used",
                        IsSuccess = false,

                    };
                    
                }
                if (_dataBase.Users.Where(x => x.UserName == _DTOAppUser.UserName).ToList().Count != 0)
                {
                    return new UserResponse
                    {
                        Message = "Already " + _DTOAppUser.UserName + " is used",
                        IsSuccess = false,

                    };

                }

                #endregion

                #region SigUp
                ClaimUser claimUser = new ClaimUser();
                claimUser.ClaimValue = "User";
                var result = await
                _userManager.CreateAsync(MapperUser.ToAppUser(_DTOAppUser), _DTOAppUser.Password);

                if (result.Succeeded)
                    {
                        AppUser user2 = await _userManager.FindByEmailAsync
                            (email: _DTOAppUser.Email);
                        claimUser.UserId = user2.Id;
                        _dataBase.UserClaims.Add(claimUser);
                        _dataBase.SaveChanges();

                        return new UserResponse
                        {
                            Message = "Üyelik Oluşturuldu",
                            IsSuccess = true

                        };
                    }
                    else
                    {
                        List<string> listError = new List<string>();
                        foreach( var response in result.Errors )
                        {
                            listError.Add(response.Description);
                        }
                        return new UserResponse
                        {
                            Erorrs = listError,
                            IsSuccess = false
                        };
                    }
                #endregion
            }
            public async Task<UserResponse> LoginUserAsync(DTOAppUser model)
            {
                #region Validation

                AppUser user = _dataBase.Users.Where(x => x.Email == model.Email).FirstOrDefault();
                if (user == null)
                {
                    return new UserResponse
                    {
                        Message = "Password or Email is wrong",
                        IsSuccess = false

                    };
                }

                var result = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!result)
                {
                    return new UserResponse
                    {
                        Message = "Password or Email is wrong",
                        IsSuccess = false
                    };
                }
                #endregion
                #region Claim and Token
                var bytes = Encoding.UTF8.GetBytes("ertunc123ertunc123ertunc123");
                SymmetricSecurityKey key = new SymmetricSecurityKey(bytes);
                List<Claim> listclaim = new List<Claim>();
                List<string> listclaim2 = new List<string>();
                foreach (var item in _dataBase.UserClaims.ToList().Where(x => x.Id == user.Id))
                {
                    listclaim.Add(new Claim(ClaimTypes.Role,item.ClaimValue));
                    listclaim2.Add(item.ClaimValue);
                } 
              

                SigningCredentials _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                JwtSecurityToken _securityToken = new JwtSecurityToken
                    (
                        issuer: "http://localhost",
                        audience: "http://localhost",
                        notBefore: DateTime.Now,
                        expires: DateTime.Now.AddHours(30),
                        signingCredentials: _signingCredentials,
                        claims: listclaim
                    );
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

                return new UserResponse
                {
                    ClaimRole = listclaim2,
                    Message = jwtSecurityTokenHandler.WriteToken(_securityToken),
                    IsSuccess = true,
                };
                #endregion

                #region Token Way 1
                /*var claims = new[]
                {
                new Claim("Email",model.Email),
                new Claim(ClaimTypes.NameIdentifier,Convert.ToString(user.Id))
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["AuthSettings:Issuer"],
                    audience: _configuration["AuthSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                    );
                string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
                return new UserManagerResponse
                {
                    Message = tokenAsString,
                    IsSuccess = true,
                    Date = token.ValidTo
                }; */
                #endregion
            }

            public async Task<UserResponse> ToAdmin(DTOAppUser dtoAppUser)
            {
                try
                {
                    ClaimUser claimUser = new ClaimUser();
                    claimUser.ClaimValue = "Admin";
                    AppUser user = await _userManager.FindByNameAsync
                            (userName: dtoAppUser.UserName);
                    claimUser.UserId = user.Id;
                    _dataBase.UserClaims.Add(claimUser);
                    _dataBase.SaveChanges();

                    return new UserResponse
                    {
                        Message = "Admin Olarak atandı"
                    };
                }

                catch
                {
                    return new UserResponse
                    {
                        Message = "Admin Atanamadı"
                    };
                }
            
            }

        }
    }
}
