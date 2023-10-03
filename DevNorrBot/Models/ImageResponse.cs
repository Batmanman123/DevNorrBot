using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNorrBot.Models
{
    internal class ImageResponse
    {
        public long created { get; set; }
        public List<ImageData> data { get; set; }
    }

    public class ImageData
    {
        public string url { get; set; }
    }
}
