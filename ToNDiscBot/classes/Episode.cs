using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace ToNDiscBot.classes
{
    public class Episode : IBotCalls
    {
        public string EpisodeTitle { get; set; }
        public string EpisodeDescription { get; set; }
        public string EpisodeLength { get; set; }
        public string NPCs { get; set; }
        public string RuleSet { get; set; }
        //public Color EmbedColor { get; set; }
        public bool AllowRandom => true; // I think this syntax is allowed.  If it yells at you do the following line
                                         // public bool AllowRandom {get { return true; } }

        public async Task SendChannelMessageAsync(SocketMessage message)
        {
            var builder = new EmbedBuilder()
            {
                Color = Color.Blue,
                Title = $"{EpisodeTitle}",
                Description = $"Episode Description: '{this.EpisodeDescription}'",
                //ImageUrl = $"{this.ImageUrl}",
                Timestamp = DateTimeOffset.Now,
            }
                            .WithFooter(footer => footer.Text = $"Ruleset: {this.RuleSet}")
                            .AddField("Key NPCs: ", $"{this.NPCs}")
                            .AddField($"Episode Length:", $"{this.EpisodeLength}");

            await message.Channel.SendMessageAsync(string.Empty, false, builder.Build());
        }
    }
}
