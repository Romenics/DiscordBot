using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.SlashCommands;

namespace DiscordBot {

	public class Program {
		
		public static DiscordClient discord;

		static CommandsNextExtension commands;

		public static int PartyCount;
		//Загружается из файла Token.txt
		public static string DiscordToken;

		
		static void Main (string[] args) {

			DiscordToken = ReadTxt ("Token.txt");
			Console.WriteLine ("You Token: " + DiscordToken);
			MainAsync(args).ConfigureAwait (false).GetAwaiter ().GetResult();
		}

		static async Task MainAsync (string[] args) {
			
			//Настройка базовой конфигурации бота
			DiscordConfiguration DiscordConfig = new DiscordConfiguration {
				Token = DiscordToken,
				TokenType = TokenType.Bot,
				MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
				Intents = DiscordIntents.AllUnprivileged
			};
			
			discord = new DiscordClient (DiscordConfig);

			string[] Prefixes = new string[1];
			Prefixes[0] = "!!";

			//Настройка списка комманд
			CommandsNextConfiguration commandsConfig = new CommandsNextConfiguration {
			
				StringPrefixes = Prefixes,
				EnableMentionPrefix = true
			};

			InteractivityConfiguration interactivityConfig = new InteractivityConfiguration {
				PollBehaviour = DSharpPlus.Interactivity.Enums.PollBehaviour.KeepEmojis,
				Timeout = TimeSpan.FromSeconds(30)
			};

			SlashCommandsExtension slashCommands = discord.UseSlashCommands();
			//slashCommands.RegisterCommands<MyCommands>();


			commands = discord.UseCommandsNext (commandsConfig);
			commands.RegisterCommands<MyCommands> ();
			commands.RegisterCommands <VozhbanCommands> ();
			MyCommands.FillList ();
			VozhbanCommands.LoadText ();
			Console.WriteLine ("Bot staterted 3.0");


			//Working when readction added
			discord.MessageReactionAdded += Clock;
			discord.MessageReactionAdded += Permission;
			discord.MessageCreated		 += RollDice;
			discord.MessageCreated		 += RollXDice;
			discord.MessageCreated		 += TolpojDS;
			discord.MessageCreated		 += RollAttackDice;


			async Task Clock (DiscordClient discordClient, MessageReactionAddEventArgs context) {

				if (context.Emoji.Name == "⏲") {
					string Respond = context.User.Username + " готов поиграть в ДС!";
					if (PartyCount > 0) {
						Respond += "\n Кстати в войсе уже " + PartyCount + " людей!";
					}
					await context.Message.RespondAsync (Respond);
				}
			}
			await discord.ConnectAsync();

			async Task Permission (DiscordClient discordClient, MessageReactionAddEventArgs context) {

				if (context.Emoji.Name == "🔫") {
					await context.Message.RespondAsync (context.Message.Author.Username + " расстрелян");
				}
			}

			async Task TolpojDS (DiscordClient discordClient, MessageCreateEventArgs context) {
				
				string message = context.Message.Content;

				if (message.Contains("<@&515543976678391808>")) { //("<@&515543976678391808>")) {
					await context.Message.CreateReactionAsync(DiscordEmoji.FromName (discordClient, ":r1:", true));
					await context.Message.CreateReactionAsync(DiscordEmoji.FromName (discordClient, ":g6:", true));
				}
			}
			
			
			async Task RollXDice (DiscordClient discordClient, MessageCreateEventArgs context) {

				string Message = context.Message.Content.ToLower();
				
				int d = Message.IndexOf("r");
				
				if (d != -1) {
				
					string Respond = "";
					int RollCount = 0;
					int s = Message.IndexOf("+");
					int Mod = 0;
					int Sides = 0;
					int Sum = 0;
					string Sign = "";
					string Side = "";
					Random Die = new Random ();
					int countC = 0;
					int validC = 1;
					
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

					foreach (char c in Message) {
						if (char.ToLower(c) == 'r') {
							countC++;
						} else if (!char.IsDigit(c) && c != '+' && c != '-') {
							validC = 0;
							break;
						}
					}
					
					if (countC == 1 && validC == 1) {
						
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
							if (Sides <= 0) {
								RollCount = 0;
								Respond = context.Message.Author.Mention + " роняет кубы, выкидывает **-11** и страдает. За тупость.\n";
							}
		 				}
					}
					
					if (RollCount == 1) {
						int Dice = Die.Next (1,Sides+1);
						if(Mod == 0) {
							Respond += context.Message.Author.Mention + " кидает " + Sides + "-гранник и выкидывает **" + Dice + "**\n";
						}
						else {
							Respond += context.Message.Author.Mention + " кидает " + Sides + "-гранник и выкидывает " + Dice + Sign + Mod + " = **" + (Dice+Mod) + "**\n";
						}
					}
					else if (RollCount > 1) {
						if(Mod == 0) {
							Respond += context.Message.Author.Mention + " кидает " + Sides + "-гранник " + RollCount + " раз и выкидывает **";
						}
						else {
						Respond += context.Message.Author.Mention + " кидает " + Sides + "-гранник [" + Sign + Mod + "] " + RollCount + " раз и выкидывает **";
						}
						for (int i = 0; i < RollCount; i++) {
							int Dice = Die.Next (1,Sides+1);
							Sum += Dice + Mod;
							Respond += (Dice+Mod);
							if (i == RollCount - 1) {
								Respond += "**.";
							}
							else {
								Respond += ", ";
							}
						}
						Respond += "\nСумма: **" + Sum + "**";
					}
					// Не надо пытаться посылать пустой ответ
					if (Respond !="") {
						await discord.SendMessageAsync (context.Channel, Respond);
						await context.Message.DeleteAsync ();
					}
				}
			}
			
			async Task RollDice (DiscordClient discordClient, MessageCreateEventArgs context) {

				string Message = context.Message.Content.ToLower();
				
				int d = Message.IndexOf("d");

				if (d != -1) {
					string Respond = "";
					int RollCount = 0;
					int Mod = 0;
					int Sum = 0;
					string Sign = "";
					Random GreenDie = new Random ();
					Random RedDie = new Random ();
					int countC = 0;
					int validC = 1;

					foreach (char c in Message) {
						if (char.ToLower(c) == 'd') {
							countC++;
						} else if (!char.IsDigit(c) && c != '+' && c != '-') {
							validC = 0;
							break;
						}
					}
					
					if (countC == 1 && validC == 1) {
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
					}


					if (int.TryParse (Message.Remove (0,d+1), out Mod) == true) {
						if (Mod >= 0) {
							Sign = "+";
						}
					}

					
					
					if (RollCount == 1) {
						
						int Green  = GreenDie.Next (1,7);
						int Red = RedDie.Next (1,7);
						string SignD = "";
						string EmoGreenDie;
						string EmoRedDie;
						EmoGreenDie = DiscordEmoji.FromName(discordClient, ":g" + Green + ":", true).ToString();
						EmoRedDie   = DiscordEmoji.FromName(discordClient, ":r" + Red + ":", true).ToString();
						
						if(Green - Red + Mod> 0) {
							SignD = "+";
						}
						
						/*if (Message.Length == d+1) {
							Respond += context.Message.Author.Mention + " выкидывает " + EmoGreenDie + EmoRedDie + " | " + Green + " - " + Red + " = **" + SignD + (Green - Red);
							return;
						}*/
						
						if(Mod == 0) {
							Respond += context.Message.Author.Mention + " выкидывает " + EmoGreenDie + EmoRedDie + " | " + Green + " - " + Red + " = **" + SignD + (Green - Red);
						}
						else {
							Respond += context.Message.Author.Mention + " выкидывает " + EmoGreenDie + EmoRedDie + " | " + Green + " - " + Red + " " + Sign + Mod + " = **" + SignD + (Green - Red + Mod);
						}
						if ((Green - Red) == 5) {
							Respond += ". Критический Успех";
						}
						else if ((Green - Red) == -5) {
							Respond += ". Критический Провал";
						}
						Respond += "**";
					}
					else if (RollCount > 1) {
						if(Mod == 0) {
							Respond += context.Message.Author.Mention + " кидает кубы " + RollCount + " раз и выкидывает **";
						}
						else {
						Respond += context.Message.Author.Mention + " кидает кубы [" + Sign + Mod + "] " + RollCount + " раз и выкидывает **";
						}
						for (int i = 0; i < RollCount; i++) {
							int Green  = GreenDie.Next (1,7);
							int Red = RedDie.Next (1,7);
							string SignD = "";
							if(Green - Red + Mod > 0) {
								SignD = "+";
							}
							Sum += Green - Red + Mod;
							if ((Green - Red) == 5) {
								Respond += "{__" + SignD + (Green-Red+Mod) + "__}";
							}
							else if ((Green - Red) == -5) {
								Respond += "(__" + SignD + (Green-Red+Mod) + "__)";
							}
							else {
								Respond += SignD + (Green-Red+Mod);
							}
							if (i == RollCount - 1) {
								Respond += "**.";
							}
							else {
								Respond += ", ";
							}
						}
						Respond += "\nСумма: **" + Sum + "**";
					}
					Console.WriteLine("Respond: " + Respond);
					if (Respond !="") {
						await discord.SendMessageAsync (context.Channel, Respond);
						await context.Message.DeleteAsync ();
					}
				}
			}

			async Task RollAttackDice (DiscordClient discordClient, MessageCreateEventArgs context) {

				string Message = context.Message.Content.ToLower();
				
				int d = Message.IndexOf("a");
				/*формат значит классический хАу, где в данном случае:
					х - урон к которому мы кидаем кубы
					у - наша точность, к которой мы тоже, внезапно, кидаем кубы
				Так как вся ебля лежит на мастере, нужно просто кинуть кубы 2 раза, один раз на точность, второй на урон
				*/
				if (d != -1) {
				
					string Respond = "";
					int Damage = 0;
					int DamageBase = 0;
					int Mod = 0;
					string Sign = "";
					Random GreenDie = new Random ();
					Random RedDie = new Random ();
					int RollCount = 0;
					int countC = 0;
					int validC = 1;
					
					//Достаём урон, там где раньше доставали количество бросков. Урон может быть отрицательным в процессе расчётов.
					int.TryParse (Message.Remove (d), out Damage);
					Console.Write ("Message" + context.Message.Content + " Damage: " + Damage);
					
					//Достаём проверку, так как это просто модификатор, оставляетм как есть
					if (int.TryParse (Message.Remove (0,d+1), out Mod) == true) {
						if (Mod >= 0) {
							Sign = "+";
						}
					}


					foreach (char c in Message) {
						if (char.ToLower (c) == 'a') {
							countC++;
						} else if (!char.IsDigit(c) && c != '+' && c != '-') {
							validC = 0;
							break;
						}
					}
					
					if (countC == 1 && validC == 1) {
						RollCount = 1;
					}
					
					if (RollCount == 1) {
						int Green  = GreenDie.Next (1,7);
						int Red = RedDie.Next (1,7);
						string SignD = "";
						string EmoGreenDie;
						string EmoRedDie;
						EmoGreenDie = DiscordEmoji.FromName(discordClient, ":g" + Green + ":", true).ToString();
						EmoRedDie   = DiscordEmoji.FromName(discordClient, ":r" + Red + ":", true).ToString();
						
						if(Green - Red + Mod> 0) {
							SignD = "+";
						}
						
						//Кидаем на точность
						if(Mod == 0) {
							Respond += context.Message.Author.Mention + " выкидывает " + EmoGreenDie + EmoRedDie + " | " + Green + " - " + Red + " = **" + SignD + (Green - Red);
						}
						else {
							Respond += context.Message.Author.Mention + " выкидывает " + EmoGreenDie + EmoRedDie + " | " + Green + " - " + Red + " " + Sign + Mod + " = **" + SignD + (Green - Red + Mod);
						}
						if ((Green - Red) == 5) {
							Respond += ". Критический Успех";
						}
						else if ((Green - Red) == -5) {
							Respond += ". Критический Провал";
						}
						Respond += "**\n";
						
						//Кидаем на урон
						Green = GreenDie.Next (1,7);
						Red = RedDie.Next (1,7);
						EmoGreenDie = DiscordEmoji.FromName(discordClient, ":g" + Green + ":", true).ToString();
						EmoRedDie   = DiscordEmoji.FromName(discordClient, ":r" + Red + ":", true).ToString();
		
						DamageBase = Damage;
						Damage = Green - Red + Damage;
						
						if(Damage <= 0) {
							Respond += "И не наносит урона.";
						}
						else {
							Respond += "И наносит урон. ";
							Respond += " " + EmoGreenDie + EmoRedDie + " | " + DamageBase + " + " + Green + " - " + Red + " = **" + Damage + "**";
						}
						if ((Green - Red) == 5) {
							Respond += " **. Критический Успех**";
						}
						else if ((Green - Red) == -5) {
							Respond += " **. Критический Провал**";
						}

					}
					
					if (Respond !="") {
						await discord.SendMessageAsync (context.Channel, Respond);
						await context.Message.DeleteAsync ();
					}
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
