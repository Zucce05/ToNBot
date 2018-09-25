using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace ToNDiscBot
{
    class BotHelp : IBotCalls
    {
        public bool AllowRandom => false;

        public Task SendChannelMessageAsync(SocketMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
