using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;

namespace DiscordBot {

	public class Program {
		
		public static DiscordClient discord;

		static CommandsNextModule commands;

		public static int PartyCount;
		public static string LastDeletedMessage;

		
		static void Main (string[] args) {
			MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		static async Task MainAsync (string[] args) {
			
			//Настройка базовой конфигурации бота
			DiscordConfiguration DiscordConfig = new DiscordConfiguration {
				Token = "NTMxMjE2MDAyOTU2OTE4ODA0.Dy2k6w.Ly_zkEgU4Udq1jAJc_Mf0SugjUU",
				TokenType = TokenType.Bot,
				UseInternalLogHandler = true, 
				LogLevel = LogLevel.Debug
				
			};
			
			discord = new DiscordClient (DiscordConfig);

			//Настройка списка комманд
			CommandsNextConfiguration commandsConfig = new CommandsNextConfiguration {
				StringPrefix = "!!",
				EnableMentionPrefix = true
			};

			commands = discord.UseCommandsNext (commandsConfig);
			commands.RegisterCommands <MyCommands> ();
			Console.WriteLine ("Bot staterted");

			
			//Detecting new message
			discord.MessageCreated += async e => {
				await Task.Delay(0);
			};


			//Restore deleting message
			discord.MessageDeleted += async e => {
				LastDeletedMessage = e.Message.Content;
				await Task.Delay(0);
			};

			//Working when readction added
			discord.MessageReactionAdded += TumbUp;
			discord.MessageReactionAdded += Clock;
			discord.MessageReactionAdded += Permission;
			
			async Task TumbUp (MessageReactionAddEventArgs e) {

				if (e.Emoji.Name == "👍") {
					await e.Message.RespondAsync (e.User.Username + " like it!");
				}
			}

			async Task Clock (MessageReactionAddEventArgs e) {

				if (e.Emoji.Name == "⏲") {
					string Respond = e.User.Username + " готов поиграть в ДС!";
					if (PartyCount > 0) {
						Respond += "\n Кстати в войсе уже " + PartyCount + " людей!";
					}
					await e.Message.RespondAsync (Respond);
				}
			}
			await discord.ConnectAsync();

			async Task Permission (MessageReactionAddEventArgs e) {

				if (e.Emoji.Name == "🔫") {
					await e.Message.RespondAsync (e.Message.Author.Username + " расстрелян");
				}
			}

			while(true) {
				string Message = Console.ReadLine ();
				DSharpPlus.Entities.DiscordChannel channel = await discord.GetChannelAsync (292562693993529349);//NSFW Science 439527469897351178 //Bot log 530096997726945317
				await discord.SendMessageAsync (channel, Message);
			}

			await Task.Delay(-1);
		}
	}
}
