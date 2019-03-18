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
			commands.RegisterCommands <VozhbanCommands> ();
			MyCommands.FillList ();
			VozhbanCommands.LoadText ();
			Console.WriteLine ("Bot staterted");



			//Restore deleting message
			discord.MessageDeleted += async e => {
				LastDeletedMessage = e.Message.Content;
				await Task.Delay(0);
			};

			//Working when readction added
			discord.MessageReactionAdded += TumbUp;
			discord.MessageReactionAdded += Clock;
			discord.MessageReactionAdded += Permission;
			discord.MessageCreated		 += StatCheck;
			
			async Task TumbUp (MessageReactionAddEventArgs context) {

				if (context.Emoji.Name == "👍") {
					await context.Message.RespondAsync (context.User.Username + " like it!");
				}
			}

			async Task Clock (MessageReactionAddEventArgs context) {

				if (context.Emoji.Name == "⏲") {
					string Respond = context.User.Username + " готов поиграть в ДС!";
					if (PartyCount > 0) {
						Respond += "\n Кстати в войсе уже " + PartyCount + " людей!";
					}
					await context.Message.RespondAsync (Respond);
				}
			}
			await discord.ConnectAsync();

			async Task Permission (MessageReactionAddEventArgs context) {

				if (context.Emoji.Name == "🔫") {
					await context.Message.RespondAsync (context.Message.Author.Username + " расстрелян");
				}
			}

			async Task TolpojDS (MessageCreateEventArgs context) {
				
				string Message = context.Message.Content.ToLower();
				
				if (Message.Contains("@ДС") {
					context.CreateReactionAsync(DSharpPlus.Entities.DiscordEmoji.FromName(discord, ":g6:"));
					context.CreateReactionAsync(DSharpPlus.Entities.DiscordEmoji.FromName(discord, ":r1:"));
				}
			}
			
			async Task StatCheck (MessageCreateEventArgs context) {

				string Message = context.Message.Content.ToLower();
				
				if (Message[0] == 'd') {

					string Respond = "";
					//Бросаем 2 d6 
					Random FirstD6  = new Random ();
					Random SecondD6 = new Random ();
					int First  = FirstD6 .Next (1,7);
					int Second = SecondD6.Next (1,7);
					
					string GreenDie;
					string RedDie;
					
					GreenDie = DSharpPlus.Entities.DiscordEmoji.FromName(discord, ":g" + First + ":").ToString();
					RedDie   = DSharpPlus.Entities.DiscordEmoji.FromName(discord, ":r" + Second + ":").ToString();

					if (Message.Length == 1) {
						Respond = context.Message.Author.Mention + " выкидывает " + GreenDie + RedDie + " | " + First + "-" + Second + "= **" + (First - Second) + "**";
					}
					else {
						int Side = 0;

						if (Message.Length > 2 && Message[1] == '-') {
							if (int.TryParse (Message.Remove (0,2), out Side) == true) {
								Random random  = new Random ();
								int Result = random.Next (1, Side);
								Respond = context.Message.Author.Mention + " выкидывает " + GreenDie + RedDie + " | " + First + "-" + Second + "-" + Side + "= **" + (First - Second - Side) + "**";
							}
						}
						else {

							if (int.TryParse (Message.Remove (0,1), out Side) == true) {
								Random random  = new Random ();
								int Result = 0;
								if (Side > 1) {
									Result = random.Next (1, Side);
								}
								Respond = context.Message.Author.Mention + " выкидывает " + GreenDie + RedDie + " | " + First + "-" + Second + "+" + Side + "= **" + (First - Second + Side) + "**";
							}
						}
					}
					await context.Message.RespondAsync (Respond);
					await context.Message.DeleteAsync ();
				}
			}

			//Позволяет печатать прямо из консоли в любой канал, при отправке в azure лучше закоментить этот код
			//while(true) {
			//	string Message = Console.ReadLine ();
			//	DSharpPlus.Entities.DiscordChannel channel = await discord.GetChannelAsync (292562693993529349);//NSFW Science 439527469897351178 //Bot log 530096997726945317
			//	await discord.SendMessageAsync (channel, Message);
			//}

			await Task.Delay(-1);
		}
	}
}
