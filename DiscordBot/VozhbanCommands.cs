using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.IO; //Реально не знаю нужно ли



namespace DiscordBot {
	
	public class VozhbanCommands {
		
		static List<string> Size   = new List<string> ();
		static List<string> Adj    = new List<string> ();
		static List<string> AdjPl  = new List<string> ();
		static List<string> Base   = new List<string> ();
		static List<string> BaseRP = new List<string> ();
		static List<string> Limbs  = new List<string> ();
		static List<string> Limb  = new List<string> ();
		static List<string> Spot  = new List<string> ();
		static List<string> Mat  = new List<string> ();
		static List<string> Aura  = new List<string> ();
		static List<string> Danger  = new List<string> ();
		static List<string> Liquid  = new List<string> ();

		static int SizeCount = 0;
		static int AdjCount = 0;
		static int AdjPlCount = 0;
		static int BaseCount = 0;
		static int BaseRPCount = 0;
		static int LimbsCount = 0;
		static int LimbCount = 0;
		static int SpotCount = 0;
		static int MatCount = 0;
		static int AuraCount = 0;
		static int DangerCount = 0;
		static int LiquidCount = 0;

		static string Str = "";

		public static void LoadText () {
			//В теории, должно дёргать всякое из файлов
			StreamReader SRSize = new StreamReader ("Txts/Size.txt");
			while ((Str = SRSize.ReadLine ()) != null) {
				Size.Add ( Str );
				SizeCount++;
			}
			
			StreamReader SRAdj = new StreamReader ("Txts/Adj.txt");
			while ((Str = SRAdj.ReadLine ()) != null) {
				Adj.Add ( Str );
				AdjCount++;
			}
			
			StreamReader SRAdjPl = new StreamReader ("Txts/AdjPl.txt");
			while ((Str = SRAdjPl.ReadLine ()) != null) {
				AdjPl.Add ( Str );
				AdjPlCount++;
			}
			
			StreamReader SRBase = new StreamReader ("Txts/Base.txt");
			while ((Str = SRBase.ReadLine ()) != null) {
				Base.Add ( Str );
				BaseCount++;
			}
			
			StreamReader SRBaseRP = new StreamReader ("Txts/BaseRP.txt");
			while ((Str = SRBaseRP.ReadLine ()) != null) {
				BaseRP.Add ( Str );
				BaseRPCount++;
			}
			
			StreamReader SRLimbs = new StreamReader ("Txts/Limbs.txt");
			while ((Str = SRLimbs.ReadLine ()) != null) {
				Limbs.Add ( Str );
				LimbsCount++;
			}
			
			StreamReader SRLimb = new StreamReader ("Txts/Limb.txt");
			while ((Str = SRLimb.ReadLine ()) != null) {
				Limb.Add ( Str );
				LimbCount++;
			}
			
			StreamReader SRSpot = new StreamReader ("Txts/Spot.txt");
			while ((Str = SRSpot.ReadLine ()) != null) {
				Str = Str[0].ToUpper() + Str.Substring(1);
				Spot.Add ( Str );
				SpotCount++;
			}
			
			StreamReader SRMat = new StreamReader ("Txts/Mat.txt");
			while ((Str = SRMat.ReadLine ()) != null) {
				Mat.Add ( Str );
				MatCount++;
			}
			
			StreamReader SRAura = new StreamReader ("Txts/Aura.txt");
			while ((Str = SRAura.ReadLine ()) != null) {
				Aura.Add ( Str );
				AuraCount++;
			}
			
			StreamReader SRDanger = new StreamReader ("Txts/Danger.txt");
			while ((Str = SRDanger.ReadLine ()) != null) {
				Danger.Add ( Str );
				DangerCount++;
			}
			
			StreamReader SRLiquid = new StreamReader ("Txts/Liquid.txt");
			while ((Str = SRLiquid.ReadLine ()) != null) {
				Liquid.Add ( Str );
				LiquidCount++;
			}
	
		}

		[Command("форготня")]
		public async Task ForgottenBeast (CommandContext context) {
			
			Random Chance = new Random ();

			string Respond = "";
			
			//Тестовая версия теоритически содержит всё до индивидуальных конечностей

			/*
			Формула Форготни:
			Забытое чудовище *Имя* выглядит как
			*Размер* *50% Прилаг* *25% Прилаг* *Внешность*
			с *50% Прилаг* *25% Прилаг* головой похожей на *Внешность РП*
			75% тело которого покрывают *50% ПрилагМн* *25% ПрилагМн* *Конечности* 50% и *50% ПрилагМн* *25% ПрилагМн* *Конечности*
			*/
			
			Respond += "Забытое чудовище *Имя* выглядит как " + Size[Chance.Next (0, SizeCount)] + " ";	//Забытое чудовище *Имя* выглядит как *Размер*
			if (Chance.Next (0, 100) < 50) {
				Respond += Adj[Chance.Next (0, AdjCount)] + " ";	//*50% Прилаг*
			}
			if (Chance.Next (0, 100) < 25) {
				Respond += Adj[Chance.Next (0, AdjCount)] + " ";	//*25% Прилаг*
			}
			Respond += Base[Chance.Next (0, BaseCount)] + " c ";	//*Внешность* с 
			if (Chance.Next (0, 100) < 50) {
				Respond += Adj[Chance.Next (0, AdjCount)] + " ";	//*50% Прилаг*
			}
			if (Chance.Next (0, 100) < 25) {
				Respond += Adj[Chance.Next (0, AdjCount)] + " ";	//*25% Прилаг*
			}
			Respond += "головой похожей на " + BaseRP[Chance.Next (0, BaseRPCount)];	//головой похожей на *Внешность РП*
			if (Chance.Next (0, 100) < 75) {
				Respond += " тело которого покрывают ";	//75% тело которого покрывают
				if (Chance.Next (0, 100) < 50) {
					Respond += AdjPl[Chance.Next (0, AdjCount)] + " ";	//*50% ПрилагМн*
				}
				if (Chance.Next (0, 100) < 25) {
					Respond += AdjPl[Chance.Next (0, AdjCount)] + " ";	//*25% ПрилагМн*
				}
				Respond += Limbs[Chance.Next (0, LimbsCount)];	//*Конечности*
				if (Chance.Next (0, 100) < 50) {
					Respond += " и ";	//50% и 
					if (Chance.Next (0, 100) < 50) {
						Respond += AdjPl[Chance.Next (0, AdjPlCount)] + " ";	//*50% ПрилагМн*
					}
					if (Chance.Next (0, 100) < 25) {
						Respond += AdjPl[Chance.Next (0, AdjPlCount)] + " ";	//*25% ПрилагМн*
					}
					Respond += Limbs[Chance.Next (0, LimbsCount)];	//*Конечности*
				}
			}
			Respond += ".\n";	//.
			/*
			=== Конечности от 0 до 100 шт ===
			*Положение* растёт *50% Прилаг* *25% Прилаг* *Конечность* как *25% Внешность РП* из *тип материала* которую покрывают *50% Конечности*.
			*/
			
			for (int i = 0; i < 4; i ++) {
				if (Chance.Next (0, 100) < 50) {
					Respond += Spot[Chance.Next (0, SpotCount)] + " растёт ";
					if (Chance.Next (0, 100) < 50) {
						Respond += Adj[Chance.Next (0, AdjCount)] + " ";
					}
					if (Chance.Next (0, 100) < 25) {
						Respond += Adj[Chance.Next (0, AdjCount)] + " ";
					}
					Respond += Limb[Chance.Next (0, LimbCount)] + " ";
					if (Chance.Next (0, 100) < 25) {
						Respond += "как у " + BaseRP[Chance.Next (0, BaseRPCount)] + " ";
					}
					Respond += "из " Mat[Chance.Next (0, MatCount)] + " ";
					if (Chance.Next (0, 100) < 50) {
						Respond += "которую покрывают "
						if (Chance.Next (0, 100) < 50) {
							Respond += AdjPl[Chance.Next (0, AdjPlCount)] + " ";
						}
						if (Chance.Next (0, 100) < 50) {
							Respond += AdjPl[Chance.Next (0, AdjPlCount)] + " ";
						}
						Respond += Limbs[Chance.Next (0, LimbsCount)] + ".\n";
					}
				}
			}
			
			for (i; i < 10; i ++) {
				if (Chance.Next (0, 100) < 20) {
					Respond += Spot[Chance.Next (0, SpotCount)] + " растёт ";
					if (Chance.Next (0, 100) < 50) {
						Respond += Adj[Chance.Next (0, AdjCount)] + " ";
					}
					if (Chance.Next (0, 100) < 25) {
						Respond += Adj[Chance.Next (0, AdjCount)] + " ";
					}
					Respond += Limb[Chance.Next (0, LimbCount)] + " ";
					if (Chance.Next (0, 100) < 25) {
						Respond += "как у " + BaseRP[Chance.Next (0, BaseRPCount)] + " ";
					}
					Respond += "из " Mat[Chance.Next (0, MatCount)] + " ";
					if (Chance.Next (0, 100) < 50) {
						Respond += "которую покрывают "
						if (Chance.Next (0, 100) < 50) {
							Respond += AdjPl[Chance.Next (0, AdjPlCount)] + " ";
						}
						if (Chance.Next (0, 100) < 50) {
							Respond += AdjPl[Chance.Next (0, AdjPlCount)] + " ";
						}
						Respond += Limbs[Chance.Next (0, LimbsCount)] + ".\n";
					}
				}
			}
			
			for (i; i < 20; i ++) {
				if (Chance.Next (0, 100) < 10) {
					Respond += Spot[Chance.Next (0, SpotCount)] + " растёт ";
					if (Chance.Next (0, 100) < 50) {
						Respond += Adj[Chance.Next (0, AdjCount)] + " ";
					}
					if (Chance.Next (0, 100) < 25) {
						Respond += Adj[Chance.Next (0, AdjCount)] + " ";
					}
					Respond += Limb[Chance.Next (0, LimbCount)] + " ";
					if (Chance.Next (0, 100) < 25) {
						Respond += "как у " + BaseRP[Chance.Next (0, BaseRPCount)] + " ";
					}
					Respond += "из " Mat[Chance.Next (0, MatCount)] + " ";
					if (Chance.Next (0, 100) < 50) {
						Respond += "которую покрывают "
						if (Chance.Next (0, 100) < 50) {
							Respond += AdjPl[Chance.Next (0, AdjPlCount)] + " ";
						}
						if (Chance.Next (0, 100) < 50) {
							Respond += AdjPl[Chance.Next (0, AdjPlCount)] + " ";
						}
						Respond += Limbs[Chance.Next (0, LimbsCount)] + ".\n";
					}
				}
			}
			
			for (i; i < 35; i ++) {
				if (Chance.Next (0, 100) < 5) {
					Respond += Spot[Chance.Next (0, SpotCount)] + " растёт ";
					if (Chance.Next (0, 100) < 50) {
						Respond += Adj[Chance.Next (0, AdjCount)] + " ";
					}
					if (Chance.Next (0, 100) < 25) {
						Respond += Adj[Chance.Next (0, AdjCount)] + " ";
					}
					Respond += Limb[Chance.Next (0, LimbCount)] + " ";
					if (Chance.Next (0, 100) < 25) {
						Respond += "как у " + BaseRP[Chance.Next (0, BaseRPCount)] + " ";
					}
					Respond += "из " Mat[Chance.Next (0, MatCount)] + " ";
					if (Chance.Next (0, 100) < 50) {
						Respond += "которую покрывают "
						if (Chance.Next (0, 100) < 50) {
							Respond += AdjPl[Chance.Next (0, AdjPlCount)] + " ";
						}
						if (Chance.Next (0, 100) < 50) {
							Respond += AdjPl[Chance.Next (0, AdjPlCount)] + " ";
						}
						Respond += Limbs[Chance.Next (0, LimbsCount)] + ".\n";
					}
				}
			}
			
			for (i; i < 100; i ++) {
				if (Chance.Next (0, 100) < 1) {
					Respond += Spot[Chance.Next (0, SpotCount)] + " растёт ";
					if (Chance.Next (0, 100) < 50) {
						Respond += Adj[Chance.Next (0, AdjCount)] + " ";
					}
					if (Chance.Next (0, 100) < 25) {
						Respond += Adj[Chance.Next (0, AdjCount)] + " ";
					}
					Respond += Limb[Chance.Next (0, LimbCount)] + " ";
					if (Chance.Next (0, 100) < 25) {
						Respond += "как у " + BaseRP[Chance.Next (0, BaseRPCount)] + " ";
					}
					Respond += "из " + Mat[Chance.Next (0, MatCount)] + " ";
					if (Chance.Next (0, 100) < 50) {
						Respond += "которую покрывают "
						if (Chance.Next (0, 100) < 50) {
							Respond += AdjPl[Chance.Next (0, AdjPlCount)] + " ";
						}
						if (Chance.Next (0, 100) < 50) {
							Respond += AdjPl[Chance.Next (0, AdjPlCount)] + " ";
						}
						Respond += Limbs[Chance.Next (0, LimbsCount)] + ".\n";
					}
				}
			}

			Respond += "Его плоть из " + Mat[Chance.Next (0, MatCount)] + ".\n";
			Respond += "Его внутренности из " + Mat[Chance.Next (0, MatCount)] + ".\n";
			Respond += "Его важные органы из " + Mat[Chance.Next (0, MatCount)] + ".\n";
			Respond += "В его жилах течёт " + Liquid[Chance.Next (0, LiquidCount)] + ".\n";
			Respond += "Его окружает аура " + Aura[Chance.Next (0, AuraCount)];
			if (Chance.Next (0, 100) < 50) {
				Respond += " и " + Aura[Chance.Next (0, AuraCount)];
			}
			Respond += ".\nОстерегайтесь его " + Danger[Chance.Next (0, DangerCount)] + ".";
			
			
			await context.RespondAsync (Respond);
			await context.Message.DeleteAsync();
		}
	}
}
