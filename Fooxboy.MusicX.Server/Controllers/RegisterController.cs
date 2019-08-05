using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Server.Models;
using Fooxboy.MusicX.Core.Server.Models.Api.Auth;
using Fooxboy.MusicX.Server.Builders;
using Fooxboy.MusicX.Server.Databases.DataModels;
using Fooxboy.MusicX.Server.Generators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fooxboy.MusicX.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : Controller
    {

        private readonly IGenerator<string, string[]> Generator;

        public RegisterController(IGenerator<string, string[]> generator)
        {
            Generator = generator;
        }

        [HttpGet]
        public JsonResult Get(string login = null, string password= null)
        {
            using (var db = new Databases.DataContext())
            {
                try
                {
                    if (db.Users.Any(user => user.Login.ToLower() == login.ToLower()))
                    {
                        //Todo: возвращаем инфу что такой пидор уже есть
                        var errorInfo = new Error()
                        {
                            Code = 1,
                            Title = "Пользователь с таким логином уже существует.",
                            Description = "Попробуйте выбрать другой логин."
                        };

                        return Json(RootResponseBuilder.BuildError(errorInfo));
                    }
                    else
                    {
                        var accessToken = Generator.Generate(new string[] {login, password});
                        db.Users.Add(new User()
                        {
                            CurrentAccountToken = accessToken,
                            CurrentVkToken = "no",
                            Id = new Random().Next(1, 99999),
                            Login = login,
                            Password = password
                        });

                        db.SaveChanges();
                        var response = new Login() {AccessToken = accessToken};
                        return Json(RootResponseBuilder.Build<Login>(response));
                    }
                }
                catch (Exception e)
                {
                    var errorInfo = new Error()
                    {
                        Code = 0,
                        Description = e.ToString(),
                        Title = "Произошла неизвестная ошибка."
                    };
                    return Json(RootResponseBuilder.BuildError(errorInfo));
                }
                
            }


            //return new string[] { "value1", "value2" };
        }
    }
}