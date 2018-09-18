using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace ToNDiscBot
{
    public class InfoModule : ModuleBase
    {
        public InfoModule()
        {
        }

        //// ~sample square 20 -> 400
        //[Command("square")]
        //[Summary("Squares a number.")]
        //public async Task SquareAsync(
        //    [Summary("The number to square.")]
        //int num)
        //{
        //    // We can also access the channel from the Command Context.
        //    await Context.Channel.SendMessageAsync($"{num}^2 = {Math.Pow(num, 2)}");
        //}

        //// ~sample userinfo --> foxbot#0282
        //// ~sample userinfo @Khionu --> Khionu#8708
        //// ~sample userinfo Khionu#8708 --> Khionu#8708
        //// ~sample userinfo Khionu --> Khionu#8708
        //// ~sample userinfo 96642168176807936 --> Khionu#8708
        //// ~sample whois 96642168176807936 --> Khionu#8708
        //[Command("userinfo")]
        //[Summary
        //("Returns info about the current user, or the user parameter, if one passed.")]
        //[Alias("user", "whois")]
        //public async Task UserInfoAsync(
        //    [Summary("The (optional) user to get info from")]
        //SocketUser user = null)
        //{
        //    var userInfo = user ?? Context.Client.CurrentUser;
        //    await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
        //}


        [Command("embed")]
        public async Task SendRichEmbedAsync()
        {
            var embed = new EmbedBuilder
            {
                // Embed property can be set within object initializer
                Title = "Hello world!",
            Description = "I am a description set by initializer."
            };
            // Or with methods
            embed.AddField("Field title",
                "Field value. I also support [hyperlink markdown](https://example.com)!")
                .WithAuthor(Context.Client.CurrentUser)
                .WithFooter(footer => footer.Text = "I am a footer.")
                .WithColor(Color.Blue)
                .WithTitle("I overwrote \"Hello world!\"")
                .WithDescription("I am a description.")
                .WithUrl("https://example.com")
                .WithCurrentTimestamp()
                .Build();
            //Console.WriteLine(embed.ToString());
            await ReplyAsync("Test");
        }
    }
}
