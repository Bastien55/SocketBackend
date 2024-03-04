﻿using SocketBackend.Enumeration;
using SocketBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketBackend.Messages
{
    public class UserMessage : Message
    {
        public string Rule { get; set; } = string.Empty;

        public User UserModel { get; set; }

        public UserMessage(string name, string message, TypeMessage type, string rule) : base(name, message, type)
        {
            UserModel = new User() { 
                Name = name,
                Rule = rule
            };

            Rule = rule;
        }

        public UserMessage(User userModel, TypeMessage type) : base(type)
        {
            UserModel = userModel;
        }

        public override string ToString()
        {
            return base.ToString() + $";{Rule}";
        }
    }
}
