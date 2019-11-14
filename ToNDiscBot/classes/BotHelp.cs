using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace ToNDiscBot
{
    public class BotHelp : IBotCalls
    {
        public bool AllowRandom => false;
        public string HelpDescription { get; set; }
        public string HelpExample { get; set; }

        public async Task SendChannelMessageAsync(SocketMessage message)
        {
            var builder = new EmbedBuilder()
            {
                Color = Color.Red,
                Title = "Tales of Nowhere Help",
                Description = $"{this.HelpDescription}",
                Timestamp = DateTimeOffset.Now,
            }
                            .AddField("Example: ", $"{this.HelpExample}");

            await message.Channel.SendMessageAsync(string.Empty, false, builder.Build());
        }
    }
}
