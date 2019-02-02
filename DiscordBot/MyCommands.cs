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
			"```Commands: \n Help \n Ping \n Invite \n d \n Choose \n ShowChannels \n ```";
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
		
		[Command ("ShowChannels")]
		public async Task ShowChannels (CommandContext context) {
			string Respond = "Server name: " + context.Guild.Name + "\n";
			Respond += context.Guild.Owner.Mention + "is owner \n";

			IReadOnlyList<DSharpPlus.Entities.DiscordChannel> AllChannels = context.Guild.Channels;
			for (int i = 0; i < AllChannels.Count; i++) {
				Respond += AllChannels[i].Name +" is " + AllChannels[i].Topic + "\n";
			}
			await context.RespondAsync (Respond);
		}

		[Command ("ShowServers")]
		public async Task ShowServers (CommandContext context) {
			string Respond = "";



			Console.WriteLine ("Wow");
			IReadOnlyDictionary <ulong, DSharpPlus.Entities.DiscordGuild> allGuilds = Program.discord.Guilds;
			Console.WriteLine (allGuilds.Count);

			foreach (DSharpPlus.Entities.DiscordGuild each in allGuilds.Values) {
				Respond += each.Name + " owner " + each.Owner.Nickname + " with " + each.MemberCount + " people \n";
			}
			await context.RespondAsync (Respond);
		}

		[Command ("Reverse")]
		public async Task Reverse (CommandContext context) {
			await context.RespondAsync (Program.LastDeletedMessage);
		}
	}
}
