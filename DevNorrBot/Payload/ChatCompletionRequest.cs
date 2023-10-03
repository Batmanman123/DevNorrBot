using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNorrBot.Payload
{
    public class ChatCompletionRequest
    {
        public string Model { get; set; }
        public List<Message> Messages { get; set; }
        public double Temperature { get; set; }
    }
    public class Message
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}

