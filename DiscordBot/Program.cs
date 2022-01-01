using System;
using System.IO;
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
		//Загружается из файла Token.txt
		public static string DiscordToken;

		
		static void Main (string[] args) {

			DiscordToken = ReadTxt ("Token.txt");
			Console.WriteLine ("You Token: " + DiscordToken);
			MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		static async Task MainAsync (string[] args) {
			
			//Настройка базовой конфигурации бота
			DiscordConfiguration DiscordConfig = new DiscordConfiguration {
				Token = DiscordToken,
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
			Console.WriteLine ("Bot staterted 2.0");



			//Restore deleting message
			discord.MessageDeleted += async e => {
				LastDeletedMessage = e.Message.Content;
				await Task.Delay(0);
			};

			//Working when readction added
			//discord.MessageReactionAdded += TumbUp;
			discord.MessageReactionAdded += Clock;
			discord.MessageReactionAdded += Permission;
			discord.MessageCreated		 += RollDice;
			discord.MessageCreated		 += RollXDice;
			discord.MessageCreated		 += TolpojDS;
			
			//async Task TumbUp (MessageReactionAddEventArgs context) {
			//
			//	if (context.Emoji.Name == "👍") {
			//		await context.Message.RespondAsync (context.User.Username + " like it!");
			//	}
			//}

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
				
				string Message = context.Message.Content.ToUpper();
				
				if (Message.Contains("<@&515543976678391808>")) {
					await context.Message.CreateReactionAsync(DSharpPlus.Entities.DiscordEmoji.FromName(discord, ":g6:"));
					await context.Message.CreateReactionAsync(DSharpPlus.Entities.DiscordEmoji.FromName(discord, ":r1:"));
				}
			}
			
			
			async Task RollXDice (MessageCreateEventArgs context) {

				string Message = context.Message.Content.ToLower();
				
				int d = Message.IndexOf("r");
				
				if (d != -1) {
				
					string Respond = "";
					int RollCount = 0;
					int s = Message.IndexOf("+");
					int Mod = 0;
					int Sides = 0;
					string Sign = "";
					string Side = "";
					Random Die = new Random ();
					
					if (s == -1) {
	 					s = Message.IndexOf ("-");
	 				}
	 				
	 				if (s != -1) {
	 					if (int.TryParse (Message.Remove (0,s), out Mod) == true) {
	 						if (Mod > 0) {
	 							Sign = "+";
	 						}
	 					}
	 					Side = Message.Remove (s);
	 				}
	 				else {
	 					Mod = 0;
	 					Side = Message;
	 				}
				
					if (d == 0) {
						RollCount = 1;
					}
					else {
						if (int.TryParse (Message.Remove (d), out RollCount) == true) {
							if (RollCount <= 0)  {
								RollCount = 0;
								Respond = context.Message.Author.Mention + " роняет кубы, выкидывает **-11** и страдает. За тупость.\n";
							}
						}
					}
					
	 				if (int.TryParse (Side.Remove (0,d+1), out Sides) == true) {
	 					int Dice = Die.Next (1,Sides+1);
						if (Sides <= 0) {
							RollCount = 0;
							Respond = context.Message.Author.Mention + " роняет кубы, выкидывает **-11** и страдает. За тупость.\n";
						}
	 				}
					else {
						RollCount = 0;
					}
					
					if ((Message.Length == d+1) || (Message.Length == s+1) || (s == d+1))  {
						RollCount = 0;
					}
					
					for (int i = 0; i < RollCount; i++) {
						if (Mod == 0) {
							int Dice = Die.Next (1,Sides+1);
							Respond += context.Message.Author.Mention + " кидает " + Sides + "-гранник и выкидывает **" + Dice + "**\n";
						}
						else {
							int Dice = Die.Next (1,Sides+1);
							Respond += context.Message.Author.Mention + " кидает " + Sides + "-гранник и выкидывает " + Dice + Sign + Mod + " = **" + (Dice+Mod) + "**\n";
						}
					}
					await context.Message.RespondAsync (Respond);
					await context.Message.DeleteAsync ();
				}
			}
			
			async Task RollDice (MessageCreateEventArgs context) {

				string Message = context.Message.Content.ToLower();
				
				int d = Message.IndexOf("d");
				
				if (d != -1) {
				
					string Respond = "";
					int RollCount = 0;
					
					if (d == 0) {
						RollCount = 1;
					}
					else {
						if (int.TryParse (Message.Remove (d), out RollCount) == true) {
							if (RollCount <= 0) {
								RollCount = 0;
								Respond = context.Message.Author.Mention + " роняет кубы, выкидывает **-11** и страдает. За тупость.\n";
							}
						}
					}
					Console.Write ("Message" + context.Message.Content + " RollCount: " + RollCount);
					for (int i = 0; i < RollCount; i++) {
						Random GreenDie = new Random ();
						Random RedDie = new Random ();
							
						int Green  = GreenDie.Next (1,7);
						int Red = RedDie.Next (1,7);
							
						string EmoGreenDie;
						string EmoRedDie;
							
						EmoGreenDie = ":green_square:"; //DSharpPlus.Entities.DiscordEmoji.FromName(discord, ":g" + Green + ":").ToString();
						EmoRedDie   = ":red_square:";//DSharpPlus.Entities.DiscordEmoji.FromName(discord, ":r" + Red + ":").ToString();
							
						if (Message.Length == d+1) {
							Respond += context.Message.Author.Mention + " выкидывает " + EmoGreenDie + EmoRedDie + " | " + Green + " - " + Red + " = **" + (Green - Red);
						}
						else {
							int Mod = 0;
							string Sign = "";
							if (int.TryParse (Message.Remove (0,d+1), out Mod) == true) {
								if (Mod >= 0) {
									Sign = "+";
								}
								else {
									Sign = "";
								}
								Respond += context.Message.Author.Mention + " выкидывает " + EmoGreenDie + EmoRedDie + " | " + Green + " - " + Red + " " + Sign + Mod + " = **" + (Green - Red + Mod);
							}
							else {
								// Защита от удаления сообщения, если это не запрос боту, а слово на d
								return;
							}
						}
						if ((Green - Red) == 5) {
							Respond += ". Критический Успех";
						}
						else if ((Green - Red) == -5) {
							Respond += ". Критический Провал";
						}
						Respond += "**\n";
					}
					await context.Message.RespondAsync (Respond);
					await context.Message.DeleteAsync ();
				}
			}
			
			/*
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
			*/

			//Позволяет печатать прямо из консоли в любой канал, при отправке в azure лучше закоментить этот код
			//while(true) {
			//	string Message = Console.ReadLine ();
			//	DSharpPlus.Entities.DiscordChannel channel = await discord.GetChannelAsync (292562693993529349);//NSFW Science 439527469897351178 //Bot log 530096997726945317
			//	await discord.SendMessageAsync (channel, Message);
			//}

			await Task.Delay(-1);
		}
	
		

		/// <summary>
		/// Выдает содержимое каждой строки txt файла который лежит в корне Unity проекта (рядом с Assets)
		/// Имя файла например Test (без расширения)
		/// </summary>
		public static string[] ReadTxtLines (string FileName) {

			string Path = @"./" + FileName + ".txt";


			if (File.Exists (Path) == false) {
				return new string[0];
			}
			else {
				return File.ReadAllLines (Path);
			}
		}


		/// Выдает содержимое txt файла который лежит в корне Unity проекта (рядом с Assets)
		/// Имя файла например Test.txt
		public static string ReadTxt (string FileName) {

			string Path = @"./" + FileName;


			if (File.Exists (Path) == false) {
				return string.Empty;
			}
			else {
				return File.ReadAllText (Path);
			}
		}

	}
}
