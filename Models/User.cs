using System;
using System.Collections.Generic;

namespace SocketBackend.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Rule { get; set; }
    }
}
