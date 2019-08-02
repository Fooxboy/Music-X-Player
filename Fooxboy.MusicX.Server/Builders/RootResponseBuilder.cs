using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Server.Models;

namespace Fooxboy.MusicX.Server.Builders
{
    public static class RootResponseBuilder
    {
        public static RootResponse<T> Build<T>(T response)
        {
            var responseModel = new RootResponse<T>();
            responseModel.Result = response;
            responseModel.Status = true;
            return responseModel;
        }


        public static RootResponse<string> BuildError(Error error)
        {
            var responseModel = new RootResponse<string>();
            responseModel.Status = false;
            responseModel.Result = "Произошла ОШИБКА!";
            responseModel.Error = error;
            return responseModel;
        }
    }
}
