using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketBackend
{
    public class Message
    {
        /// <summary>
        /// Sender's name of the message
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string ContentMessage { get; set; }

        public Message(string name, string message)
        {
            Name = name;
            ContentMessage = message;
        }
    }
}
