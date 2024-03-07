using SocketBackend.Enumeration;
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

        public TypeMessage TypeMessage { get; set; }  

        public Message(string name, string message, TypeMessage type)
        {
            Name = name;
            ContentMessage = message;
            TypeMessage = type;
        }

        public Message(TypeMessage type) 
        {
            TypeMessage = type;
            Name = string.Empty;
            ContentMessage = string.Empty;
        }

        public override string ToString()
        {
            return $"{Name};{ContentMessage};{TypeMessage}";
        }
    }
}
