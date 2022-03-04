using Fooxboy.MusicX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fooxboy.MethodsTest
{
    public class RecomendationsTest
    {

        public readonly Api api;

        public RecomendationsTest()
        {
            var vkToken = "1ee1c5bdac35313373684b1f4b14e1f6c41040ffcc59d4640494ade97d7b8f4ce64483535225dd5de81f8";
            api = Api.GetApi(new LoggerService());

            api.VKontakte.Auth.Auto(vkToken, null);
        }


        [Fact]
        public async Task NewRecomendationsTest()
        {
            //var res = await api.VKontakte.Music.Recommendations.GetHomeAsync().ConfigureAwait(false);

            //var res = await api.VKontakte.Music.Blocks.GetAsync("PUlQVA8GR0R3W0tMF0QHBz8HAAVBRzQUIwgGG0YWR0R-SVNFBQxcUHJcUUAZFl5EfEkOE1tRGQcqSUVUARZRV2pJWEMXDlpKZFlfVA8HW15xXV1BDQIW").ConfigureAwait(false);


            //var res = await api.VKontakte.Music.Catalog.GetSectionById("PUldVA8FR0RzSVNUagZJSmRSS0wEGEleZFFdQwAGUlNyU1AL");


            var res = await api.VKontakte.Music.Catalog.GetBlockItems("PUlQVA8GR0R3W0tMF0QHBz8HAAVBRzQUIwgGG0YWR0R-SVNFBQxcUHJcUUAZFl5EfEkOE1tRGQcqSUVUARZRV2pJWEMXDlpKZFlfVA8HW15xXV1BDQIW");
            Assert.True(true);
        }
    }
}
