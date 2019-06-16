﻿using System;
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

            if (StaticContent.LocalFolder.TryGetItemAsync("token.json") == null) fileToken = await StaticContent.LocalFolder.CreateFileAsync("token.json");
            else fileToken = await StaticContent.LocalFolder.GetFileAsync("token.json");

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
    }
}