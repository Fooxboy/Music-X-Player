using Fooxboy.MusicX.Core.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core.Helpers
{
    public static class VkBlockHelper
    {

        public static ResponseVk Proccess(this ResponseVk response)
        {

            foreach(var block in response.Response.Section.Blocks)
            {
                if(block.AudiosIds != null || block.AudiosIds.Count > 0)
                {
                    foreach(var audioId in block.AudiosIds)
                    {

                    }
                }
            }

            return response;
        }
    }
}
