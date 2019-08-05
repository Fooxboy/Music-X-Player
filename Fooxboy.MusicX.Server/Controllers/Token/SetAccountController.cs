using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fooxboy.MusicX.Server.Controllers.Token
{
    [Route("api/token/[controller]")]
    [ApiController]
    public class SetAccountController : Controller
    {
        [HttpGet]
        public JsonResult Get(string token = null, string access_token = null)
        {
            //Todo: сохранение вк токена
            if (token == null)
                return null;
            return null;
        }
    }
}