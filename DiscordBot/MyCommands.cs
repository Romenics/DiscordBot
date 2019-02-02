using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;


namespace DiscordBot {
	public class MyCommands {

		[Command("Ping")]
		public async Task Ping (CommandContext context) {
			await context.RespondAsync ("📡 Pong");
		}

		[Command ("Invite")]
		public async Task Invite (CommandContext context) {
			await context.RespondAsync ("https://discordapp.com/oauth2/authorize?client_id=531216002956918804&scope=bot&permissions=8");
		}

		[Command ("Help")]
		public async Task Help (CommandContext context) {
			string Respond = 
			"```Commands: \n Help \n Ping \n Invite \n d \n Choose```";
			await context.RespondAsync (Respond);
		}

		[Command("d")]
		public async Task RollDice (CommandContext context, int Side) {
			Random rand = new Random ();
			Console.WriteLine (Side);
			string Respond = "🎲 Rolled: " + rand.Next (1, Side);
			await context.RespondAsync (Respond);
		}

		[Command ("Choose")]
		public async Task Choose (CommandContext context) {
			string[] Suggestions = context.Message.Content.Split(' ');
			Random rand = new Random ();
			int Answer = rand.Next (1, Suggestions.Length);
			string Respond = "🤔 Hmm, I'm Choosing: " + Suggestions[Answer];
			await context.RespondAsync (Respond);
		}
		
	}
}
