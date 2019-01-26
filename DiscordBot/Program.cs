using System;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;

namespace DiscordBot {
	class Program {
		
		static DiscordClient discord;

		public static int PartyCount;

		static void Main (string[] args) {
			MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		static async Task MainAsync (string[] args) {
			
			DiscordConfiguration DiscordConfig = new DiscordConfiguration {
				Token = "NTMxMjE2MDAyOTU2OTE4ODA0.Dy2k6w.Ly_zkEgU4Udq1jAJc_Mf0SugjUU",
				TokenType = TokenType.Bot 
			};
			
			Console.WriteLine ("Bot staterted");

			discord = new DiscordClient (DiscordConfig);

			//Bot check
			discord.MessageCreated += async e => {
				if (e.Message.Content.ToLower().StartsWith("ping"))
					await e.Message.RespondAsync("pong!");
			};

			//Restore deleting message
			discord.MessageDeleted += async e => {
				string Respond = e.Message.Author.Username +" пытается удалить  " + e.Message.Content + "\n";
				Respond += "Но у него не получается!";
				await e.Message.RespondAsync (Respond);
			};

			//Working when readction added
			discord.MessageReactionAdded += async e => {
				if (e.Emoji.Name == "👍") {
					await e.Message.RespondAsync ("Это палец");
				}
			};

			discord.MessageReactionAdded += async e => {

				if (e.Emoji.Name == "⏲") {
					string Respond = e.User.Username + " готов поиграть в ДС!";
					if (PartyCount > 0) {
						Respond += "\n Кстати в войсе уже " + PartyCount + " людей!";
					}
					await e.Message.RespondAsync (Respond);
				}

				System.Collections.Generic.IReadOnlyList<DSharpPlus.Entities.DiscordConnection> x = await e.Client.ge discord.GetConnectionsAsync ();
				for (int i = 0; i < x.Count; i++) {
					Console.WriteLine ("Get connection");
					if (x[i].Id == 370315299968516114) {

					}
				}
			};
			
			discord.ChannelUpdated += async e => {
				Console.WriteLine ("Channel updated");
				if (e.ChannelAfter.Id == 370315299968516114) {
					Console.WriteLine ("User joined, now party size: " + PartyCount);
					PartyCount ++;
				}

				if (e.ChannelBefore.Id == 370315299968516114) {
					Console.WriteLine ("User left, now party size: " + PartyCount);
					PartyCount --;
				}
			};

		
			
			

			discord.VoiceStateUpdated += async e => {
				
				Console.WriteLine ("Channel updated " + e.Channel.UserLimit);
				Console.WriteLine (e.Channel.Position);
				string Respond = e.User.Username + " updated " + e.Channel.Name;
				if (e.Channel.Name.ToLower () == "таверна") {
					//PartyCount = e
				}
			};

			await discord.ConnectAsync();
            await Task.Delay(-1);
		}

		public void StartTimer (double seconds) {
			System.Timers.Timer timer = new System.Timers.Timer (seconds);
			timer.Elapsed  += new System.Timers.ElapsedEventHandler (StartEvent);
		}

		public static void StartEvent (object source, System.Timers.ElapsedEventArgs e) {

		}
	}
}
