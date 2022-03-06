using Fooxboy.MusicX.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fooxboy.MethodsTest
{
    public class VkTests
    {
        [Fact]
        public async Task GetAudioCatalogTest()
        {
            var vkSevice = new VkService(null);

            await vkSevice.SetTokenAsync("1ee1c5bdac35313373684b1f4b14e1f6c41040ffcc59d4640494ade97d7b8f4ce64483535225dd5de81f8", null);

            var catalog = await vkSevice.GetAudioCatalogAsync();

            Assert.True(catalog.Catalog != null && catalog.Catalog.Sections.Count > 0);
        }

        [Fact]
        public async Task GetSectionTest()
        {
            var vkSevice = new VkService(null);

            await vkSevice.SetTokenAsync("1ee1c5bdac35313373684b1f4b14e1f6c41040ffcc59d4640494ade97d7b8f4ce64483535225dd5de81f8", null);

            var catalog = await vkSevice.GetSectionAsync("PUldVA8FW0pkXktMF2tfVmRHS08XDlpKZF1LTBcMWFNwXlhPBwNYUnJSW04EA19EOw");

            Assert.True(catalog.Audios.Count > 0);
        }

        [Fact]
        public async Task GetPlaylistTest()
        {
            var vkSevice = new VkService(null);

            await vkSevice.SetTokenAsync("1ee1c5bdac35313373684b1f4b14e1f6c41040ffcc59d4640494ade97d7b8f4ce64483535225dd5de81f8", null);

            var plist = await vkSevice.GetPlaylistAsync(100, 84615054, "60192863dd2492b9f1", -143914468);

            Assert.True(plist.Playlist != null);
        }


        [Fact]
        public async Task AddAudioTest()
        {
            var vkSevice = new VkService(null);

            await vkSevice.SetTokenAsync("1ee1c5bdac35313373684b1f4b14e1f6c41040ffcc59d4640494ade97d7b8f4ce64483535225dd5de81f8", null);

            await vkSevice.AudioAddAsync(106983262, -2001983262);

            Assert.True(true);
        }

        [Fact]
        public async Task GetArtistAudioTest()
        {
            var vkSevice = new VkService(null);

            await vkSevice.SetTokenAsync("1ee1c5bdac35313373684b1f4b14e1f6c41040ffcc59d4640494ade97d7b8f4ce64483535225dd5de81f8", null);

            var res = await vkSevice.GetAudioArtistAsync("835651927344928174");

            Assert.True(true);
        }
    }
}
