using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using ToNDiscBot.classes;


//When adding a new dictionary and series of commands, follow these steps:
//    1. Create the json to hold the data
//    2. Create the class that is going to hold the live data
//    3. Implement the interface on the class, and set class as public
//    4. Create the dictionary that will hold the objects
//    5. Add the ref of the dictionary to the SetUp() function
//    6. Populate the dictionary in SetUp
//    7. Add the command to the commands dictionary before leaving setup

namespace ToNDiscBot
{
    public class Program
    {
        DiscordSocketClient client;
        BotConfig botConfig = new BotConfig();

        Dictionary<string, Dictionary<string, IBotCalls>> commands;
        Dictionary<string, Character> characters;
        Dictionary<string, BotHelp> help;
        Dictionary<string, Episode> episodes;

        static string prefix = $"~";


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

            SetUp(ref botConfig, ref characters, ref commands, ref help, ref episodes);

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
            if (message.Content.StartsWith(prefix))
            {
                string msg = message.Content.Substring(1).ToLower();

                string[] substring = msg.Split(" ", 2);

                //Makes it here
                if (substring.Length != 1)
                {
                    if (commands.TryGetValue(substring[0], out Dictionary<string, IBotCalls> dict))
                    {
                        // We now have the command dictionary (chars or whatever else)
                        if (dict.TryGetValue(substring[1], out IBotCalls item))
                        {
                            await item.SendChannelMessageAsync(message);
                        }
                        else
                        {
                            // Couldn't find the command.  Could it be random?
                            // If it's allowed, grab a random one.
                            if (dict.Values.FirstOrDefault().AllowRandom && substring[1].ToLower().Equals("random"))
                            {
                                IBotCalls randomItem = dict.ElementAt(new Random().Next(dict.Count - 1)).Value;
                                await randomItem.SendChannelMessageAsync(message);
                            }
                            else if (substring[1].ToLower().Equals("list"))
                            {
                                string listOutput = "```\n";
                                int tabs = 0;
                                foreach (KeyValuePair<string, IBotCalls> key in dict)
                                {
                                    listOutput += $"{key.Key.ToString()}\t";
                                    tabs++;
                                    if(tabs == 5)
                                    {
                                        listOutput += $"\n";
                                        tabs = 0;
                                    }
                                }
                                listOutput += "```";
                                await message.Channel.SendMessageAsync(listOutput);
                            }
                            else
                            {
                                await message.Channel.SendMessageAsync($"Command {substring[1]} not found");
                            }
                        }

                    }
                    else
                    {
                        // Command not found
                        // Send message that the command is invalid.  Possibly print command list?
                    }

                }
                else
                {
                    await PrintDefaultHelpAsync(message);
                }
            }
        }

        public static void SetUp(ref BotConfig bc, ref Dictionary<string, Character> chars, ref Dictionary<string, Dictionary<string, IBotCalls>> commands, ref Dictionary<string, BotHelp> help, ref Dictionary<string, Episode> episodes)
        {
            JsonTextReader reader;
            try
            {
                // This is good for deployment where I've got the config with the executable
                reader = new JsonTextReader(new StreamReader("json\\BotConfig.json"));
                bc = JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText("json\\BotConfig.json"));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Executable Level SetUp Exception:\n\t{e.Message}");
            }

            try
            {
                try
                {
                    reader = new JsonTextReader(new StreamReader("json\\characters.json"));
                    chars = JsonConvert.DeserializeObject<Dictionary<string, Character>>(File.ReadAllText("json\\characters.json"));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Characters reading in error:\n\t{e.Message}");
                }

                try
                {
                    reader = new JsonTextReader(new StreamReader("json\\episodes.json"));
                    episodes = JsonConvert.DeserializeObject<Dictionary<string, Episode>>(File.ReadAllText("json\\episodes.json"));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Episodes reading in error:\n\t{e.Message}");
                }

                Dictionary<string, IBotCalls> charTemp = new Dictionary<string, IBotCalls>();

                // loop through IBotCalls
                foreach (KeyValuePair<string, Character> kvp in chars)
                {
                    charTemp.Add(kvp.Key, kvp.Value);
                }
                try
                {
                    commands = new Dictionary<string, Dictionary<string, IBotCalls>>
                    {
                        { "char", charTemp }
                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine($"commands.Add character Level Error:\n\t{e}");
                }
                try
                {
                    reader = new JsonTextReader(new StreamReader("json\\botHelp.json"));
                    help = JsonConvert.DeserializeObject<Dictionary<string, BotHelp>>(File.ReadAllText("json\\botHelp.json"));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"botHelp reading in error:\n\t{e.Message}");
                }

                Dictionary<string, IBotCalls> botHelptemp = new Dictionary<string, IBotCalls>();

                // loop through IBotCalls
                foreach (KeyValuePair<string, BotHelp> kvp in help)
                {
                    botHelptemp.Add(kvp.Key, kvp.Value);
                }
                try
                {
                    commands.TryAdd("help", botHelptemp);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"commands.Add botHelp Level Error:\n\t{e}");
                }

                // loop through IBotCalls for episodes
                Dictionary<string, IBotCalls> episodetemp = new Dictionary<string, IBotCalls>();
                foreach (KeyValuePair<string, Episode> kvp in episodes)
                {
                    episodetemp.Add(kvp.Key, kvp.Value);
                }
                try
                {
                    commands.TryAdd("ep", episodetemp);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"commands.Add episode Level Error:\n\t{e}");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Project Level Character[] Initilization Exception:\n\t{e.Message}");
            }
        }

        public static async Task PrintDefaultHelpAsync(SocketMessage msg)
        {
            var builder = new EmbedBuilder()
            {
                Color = Color.Gold,
                Title = "ToN Help",
                Description = $"Use {prefix}help <command> for a description of what parameters that command can take",
                Timestamp = DateTimeOffset.Now,
            }
                            .AddField($"Example:\n", $"``{prefix}help list`` to show all help commands\n``{prefix}help char`` to see the char command help");

            await msg.Channel.SendMessageAsync(string.Empty, false, builder.Build());
        }


    }
}
