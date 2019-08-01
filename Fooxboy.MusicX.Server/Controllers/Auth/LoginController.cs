using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Server.Models;
using Fooxboy.MusicX.Core.Server.Models.Api.Auth;
using Fooxboy.MusicX.Server.Builders;
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
            var response = new Login() {AccessToken = Generator.Generate(new string[] {login, password})};

            return Json(RootResponseBuilder.Build<Login>(response));
        }
    }
}