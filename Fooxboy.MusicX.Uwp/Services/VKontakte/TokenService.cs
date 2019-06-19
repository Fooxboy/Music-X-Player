using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Newtonsoft.Json;
using Windows.Storage;

namespace Fooxboy.MusicX.Uwp.Services.VKontakte
{
    public static class TokenService
    {

        public async static Task Save(string token)
        {
            var tokenModel = new TokenFile()
            {
                Token = token
            };

            StorageFile fileToken = null;

            
            try
            {
                fileToken = await StaticContent.LocalFolder.GetFileAsync("token.json");
            }catch
            {
                fileToken = await StaticContent.LocalFolder.CreateFileAsync("token.json");
            }

            var json = JsonConvert.SerializeObject(tokenModel);

            await FileIO.WriteTextAsync(fileToken, json);
        }

        public async static Task<TokenFile> Load()
        {
            var fileToken = await StaticContent.LocalFolder.GetFileAsync("token.json");
            var json = await FileIO.ReadTextAsync(fileToken);
            var model = JsonConvert.DeserializeObject<TokenFile>(json);
            return model;
        }


        public async static Task Delete()
        {
            var fileToken = await StaticContent.LocalFolder.GetFileAsync("token.json");
            await fileToken.DeleteAsync();
        }

        
    }
}
