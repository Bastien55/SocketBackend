using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketBackend.Enumeration
{
    public enum TypeMessage
    {
        UNKNOW = -1,
        GAME_REPLAY,
        VALID_RULE,
        INVALID_RULE,
        MSG_CHAT
    }
}
