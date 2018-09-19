using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using ToNDiscBot.classes;

namespace ToNDiscBot
{
    public class Program
    {
        DiscordSocketClient client;
        BotConfig botConfig = new BotConfig();

        Dictionary<string, Dictionary<string, IBotCalls>> commands;
        Dictionary<string, Character> characters;


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

            SetUp(ref botConfig, ref characters, ref commands);

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

                string[] substring = msg.Split(" ", 2);
                //foreach (string s in substring)
                //{
                //    Console.WriteLine(s);
                //}
                switch (substring[0])
                {
                    case "char":
                        if (characters.TryGetValue(substring[1], out Character c))
                        {
                            await MyEmbedBuilderAsync(c, message);
                        }
                        else if (substring[1] == "random")
                        {
                            Random rand = new Random();
                            Character randChar = characters.ElementAt(rand.Next(0, characters.Count)).Value;
                            await MyEmbedBuilderAsync(randChar, message);
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync($"Character '{substring[1]}' not found. Try '^list char' to see available characters.");
                        }

                        break;
                    case "list":
                        if (substring[1] == "char")
                        {
                            string output = "```\n";
                            foreach (KeyValuePair<string, Character> kvp in characters)
                            {
                                //Character ch = kvp.Key;
                                output += $"{kvp.Key.ToString()}\n";
                            }
                            output += "```";
                            await message.Channel.SendMessageAsync(output);
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync("Unknown list command");
                        }
                        break;
                    case "help":
                        if (substring.Length > 1)
                        {
                            await message.Channel.SendMessageAsync("Help Command Not Implemented");
                        }
                        else
                        {
                            string output = "```\n";
                            output += "Command '^char charName' will show the trading card for that character\n";
                            output += "Command '^list char' will list all of the currently available characters\n";
                            output += "```";
                            await message.Channel.SendMessageAsync(output);
                        }
                        break;
                    default:
                        // Send list of commands.
                        await message.Channel.SendMessageAsync("Unknown command");
                        break;
                }
            }
        }

        public static void SetUp(ref BotConfig bc, ref Dictionary<string, Character> chars, ref Dictionary<string, Dictionary<string, IBotCalls>> commands)
        {
            JsonTextReader reader;
            try
            {
                // This is good for development where I've got the config with the project
                reader = new JsonTextReader(new StreamReader("..\\..\\..\\BotConfig.json"));
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
                reader = new JsonTextReader(new StreamReader("..\\..\\..\\characters.json"));
                chars = JsonConvert.DeserializeObject<Dictionary<string, Character>>(File.ReadAllText("..\\..\\..\\characters.json"));
                
                Dictionary<string, IBotCalls> temp = new Dictionary<string, IBotCalls>();

				// loop through IBotCalls
                foreach (KeyValuePair<string, Character> kvp in chars)
                {
                    temp.Add(kvp.Key, kvp.Value);
                }
				commands.Add("char", temp);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Project Level Character[] Initilization Exception:\n\t{e.Message}");
            }
        }

        public async Task MyEmbedBuilderAsync(Character c, SocketMessage message)
        {
            var builder = new EmbedBuilder()
            {
                Color = Color.Blue,
                Title = "Tales of Nowhere",
                Description = $"Famous Saying: '{c.CharacterQuote}'",
                ImageUrl = $"{c.ImageUrl}",
                Timestamp = DateTimeOffset.Now,
            }
                            .WithFooter(footer => footer.Text = $"{c.CharacterDescription}")
                            .AddField("Name: ", $"{c.CharacterName}");
            
            await message.Channel.SendMessageAsync(c.Lore, false, builder.Build());
        }
        
    }
}
