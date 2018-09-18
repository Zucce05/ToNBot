using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using ToNDiscBot.classes;

namespace ToNDiscBot
{
    public class Program
    {
        DiscordSocketClient client;
        BotConfig botConfig = new BotConfig();
        List<Character> characters;


        public static void Main(string[] args)
                    => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            client = new DiscordSocketClient
            (new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
                //LogLevel = LogSeverity.Verbose
                //LogLegel = LogSeverity.Info
            });

            SetUp(ref botConfig, ref characters);

            client.Log += Log;
            string token = botConfig.Token;

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            client.MessageReceived += MessageReceived;

            await Task.Delay(-1);

        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content.StartsWith("^"))
            {
                string msg = message.Content.Substring(1).ToLower();

                string[] substring = msg.Split(" ");
                foreach (string s in substring)
                {
                    Console.WriteLine(s);
                }
                switch (substring[0])
                {
                    case "character":
                        var builder = new EmbedBuilder()
                        {
                            // Embed property can be set within object initializer
                            //Title = "Hello world!",
                            //Description = "I am a description set by initializer."
                        };
                        
                        foreach (Character c in characters)
                        {
                            if (substring[1] == c.CharacterName.ToLower())
                            {
                                Embed embed = builder.AddField("Name: ", $"{c.CharacterName}")
                            .WithFooter(footer => footer.Text = $"{c.CharacterDescription}")
                            .WithColor(Color.Blue)
                            .WithTitle("Tales of Nowhere")
                            .WithDescription($"Famous Saying: '{c.CharacterQuote}'")
                            .WithImageUrl($"{c.ImageUrl}")
                            .WithCurrentTimestamp()
                            .Build();
                                await message.Channel.SendMessageAsync(string.Empty, false, embed);
                            }
                        }

                        //var builder = new EmbedBuilder();
                        //builder.Title = "Hello world!";
                        //builder.Description = "I am a description set by initializer.";

                        // Or with methods
                        //Embed embed = builder.AddField("Field title",
                        //    "Field value. I also support [hyperlink markdown](https://example.com)!")
                        //    .WithFooter(footer => footer.Text = "I am a footer.")
                        //    .WithColor(Color.Blue)
                        //    .WithTitle("I overwrote \"Hello world!\"")
                        //    .WithDescription("I am a description.")
                        //    .WithUrl("https://example.com")
                        //    .WithImageUrl("http://machshare.azurewebsites.net/Mach.png")
                        //    .WithCurrentTimestamp()
                        //    .Build();

                        //await message.Channel.SendMessageAsync(testChar.embed); //I know this isn't working yet. It's where I'm trying to call from.
                        //await message.Channel.SendMessageAsync($"{message.Author.Mention} No!");
                        //await message.Channel.SendFileAsync("..\\..\\..\\media\\images\\Armitage.png", "Armitage Relative");
                        //await message.Channel.SendMessageAsync("This is text", false, embed);
                        //await message.Channel.SendFileAsync("https://1drv.ms/u/s!AjBERSB_cyheuop804_3TgFapgUr5A", "Armitage URL");

                        break;
                }
            }
        }

        public static void SetUp(ref BotConfig bc, ref List<Character> chars)
        {
            JsonTextReader conf;
            try
            {
                // This is good for development where I've got the config with the project
                conf = new JsonTextReader(new StreamReader("..\\..\\..\\BotConfig.json"));
                bc = JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText("..\\..\\..\\BotConfig.json"));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Project Level SetUp Exception:\n\t{e.Message}");
            }
            //try
            //{
            //    // This is good for deployment where I've got the config with the executable
            //    conf = new JsonTextReader(new StreamReader("BotConfig.json"));
            //    bc = JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText("BotConfig.json"));
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine($"Executable Level SetUp Exception:\n\t{e.Message}");
            //}

            try
            {
                // This is good for development where I've got the config with the project
                conf = new JsonTextReader(new StreamReader("..\\..\\..\\characters.json"));
                chars = JsonConvert.DeserializeObject<List<Character>>(File.ReadAllText("..\\..\\..\\characters.json"));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Project Level Character[] Initilization Exception:\n\t{e.Message}");
            }
        }

        //[Command("embed")]
        //public async Task SendRichEmbedAsync()
        //{
        //    var embed = new EmbedBuilder
        //    {
        //        // Embed property can be set within object initializer
        //        Title = "Hello world!",
        //            Description = "I am a description set by initializer."
        //    };
        //    // Or with methods
        //    embed.AddField("Field title",
        //        "Field value. I also support [hyperlink markdown](https://example.com)!")
        //        .WithAuthor(Context.Client.CurrentUser)
        //        .WithFooter(footer => footer.Text = "I am a footer.")
        //        .WithColor(Color.Blue)
        //        .WithTitle("I overwrote \"Hello world!\"")
        //        .WithDescription("I am a description.")
        //        .WithUrl("https://example.com")
        //        .WithCurrentTimestamp()
        //        .Build();
        //    //    await ReplyAsync(embed: embed);
        //    await ReplyAsync(embed);
        //}

    }
}
