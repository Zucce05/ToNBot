using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ToNDiscBot.classes
{
    public class Character : IBotCalls
    {
        public string CharacterName { get; set; }
        public string ImageUrl { get; set; }
        public string CharacterDescription { get; set; }
        public string CharacterQuote { get; set; }
        public string Lore { get; set; }
        //public Color EmbedColor { get; set; }

        public async Task SendChannelMessageAsync(SocketMessage message)
        {
            var builder = new EmbedBuilder()
            {
                Color = Color.Blue,
                Title = "Tales of Nowhere",
                Description = $"Famous Saying: '{this.CharacterQuote}'",
                ImageUrl = $"{this.ImageUrl}",
                Timestamp = DateTimeOffset.Now,
            }
                            .WithFooter(footer => footer.Text = $"{this.CharacterDescription}")
                            .AddField("Name: ", $"{this.CharacterName}");

            await message.Channel.SendMessageAsync(this.Lore, false, builder.Build());
        }
    }
}
