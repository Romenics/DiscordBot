using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.IO;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;


namespace DiscordBot {
	public class MyCommands : BaseCommandModule {

		public static List <Character> Characters = new List<Character>();

		public static List <string> Sacraments = new List<string> ();

		public class Character {

			public string Name;

			public int[] Stats = new int[8];
			public int Endurance;
			public int Agility;
			public int Sense;
			public int Intelligence;
			public int Will;
			public int Charm;
			public int Luck;
		}


		[Command ("ping")]
		public async Task Ping (CommandContext context) {

			string message = context.Message.Content;

			IReadOnlyDictionary<ulong, DiscordChannel> AllChannels = context.Guild.Channels;
			foreach (KeyValuePair <ulong, DiscordChannel> each in AllChannels) {
				DiscordChannel channel = await Program.discord.GetChannelAsync(each.Value.Id);

				if (channel.Type == ChannelType.Text) {
					DiscordMessage mess = await  Program.discord.SendMessageAsync(channel, message);
					await mess.DeleteAsync ();
				}
			}

			await context.Message.DeleteAsync();

		}

		[Command ("invite")]
		public async Task Invite (CommandContext context) {
			await context.RespondAsync ("https://discordapp.com/oauth2/authorize?client_id=531216002956918804&scope=bot&permissions=8");
		}

		[Command ("choose")]
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

			IReadOnlyDictionary<ulong ,DSharpPlus.Entities.DiscordChannel> AllChannels = context.Guild.Channels;

			foreach (KeyValuePair<ulong, DiscordChannel> each in AllChannels) {
				Respond += each.Value.Name + " is " + each.Value.Topic + "\n";
			}
			await context.RespondAsync (Respond);
		}

		[Command ("ShowServers")]
		public async Task ShowServers (CommandContext context) {
			string Respond = "";

			IReadOnlyDictionary <ulong, DiscordGuild> allGuilds = Program.discord.Guilds;

			foreach (DiscordGuild each in allGuilds.Values) {
				Respond += each.Name + " owner " + each.Owner.Nickname + " with " + each.MemberCount + " people \n";
			}
			await context.RespondAsync (Respond);
		}

		[Command ("rolle")]
		public async Task RoleDice (CommandContext context, int Side) {
			Random random  = new Random ();
			int Result = random.Next (1, Side);
			string Respond = "🎲 " + context.Message.Author.Username + " rolled: *" + Result + "*";
			await context.RespondAsync (Respond);
		}

		[Command ("check")]
		public async Task CheckRandom (CommandContext context, int count) {

			

			int m5 = 0;
			int m4 = 0;
			int m3 = 0;
			int m2 = 0;
			int m1 = 0;
			int zero = 0;
			int p1 = 0;
			int p2 = 0;
			int p3 = 0;
			int p4 = 0;
			int p5 = 0;


			for (int i = 0; i < count; i++) {

				Random GreenDie = new Random ();
				Random RedDie = new Random ();
				
				int Green  = GreenDie.Next (1,7);
				int Red = RedDie.Next (1,7);

				if ((Green - Red) == -5) {
					m5++;
				}
				if ((Green - Red) == -4) {
					m4++;
				}
				if ((Green - Red) == -3) {
					m3++;
				}
				if ((Green - Red) == -2) {
					m2++;
				}
				if ((Green - Red) == -1) {
					m1++;
				}
				if ((Green - Red) == 0) {
					zero++;
				}
				if ((Green - Red) == 1) {
					p1++;
				}
				if ((Green - Red) == 2) {
					p2++;
				}
				if ((Green - Red) == 3) {
					p3++;
				}
				if ((Green - Red) == 4) {
					p4++;
				}
				if ((Green - Red) == 5) {
					p5++;
				}

			}
			await context.RespondAsync ("-5 выпало: " + m5 + "\n -4 выпало: " + m4 + "\n -3 выпало: " + m3 + "\n -2 выпало: " + m2 + "\n -1 выпало: " + m1 + "\n 0 выпало: " + zero + "\n +1 выпало: " + p1 + "\n +2 выпало: " + p2 + "\n +3 выпало: " + p3 + "\n +4 выпало: " + p4 + "\n +5 выпало: " + p5);
		}

		[Command ("обеты")]
		public async Task RoleSacrament (CommandContext context, int Count) {

			string Respond = "```";
			List <int> Choosen = new List<int>();

			for (int i = 0; i < Count; i++) {
				Random random  = new Random ();
				int Number = random.Next (0, Sacraments.Count);

				//Проверяем, чтобы не было одинаковых обетов
				if (Choosen.Contains (Number) == false) {
					Choosen.Add (Number);
					Respond += (Number + 1) + " " + Sacraments[Number] + "\n";
				}
				else {
					i--;
				}
			} 

			Respond += "```";
	
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}


		#region Статы

		[Command ("осил")]
		public async Task Strength (CommandContext context) {
			string Respond = "**Сила** \n";
			Respond += RoleStat (0);
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		[Command("овын")]
		public async Task Stamina (CommandContext context) {
			string Respond = "**Выносливость** \n";
			Respond += RoleStat (1);
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		[Command("олов")]
		public async Task Agility (CommandContext context) {
			string Respond = "**Ловкость** \n";
			Respond += RoleStat (2);
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		[Command("овос")]
		public async Task Perception (CommandContext context) {
			string Respond = "**Восприятие** \n";
			Respond += RoleStat (3);
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		[Command("оинт")]
		public async Task Intelligence (CommandContext context) {
			string Respond = "**Интеллект** \n";
			Respond += RoleStat (4);
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		[Command("овол")]
		public async Task Will (CommandContext context) {
			string Respond = "**Воля** \n";
			Respond += RoleStat (5);
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		[Command("ооба")]
		public async Task Charm (CommandContext context) {
			string Respond = "**Обаяние** \n";
			Respond += RoleStat (6);
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		[Command("оуда")]
		public async Task Luck (CommandContext context) {
			string Respond = "**Удача** \n";
			Respond += RoleStat (7);
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		#endregion


		public static string RoleStat (int State) {

			string Respond = "";

			for (int i = 0; i < Characters.Count; i ++) {
				Random FirstD6  = new Random ();
				Random SecondD6 = new Random ();

				int Result = FirstD6.Next (1,7) - SecondD6.Next (1,7) + Characters[i].Stats[State];
				Respond += Characters[i].Name + " stat: " + Characters[i].Stats[State] + " rolled: " +  Result + "\n"; 
			}

			return Respond;
		}

		
		[Command("хард")]
		public async Task HardNabor (CommandContext context) {
			string Respond = context.Message.Author.Mention + "** раскидывает хардкорный набор**\n";
			
			Random GreenDie = new Random ();
			Random RedDie = new Random ();
			
			int[] Stat = new int[8];
			int Sum = 0;

			for (int i = 0; i < 8; i ++) {
				Stat[i] = GreenDie.Next (1,7) - RedDie.Next (1,7);
				Sum += Stat[i];
			}

			for (int i = 0; i < 4; i++) {

				if (Stat[i] > 0) {
					Respond += "+";
				}
				else if (Stat[i] == 0) {
					Respond += " ";
				}

				Respond += Stat[i] + "\t";

				if (Stat[i + 4] > 0) {
					Respond += "+";
				}
				else if (Stat[i + 4] == 0) {
					Respond += " ";
				}
				Respond += Stat[i + 4] + "\n";
			}
			Respond += "**Сумма: " + Sum + "**";

			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}
		
		[Command("набор")]
		public async Task Nabor (CommandContext context) {
			string Respond = context.Message.Author.Mention + "** раскидывает набор**\n";
			
			Random GreenDie = new Random ();
			Random RedDie = new Random ();
			
			int[] Stat = new int[8];
			
			for (int i = 0; i < 8; i ++) {
				Stat[i] = GreenDie.Next (1,7) - RedDie.Next (1,7);
			}
			int j = 0;
			for (int i = 0; i < 7; i ++) {
				for (j = 7; j > i; j --) {
					if(Stat[j] > Stat[j - 1]) {
						Stat[j - 1] += Stat[j];
						Stat[j] = Stat[j - 1] - Stat[j];
						Stat[j - 1] -= Stat[j];
					}
				}
			}
			
			j = 0;
			for (int i = 0; i < 8; i ++) {
				if ((i > 0) && (Stat[i] != Stat[i-1])) Respond += "\n";
				if (Stat[i] > 0) Respond += "+";
				Respond += Stat[i] + " ";
				j += Stat[i];
			}
			
			Respond += "\n**Сумма: " + j + "**";
			
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}
		
		[Command("изи")]
		public async Task HeroNabor (CommandContext context) {
			string Respond = context.Message.Author.Mention + "** раскидывает три набора**\n";
			
			Random GreenDie = new Random ();
			Random RedDie = new Random ();
			
			int[][] Stat = new int[3][];

			for (int i = 0; i <3; i++) {
				Stat[i] = new int[8];
			}
			
			for (int k = 0; k < 3; k ++) {
				for (int i = 0; i < 8; i ++) Stat[k][i] = GreenDie.Next (1,7) - RedDie.Next (1,7);
			}
			
			for (int k = 0; k < 3; k ++) {
				for (int i = 0; i < 7; i ++) {
					for (int j = 7; j > i; j --) {
						if(Stat[k][j] > Stat[k][j - 1]) {
							Stat[k][j - 1] += Stat[k][j];
							Stat[k][j] = Stat[k][j - 1] - Stat[k][j];
							Stat[k][j - 1] -= Stat[k][j];
						}
					}
				}
			}
			
			for (int k = 0; k < 3; k ++) {
				int j = 0;
				for (int i = 0; i < 8; i ++) {
					if ((i > 0) && (Stat[k][i] != Stat[k][i-1])) Respond += "\n";
					if (Stat[k][i] > 0) Respond += "+";
					Respond += Stat[k][i] + " ";
					j += Stat[k][i];
				}
				Respond += "\n**Сумма: " + j + "**\n\n";
			}
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		[Command("хардзз")]
		public async Task HardNaborZZ (CommandContext context) {
			string Respond = context.Message.Author.Mention + "** раскидывает хардкорный набор**\n";
			
			Random GreenDie = new Random ();
			Random RedDie = new Random ();
			
			int[] Stat = new int[13];

			for (int i = 0; i < 13; i ++) {
				Stat[i] = GreenDie.Next (1,7) - RedDie.Next (1,7);
			}
			
			for (int i = 0; i < 4; i ++) {

				if (Stat[i] > 0) {
					Respond += "+";
				}
				else if (Stat[i] == 0) {
					Respond += " "; 
				}

				Respond += Stat[i] + "\t";

				if (Stat[i + 4] > 0) {
					Respond += "+";
				}
				else if (Stat[i + 4] == 0) {
					Respond += " "; 
				}
				
				Respond += Stat[i + 4] + "\t";
				
				if (Stat[i + 8] > 0) {
					Respond += "+";
				}
				else if (Stat[i + 8] == 0) {
					Respond += " "; 
				}
				
				Respond += Stat[i + 8] + "\n";
			}
			
			Respond += "\t\t" + Stat[12];

			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}
		
		[Command("наборзз")]
		public async Task NaborZZ (CommandContext context) {
			string Respond = context.Message.Author.Mention + "** раскидывает набор**\n";
			
			Random GreenDie = new Random ();
			Random RedDie = new Random ();
			
			int[] Stat = new int[13];
			
			for (int i = 0; i < 13; i ++) {
				Stat[i] = GreenDie.Next (1,7) - RedDie.Next (1,7);
			}
			int j = 0;
			for (int i = 0; i < 12; i ++) {
				for (j = 12; j > i; j --) {
					if(Stat[j] > Stat[j - 1]) {
						Stat[j - 1] += Stat[j];
						Stat[j] = Stat[j - 1] - Stat[j];
						Stat[j - 1] -= Stat[j];
					}
				}
			}
			
			j = 0;
			for (int i = 0; i < 13; i ++) {
				if ((i > 0) && (Stat[i] != Stat[i-1])) Respond += "\n";
				if (Stat[i] > 0) Respond += "+";
				Respond += Stat[i] + " ";
				j += Stat[i];
			}
			
			Respond += "\n**Сумма: " + j + "**";
			
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}
		
		[Command("изизз")]
		public async Task HeroNaborZZ (CommandContext context) {
			string Respond = context.Message.Author.Mention + "** раскидывает три набора**\n";
			
			Random GreenDie = new Random ();
			Random RedDie = new Random ();
			
			int[][] Stat = new int[3][];

			for (int i = 0; i <3; i++) {
				Stat[i] = new int[13];
			}
			
			for (int k = 0; k < 3; k ++) {
				for (int i = 0; i < 13; i ++) Stat[k][i] = GreenDie.Next (1,7) - RedDie.Next (1,7);
			}
			
			for (int k = 0; k < 3; k ++) {
				for (int i = 0; i < 12; i ++) {
					for (int j = 12; j > i; j --) {
						if(Stat[k][j] > Stat[k][j - 1]) {
							Stat[k][j - 1] += Stat[k][j];
							Stat[k][j] = Stat[k][j - 1] - Stat[k][j];
							Stat[k][j - 1] -= Stat[k][j];
						}
					}
				}
			}
			
			for (int k = 0; k < 3; k ++) {
				int j = 0;
				for (int i = 0; i < 13; i ++) {
					if ((i > 0) && (Stat[k][i] != Stat[k][i-1])) Respond += "\n";
					if (Stat[k][i] > 0) Respond += "+";
					Respond += Stat[k][i] + " ";
					j += Stat[k][i];
				}
				Respond += "\n**Сумма: " + j + "**\n\n";
			}
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		public static void FillList () {

			Character NewCharacter = new Character();
			NewCharacter.Name = "Поражённая-Стеклом-на-Ветру";
			NewCharacter.Stats[0] = 2;
			NewCharacter.Stats[1] = 2;
			NewCharacter.Stats[2] = 1;
			NewCharacter.Stats[3] = 1;
			NewCharacter.Stats[4] = 4;
			NewCharacter.Stats[5] = 3;
			NewCharacter.Stats[6] =-1;
			NewCharacter.Stats[7] =-3;
			Characters.Add (NewCharacter);

			NewCharacter = new Character();
			NewCharacter.Name = "Лизбид";
			NewCharacter.Stats[0] = 4;
			NewCharacter.Stats[1] = 5;
			NewCharacter.Stats[2] = 4;
			NewCharacter.Stats[3] =-2;
			NewCharacter.Stats[4] = 0;
			NewCharacter.Stats[5] =-5;
			NewCharacter.Stats[6] =-2;
			NewCharacter.Stats[7] =-4;
			Characters.Add (NewCharacter);

			NewCharacter = new Character();
			NewCharacter.Name = "Дитрав";
			NewCharacter.Stats[0] = 3;
			NewCharacter.Stats[1] = 5;
			NewCharacter.Stats[2] = 2;
			NewCharacter.Stats[3] =-3;
			NewCharacter.Stats[4] = 2;
			NewCharacter.Stats[5] = 0;
			NewCharacter.Stats[6] = 1;
			NewCharacter.Stats[7] =-3;
			Characters.Add (NewCharacter);

			NewCharacter = new Character();
			NewCharacter.Name = "Энталана";
			NewCharacter.Stats[0] = 4;
			NewCharacter.Stats[1] = 0;
			NewCharacter.Stats[2] = 0;
			NewCharacter.Stats[3] = 2;
			NewCharacter.Stats[4] = 3;
			NewCharacter.Stats[5] =-2;
			NewCharacter.Stats[6] = 0;
			NewCharacter.Stats[7] = 2;
			Characters.Add (NewCharacter);

			NewCharacter = new Character();
			NewCharacter.Name = "Юллан";
			NewCharacter.Stats[0] = 3;
			NewCharacter.Stats[1] = 1;
			NewCharacter.Stats[2] = 4;
			NewCharacter.Stats[3] = 0;
			NewCharacter.Stats[4] = 1;
			NewCharacter.Stats[5] = 0;
			NewCharacter.Stats[6] = 1;
			NewCharacter.Stats[7] =-2;
			Characters.Add (NewCharacter);


			//Загрузка обетов
			Sacraments.Clear ();
			StreamReader SRSacrament = new StreamReader ("Txts/Sacrament.txt");
			string EachLine = "";
			while ((EachLine = SRSacrament.ReadLine ()) != null) {
				Sacraments.Add (EachLine);
			}
		}
	}
}
