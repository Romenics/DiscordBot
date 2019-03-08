using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Collections; //ArrayList вроде как отсюда
using System.IO; //Реально не знаю нужно ли



namespace DiscordBot {
	
	public class VozhbanCommands {
		
		static List<string> Size = new List<string> ();
		static List<string> Adj = new List<string> ();
		static List<string> AdjPl = new List<string> ();
		static List<string> Base = new List<string> ();
		static List<string> BaseRP = new List<string> ();
		static List<string> Limbs = new List<string> ();

		static int SizeCount = 0;
		static int AdjCount = 0;
		static int AdjPlCount = 0;
		static int BaseCount = 0;
		static int BaseRPCount = 0;
		static int LimbsCount = 0;

		string Str = "";

		public static void LoadText () {
			//В теории, должно дёргать всякое из файлов
			StreamReader SRSize = new StreamReader ("DiscordBot/Txts/Size.txt");
			while ((Str = SRSize.ReadLine ()) != null) {
				Size.Add ( Str );
				SizeCount++;
			}
			
			StreamReader SRAdj = new StreamReader ("DiscordBot/Txts/Adj.txt");
			while ((Str = SRAdj.ReadLine ()) != null) {
				Adj.Add ( Str );
				AdjCount++;
			}
			
			StreamReader SRAdjPl = new StreamReader ("DiscordBot/Txts/Adj.txt");
			while ((Str = SRAdjPl.ReadLine ()) != null) {
				AdjPl.Add ( Str );
				AdjPlCount++;
			}
			
			StreamReader SRBase = new StreamReader ("DiscordBot/Txts/Base.txt");
			while ((Str = SRBase.ReadLine ()) != null) {
				Base.Add ( Str );
				BaseCount++;
			}
			
			StreamReader SRBaseRP = new StreamReader ("DiscordBot/Txts/BaseRP.txt");
			while ((Str = SRBaseRP.ReadLine ()) != null) {
				BaseRP.Add ( Str );
				BaseRPCount++;
			}
			
			StreamReader SRLimbs = new StreamReader ("DiscordBot/Txts/Limbs.txt");
			while ((Str = SRLimbs.ReadLine ()) != null) {
				Limbs.Add ( Str );
				LimbsCount++;
			}
	
		}

		[Command("форготня")]
		public async Task ForgottenBeast (CommandContext context) {
			
			Random Chance = new Random ();
			
			private int debug = 1; //Нужно мне, чтобы выводить всякое в дискорд.
			
			if (debug == 1) {
				Respond += "```\nSize.txt has " + SizeCount + " lines\n";
				Respond += "Adj.txt has " + AdjCount + " lines\n";
				Respond += "AdjPl.txt has " + AdjPlCount + " lines\n";
				Respond += "Base.txt has " + BaseCount + " lines\n";
				Respond += "BaseRP.txt has " + BaseRPCount + " lines\n";
				Respond += "Limbs.txt has " + LimbsCount + " lines\n```\n\n";
			}
			
			//Тестовая версия теоритически содержит всё до индивидуальных конечностей

			/*
			Формула Форготни:
			Забытое чудовище *Имя* выглядит как
			*Размер* *50% Прилаг* *25% Прилаг* *Внешность*
			с *50% Прилаг* *25% Прилаг* головой похожей на *Внешность РП*
			75% тело которого покрывают *50% ПрилагМн* *25% ПрилагМн* *Конечности* 50% и *50% ПрилагМн* *25% ПрилагМн* *Конечности*
			=== Конечности от 0 до 100 шт ===
			*Положение* растёт *50% Прилаг* *25% Прилаг* *Конечность* как *25% Внешность РП* из *тип материала* которую покрывают *50% Конечности*.
			*/
			
			Respond += "Забытое чудовище *Имя* выглядит как " + Size[Chance.Next (0, SizeCount) + " ";	//Забытое чудовище *Имя* выглядит как *Размер*
			if (Chance.Next (0, 100) < 50) {
				Respond += Adj[Chance.Next (0, AdjCount) + " ";	//*50% Прилаг*
			}
			if (Chance.Next (0, 100) < 25) {
				Respond += Adj[Chance.Next (0, AdjCount) + " ";	//*25% Прилаг*
			}
			Respond += Base[Chance.Next (0, BaseCount) + " c ";	//*Внешность* с 
			if (Chance.Next (0, 100) < 50) {
				Respond += Adj[Chance.Next (0, AdjCount) + " ";	//*50% Прилаг*
			}
			if (Chance.Next (0, 100) < 25) {
				Respond += Adj[Chance.Next (0, AdjCount) + " ";	//*25% Прилаг*
			}
			Respond += "головой похожей на " + BaseRP[Chance.Next (0, BaseRPCount);	//головой похожей на *Внешность РП*
			if (Chance.Next (0, 100) < 75) {
				Respond += " тело которого покрывают ";	//75% тело которого покрывают
				if (Chance.Next (0, 100) < 50) {
					Respond += AdjPl[Chance.Next (0, AdjCount) + " ";	//*50% ПрилагМн*
				}
				if (Chance.Next (0, 100) < 25) {
					Respond += AdjPl[Chance.Next (0, AdjCount) + " ";	//*25% ПрилагМн*
				}
				Respond += Limbs[Chance.Next (0, LimbsCount);	//*Конечности*
				if (Chance.Next (0, 100) < 50) {
					Respond += " и ";	//50% и 
					if (Chance.Next (0, 100) < 50) {
						Respond += AdjPl[Chance.Next (0, AdjCount) + " ";	//*50% ПрилагМн*
					}
					if (Chance.Next (0, 100) < 25) {
						Respond += AdjPl[Chance.Next (0, AdjCount) + " ";	//*25% ПрилагМн*
					}
					Respond += Limbs[Chance.Next (0, LimbsCount);	//*Конечности*
				}
			}
			Respond += ".";	//. Дальше должны пойти индивидуальные конечности. Потом. Когда заработает эта часть.
			
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}
	}
}
