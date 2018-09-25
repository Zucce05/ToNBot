using Discord.WebSocket;
using System.Threading.Tasks;

namespace ToNDiscBot
{
    public interface IBotCalls
    {
        Task SendChannelMessageAsync(SocketMessage message);
		bool AllowRandom { get; }
    }
}
