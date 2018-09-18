using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ToNDiscBot.classes
{
    public class Character : ModuleBase
    {
        public string CharacterName { get; set; }
        public string ImageUrl { get; set; }
        public string CharacterDescription { get; set; }
        public string CharacterQuote { get; set; }
		public string Lore { get; set; }
    }
}
