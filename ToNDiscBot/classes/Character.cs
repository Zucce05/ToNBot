using Discord;
using Discord.WebSocket;
using System;
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
        public bool AllowRandom => true; // I think this syntax is allowed.  If it yells at you do the following line
		// public bool AllowRandom {get { return true; } }

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
