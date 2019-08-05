using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Server.Models;
using Fooxboy.MusicX.Core.Server.Models.Api.Auth;
using Fooxboy.MusicX.Server.Builders;
using Fooxboy.MusicX.Server.Databases;
using Fooxboy.MusicX.Server.Generators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fooxboy.MusicX.Server.Controllers.Auth
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {

        private readonly IGenerator<string, string[]> Generator;

        public LoginController(IGenerator<string, string[]> generator)
        {
            Generator = generator;
        }

        [HttpGet]
        public JsonResult Get(string login, string password)
        {
            using (var db = new DataContext())
            {
                try
                {
                    if (db.Users.Any(user => user.Login == login))
                    {
                        var user = db.Users.Single(u => u.Login == login);
                        if (password == user.Password)
                        {
                            var token = Generator.Generate(new string[] {login, password});
                            var response = new Login() {AccessToken = token};
                            user.CurrentAccountToken = token;
                            db.SaveChanges();
                            return Json(RootResponseBuilder.Build<Login>(response));
                        }
                        else
                        {
                            var errorInfo = new Error();
                            errorInfo.Title = "Неверный пароль";
                            errorInfo.Code = 2;
                            errorInfo.Description = "Ваш пароль неверный, попробуйте ещё раз.";
                            return Json(RootResponseBuilder.BuildError(errorInfo));
                        }
                    }
                    else
                    {
                        var errorInfo = new Error();
                        errorInfo.Title = "Пользователя с таким логином не существует";
                        errorInfo.Code = 3;
                        errorInfo.Description = "Возможно, Вы ввели неверный логин. Попробуйте ещё раз.";
                        return Json(RootResponseBuilder.BuildError(errorInfo));
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

            

            
        }
    }
}