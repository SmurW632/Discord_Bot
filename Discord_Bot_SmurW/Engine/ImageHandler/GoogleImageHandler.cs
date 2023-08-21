using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot_SmurW.Engine.ImageHandler
{
    public class GoogleImageHandler
    {
        public Dictionary<int, string> images = new Dictionary<int, string>();
        public static readonly GoogleImageHandler Instance = new GoogleImageHandler();

        static GoogleImageHandler() { }

        public string GetImageAtId(int id)
        {
            _ = images.TryGetValue(id, out var img);
            return img!;
        }
    }
}
