using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;


namespace DiscordBot {
	public class VozbanCommands {

		[Command("test")]
		public async Task Test (CommandContext context) {
			await context.RespondAsync ("test");
		}
	}
}