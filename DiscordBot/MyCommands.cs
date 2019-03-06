using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;


namespace DiscordBot {
	public class MyCommands {

		public static List <Character> Characters = new List<Character>();

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

			IReadOnlyDictionary <ulong, DSharpPlus.Entities.DiscordGuild> allGuilds = Program.discord.Guilds;

			foreach (DSharpPlus.Entities.DiscordGuild each in allGuilds.Values) {
				Respond += each.Name + " owner " + each.Owner.Nickname + " with " + each.MemberCount + " people \n";
			}
			await context.RespondAsync (Respond);
		}

		[Command ("Reverse")]
		public async Task Reverse (CommandContext context) {
			await context.RespondAsync (Program.LastDeletedMessage);
		}

		#region Статы

		[Command ("осил")]
		public async Task Strength (CommandContext context) {
			string Respond = "**Сила** \n";
			Respond += RoleState (0);
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		[Command("овын")]
		public async Task Stamina (CommandContext context) {
			string Respond = "**Выносливость** \n";
			Respond += RoleState (1);
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		[Command("олов")]
		public async Task Agility (CommandContext context) {
			string Respond = "**Ловкость** \n";
			Respond += RoleState (2);
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		[Command("овос")]
		public async Task Perception (CommandContext context) {
			string Respond = "**Восприятие** \n";
			Respond += RoleState (3);
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		[Command("оинт")]
		public async Task Intelligence (CommandContext context) {
			string Respond = "**Интеллект** \n";
			Respond += RoleState (4);
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		[Command("овол")]
		public async Task Will (CommandContext context) {
			string Respond = "**Воля** \n";
			Respond += RoleState (5);
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		[Command("ооба")]
		public async Task Charm (CommandContext context) {
			string Respond = "**Обаяние** \n";
			Respond += RoleState (6);
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		[Command("оуда")]
		public async Task Luck (CommandContext context) {
			string Respond = "**Удача** \n";
			Respond += RoleState (7);
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}

		#endregion

		public static string RoleState (int State) {

			string Respond = "";

			for (int i = 0; i < Characters.Count; i ++) {
				Random FirstD6  = new Random ();
				Random SecondD6 = new Random ();

				int Result = FirstD6.Next (1,7) - SecondD6.Next (1,7) + Characters[i].Stats[State];
				Respond += Characters[i].Name + " stat: " + Characters[i].Stats[State] + " rolled: " +  Result + "\n"; 
			}

			return Respond;
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
		}
	}
}
