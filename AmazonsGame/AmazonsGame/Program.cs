using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AmazonsGame
{
	class Amazon
	{
		public readonly int N;
		public int Meret
		{
			get { return N*N; }
		}
		public int Jatekos;

		//Tábla étékei: 0 - üres - 'o', 1/2 - fehér/fekete bábú - 'w/b', 3 - nyíl - 'x'
		public int[] Tabla { get; set; }
		public IList<int> FeherBabuk { get; set; }
		public IList<int> FeketeBabuk { get; set; }
		
		public Amazon(int n)
		{
			N = n;
			Tabla = new int[Meret];
			FeherBabuk = new List<int>();
			FeketeBabuk = new List<int>();
		}

		//Bábu és nyíl rátétele egy táblára
		public void AddBabu(int jatekos, int hely)
		{
			Tabla[hely] = jatekos;
			if(jatekos == 1)
			{
				FeherBabuk.Add(hely);
			}
			else
			{
				FeketeBabuk.Add(hely);
			}
		}
		public void AddNyil(int hely)
		{
			Tabla[hely] = 3;
		}

		//Kezdőpozíciók beállítása - Ez lehet nem csak egy játék kezdete, hanem egy adott állapot beolvasásánál is. 
		public void KezdoPoziciok(int[] feherKezdes, int[] feketeKezdes)
		{
			for(int i = 0; i < feherKezdes.Count(); i++)
			{
				AddBabu(1, feherKezdes[i]);
				AddBabu(2, feketeKezdes[i]);
			}
		}

		//Tábla/Állapottér kirajzolása
		public void TablaNyomtat(int[] tabla, bool coloredBefore, bool coloredAfter, int babu, int hova, int nyil)
		{
			Console.Write('\n');
			Console.Write("    ");
			for (int j = 0; j < N; j++)
			{
				Console.Write(" " + j);
			}
			Console.Write('\n');
			Console.Write("   /");
			for (int j = 0; j < N; j++)
			{
				Console.Write("--");
			}
			Console.Write("-\\\n");
			for (int i = 0; i < N; i++)
			{
				Console.Write(" " + i + " |");
				for (int j = 0; j < N; j++)
				{
					Console.Write(' ');//.Append(tabla[i * N + j]);
					switch (tabla[i * N + j])
					{
						case 0:
							Console.Write('o');
							break;
						case 1:
							if ( (coloredBefore && ((i * N + j) == babu)) || (coloredAfter && ((i * N + j) == hova)) )
							{
								Console.BackgroundColor = ConsoleColor.Blue;
							}
							Console.Write('w');
							Console.BackgroundColor = ConsoleColor.Black;
							break;
						case 2:
							if ( (coloredBefore && ((i * N + j) == babu)) || (coloredAfter && ((i * N + j) == hova)) )
							{
								Console.BackgroundColor = ConsoleColor.Blue;
							}
							Console.Write('b');
							Console.BackgroundColor = ConsoleColor.Black;
							break;
						case 3:
							if (coloredAfter && ((i * N + j) == nyil))
							{
								Console.BackgroundColor = ConsoleColor.Red;
							}
							Console.Write('x');
							Console.BackgroundColor = ConsoleColor.Black;
							break;
						default:
							break;
					}
				}
				Console.Write(" |\n");
			}
			Console.Write("   \\");
			for (int j = 0; j < N; j++)
			{
				Console.Write("--");
			}
			Console.Write("-/\n\n");
		}

		//1 mozgás utánni összes lövés listája
		IList<Lepes> Lovesek(Lepes BabuMozgas, bool max)
		{
			//--Debug--
			////Console.WriteLine("IN:Lovesek");
			////Console.WriteLine("BabuMozgas: " + BabuMozgas.GetHashCode());
			//--Debug--
			IList<Lepes> Lista = new List<Lepes>();
			int x = BabuMozgas.Hova % N;
			int y = BabuMozgas.Hova / N;
			//--Debug--
			////Console.WriteLine("Babu lovesben: " + BabuMozgas.Babu + ", Hova lovesben: " + BabuMozgas.Hova);
			//--Debug--
			//Fel
			for (int b = y-1; b >= 0 && (BabuMozgas.JatekTer[b * N + x] == 0); b--)
			{
				Lepes LepesLovessel = BabuMozgas.Masolas();
				LepesLovessel.Nyil = b * N + x;
				LepesLovessel.Vegrehajtas(max);
				Lista.Add(LepesLovessel);
			}
			//Jobbra Fel
			for (int a = x+1, b = y-1; a < N && b >= 0 && (BabuMozgas.JatekTer[b * N + a] == 0); a++, b--)
			{
				Lepes LepesLovessel = BabuMozgas.Masolas();
				LepesLovessel.Nyil = b * N + a;
				LepesLovessel.Vegrehajtas(max);
				Lista.Add(LepesLovessel);
			}
			//Jobbra
			for (int a = x+1; a < N && (BabuMozgas.JatekTer[y * N + a] == 0); a++)
			{
				Lepes LepesLovessel = BabuMozgas.Masolas();
				LepesLovessel.Nyil = y * N + a;
				LepesLovessel.Vegrehajtas(max);
				Lista.Add(LepesLovessel);
			}
			//Jobbra Le
			for (int a = x+1, b = y+1; a < N && b < N && (BabuMozgas.JatekTer[b * N + a] == 0); a++, b++)
			{
				Lepes LepesLovessel = BabuMozgas.Masolas();
				LepesLovessel.Nyil = b * N + a;
				LepesLovessel.Vegrehajtas(max);
				Lista.Add(LepesLovessel);
			}
			//Le
			for (int b = y+1; b < N && (BabuMozgas.JatekTer[b * N + x] == 0); b++)
			{
				Lepes LepesLovessel = BabuMozgas.Masolas();
				LepesLovessel.Nyil = b * N + x;
				LepesLovessel.Vegrehajtas(max);
				Lista.Add(LepesLovessel);
			}
			//Balra Le
			for (int a = x-1, b = y+1; a >= 0 && b < N && (BabuMozgas.JatekTer[b * N + a] == 0); a--, b++)
			{
				Lepes LepesLovessel = BabuMozgas.Masolas();
				LepesLovessel.Nyil = b * N + a;
				LepesLovessel.Vegrehajtas(max);
				Lista.Add(LepesLovessel);
			}
			//Balra
			for (int a = x-1; a >= 0 && (BabuMozgas.JatekTer[y * N + a] == 0); a--)
			{
				Lepes LepesLovessel = BabuMozgas.Masolas();
				LepesLovessel.Nyil = y * N + a;
				LepesLovessel.Vegrehajtas(max);
				Lista.Add(LepesLovessel);
			}
			//Balra Fel
			for (int a = x-1, b = y-1; a >= 0 && b >= 0 && (BabuMozgas.JatekTer[b * N + a] == 0); a--, b--)
			{
				Lepes LepesLovessel = BabuMozgas.Masolas();
				LepesLovessel.Nyil = b * N + a;
				LepesLovessel.Vegrehajtas(max);
				Lista.Add(LepesLovessel);
			}
			//--Debug--
			////Console.WriteLine(Lista.Count);
			//--Debug--
			return Lista;
		}

		//Az összes lépés listája (lövésekkel együtt)
		IList<Lepes> Lepesek(Lepes Allapot, bool max)
        {
			IList<Lepes> LepesLista = new List<Lepes>();
			//Mind a 4 bábúval léphetek
			//foreach (int babu in Allapot.JatekosBabuk)
			int babuszam = Allapot.JatekosBabuk.Count();
			for(int i = 0; i < babuszam; i++)
			{
				int babu = Allapot.JatekosBabuk[i];
				Lepes allapotCopy = Allapot.Masolas();
				//--Debug--
				////Console.WriteLine("Bábu: " + babu);
				////TablaNyomtat(allapotCopy.JatekTer);
				////Console.WriteLine("Allapot: " + Allapot.GetHashCode());
				////Console.WriteLine("AllapotCopy: " + allapotCopy.GetHashCode());
				//--Debug--
				allapotCopy.Babu = babu;
				//--Debug--
				////Console.WriteLine(string.Join(", ", allapotCopy.JatekosBabuk));
				////Console.WriteLine(allapotCopy.Babu);
				//--Debug--
				//Csillag alakban tudunk lépni, ezeket a lépéseket gyűjtöm össze.
				//Csillag alakban tudok nyilat lőni, ezeket is ki kell gyűjtenem minden lépéshez.
				int x = babu % N;
				int y = babu / N;
				//Fel
				for (int b = y-1; b >= 0 && (allapotCopy.JatekTer[b * N + x] == 0); b--)
				{
					//--Debug--
					////Console.WriteLine("IN:FEL");
					//--Debug--
					Lepes babuMozgas = allapotCopy.Masolas();
					//--Debug--
					////Console.WriteLine("babuMozgas: " + babuMozgas.GetHashCode());
					//--Debug--
					babuMozgas.Hova = b * N + x;
					babuMozgas.Nyil = -1;
					babuMozgas.Vegrehajtas(max);
					IList<Lepes> lovesek = Lovesek(babuMozgas, max);
					//--Debug--
					////Console.WriteLine("Lovesek Utan");
					//--Debug--
					foreach (Lepes l in lovesek)
					{
						LepesLista.Add(l);
					}
				}
				//Jobbra Fel
				for (int a = x+1, b = y-1; a < N && b >= 0 && (allapotCopy.JatekTer[b * N + a] == 0); a++, b--)
				{
					Lepes babuMozgas = allapotCopy.Masolas();
					babuMozgas.Hova = b * N + a;
					babuMozgas.Nyil = -1;
					babuMozgas.Vegrehajtas(max);
					IList<Lepes> lovesek = Lovesek(babuMozgas, max);
					foreach (Lepes l in lovesek)
					{
						LepesLista.Add(l);
					}
				}
				//Jobbra
				for (int a = x+1; a < N && (allapotCopy.JatekTer[y * N + a] == 0); a++)
				{
					Lepes babuMozgas = allapotCopy.Masolas();
					babuMozgas.Hova = y * N + a;
					babuMozgas.Nyil = -1;
					babuMozgas.Vegrehajtas(max);
					IList<Lepes> lovesek = Lovesek(babuMozgas, max);
					foreach (Lepes l in lovesek)
					{
						LepesLista.Add(l);
					}
				}
				//Jobbra Le
				for (int a = x+1, b = y+1; a < N && b < N && (allapotCopy.JatekTer[b * N + a] == 0); a++, b++)
				{
					Lepes babuMozgas = allapotCopy.Masolas();
					babuMozgas.Hova = b * N + a;
					babuMozgas.Nyil = -1;
					babuMozgas.Vegrehajtas(max);
					IList<Lepes> lovesek = Lovesek(babuMozgas, max);
					foreach (Lepes l in lovesek)
					{
						LepesLista.Add(l);
					}
				}
				//Le
				for (int b = y+1; b < N && (allapotCopy.JatekTer[b * N + x] == 0); b++)
				{
					Lepes babuMozgas = allapotCopy.Masolas();
					babuMozgas.Hova = b * N + x;
					babuMozgas.Nyil = -1;
					babuMozgas.Vegrehajtas(max);
					IList<Lepes> lovesek = Lovesek(babuMozgas, max);
					foreach (Lepes l in lovesek)
					{
						LepesLista.Add(l);
					}
				}
				//Balra Le
				for (int a = x-1, b = y+1; a >= 0 && b < N && (allapotCopy.JatekTer[b * N + a] == 0); a--, b++)
				{
					Lepes babuMozgas = allapotCopy.Masolas();
					babuMozgas.Hova = b * N + a;
					babuMozgas.Nyil = -1;
					babuMozgas.Vegrehajtas(max);
					IList<Lepes> lovesek = Lovesek(babuMozgas, max);
					foreach (Lepes l in lovesek)
					{
						LepesLista.Add(l);
					}
				}
				//Balra
				for (int a = x-1; a >= 0 && (allapotCopy.JatekTer[y * N + a] == 0); a--)
				{
					Lepes babuMozgas = allapotCopy.Masolas();
					babuMozgas.Hova = y * N + a;
					babuMozgas.Nyil = -1;
					babuMozgas.Vegrehajtas(max);
					IList<Lepes> lovesek = Lovesek(babuMozgas, max);
					foreach (Lepes l in lovesek)
					{
						LepesLista.Add(l);
					}
				}
				//Balra Fel
				for (int a = x-1, b = y-1; a >= 0 && b >= 0 && (allapotCopy.JatekTer[b * N + a] == 0); a--, b--)
				{
					Lepes babuMozgas = allapotCopy.Masolas();
					babuMozgas.Hova = b * N + a;
					babuMozgas.Nyil = -1;
					babuMozgas.Vegrehajtas(max);
					IList<Lepes> lovesek = Lovesek(babuMozgas, max);
					foreach (Lepes l in lovesek)
					{
						LepesLista.Add(l);
					}
				}
			}
			//--Debug--
			////Console.WriteLine(LepesLista.Count);
			////Console.WriteLine(string.Join("|", LepesLista));
			//--Debug--
			return LepesLista;
		}

		//HA MIN lépésnél nincsenek lehetséges lépéseim, viszot az előző lépésnek vannak lépéslehetőségei rám nézve, akkor annak maximális az értéke, ugyanis akkor én nyerek.
		//Mert a 0-ás utólépéses lépés csak akkor vég számomra, ha én nem tudnék lépni. Ha az ellenfél nem tud, akkor az csak jó.

		//MiniMax keresés Max része
		public Lepes MaxKeres(Lepes Allapot, int Iteracio)
		{
			IList<Lepes> LepesLista = Lepesek(Allapot, true);
			////--Debug--
			//Console.WriteLine("IN: MAX");
			//Console.WriteLine(Allapot.ToString());
			//Console.WriteLine("J: " + Allapot.Josag);
			//Console.WriteLine("L.Sz.: " + Allapot.LepesekSzama(false));
			//Console.WriteLine("L.V.L.Sz: " + LepesLista.Count);
			////--Debug--
			//Az összegyűjtött lépési lehetőségek közül vagy az értékekek maximumát veszem, vagy a minimumok maximumát.
			int[] ertekek = new int[LepesLista.Count];
			if (Allapot.LepesekSzama(true) > 0)
			{
				if (Iteracio > 0)
				{
					for (int i = 0; i < LepesLista.Count; i++)
					{
						Lepes minLepes = MinKeres(LepesLista[i].Forgatas(), Iteracio - 1);
						if (minLepes != null)
						{
							ertekek[i] = minLepes.Josag;
							LepesLista[i].Josag = minLepes.Josag;
							////--Debug--
							//Console.WriteLine("MinJosag: " + minLepes.Josag);
							////--Degug--
						}
						else
						{
							//--Debug--
							////Console.WriteLine("Lepesek: " + minLepes);
							////Console.WriteLine("IN: MAX - ZERO");
							//--Debug--
							ertekek[i] = 0;
						}
					}
					//Itt nem kell extra feltétel, mert olyan nem kéne hogy előforduljon, hogy a lépő játékos egyszerre tud és nem is tud lépni.
				}
				else
				{
					for (int i = 0; i < LepesLista.Count; i++)
					{
						ertekek[i] = LepesLista[i].Josag;
					}
				}
				//A Max értéket kikeresni csak több értékből lehet, ha nem létezik, akkor 0 lesz, ezt le kell kezelni a hívóban!
				int maxErtek = ertekek.Max();
				////--Debug--
				//if (Iteracio > 0)
				//{
				//	int z = 0;
				//	Console.WriteLine("LepesLista:");
				//	foreach (var l in LepesLista)
				//	{
				//		Console.WriteLine(z + " - " + l.ToString() + " - " + l.Josag);
				//		z++;
				//	}
				//	Console.WriteLine("Ertekek:");
				//	z = 0;
				//	foreach (var ertek in ertekek)
				//	{
				//		Console.WriteLine(z + " - " + ertek);
				//		z++;
				//	}
				//}
				////--Debug--
				return LepesLista.Where(l => l.Josag == maxErtek).FirstOrDefault();
			}
			//Max esetben ez lép életbe, ha a lépő nem tud lépni - értéke ennek a lépésnek minimális.
			else
			{
				return null;
			}
		}
		//MiniMax keresés Min része
		public Lepes MinKeres(Lepes Allapot, int Iteracio)
		{
			IList<Lepes> LepesLista = Lepesek(Allapot, false);
			////--Debug--
			//Console.WriteLine("IN: Min");
			//Console.WriteLine(Allapot.ToString());
			//Console.WriteLine("J: " + Allapot.Josag);
			//Console.WriteLine("L.Sz.: " + Allapot.LepesekSzama(false));
			//Console.WriteLine("L.V.L.Sz: " + LepesLista.Count);
			////--Debug--
			//Az összegyűjtött lépési lehetőségek közül vagy az értékekek maximumát veszem, vagy a minimumok maximumát.
			int[] ertekek = new int[LepesLista.Count];
			if (Allapot.LepesekSzama(false) > 0)
			{
				if (LepesLista.Count > 0)
				{
					if (Iteracio > 0)
					{
						for (int i = 0; i < LepesLista.Count; i++)
						{
							Lepes maxLepes = MaxKeres(LepesLista[i].Forgatas(), Iteracio - 1);
							if (maxLepes != null)
							{
								ertekek[i] = maxLepes.Josag;
								LepesLista[i].Josag = maxLepes.Josag;
								////--Debug--
								//Console.WriteLine("MaxJosag: " + maxLepes.Josag);
								////--Degug--
							}
							else
							{
								ertekek[i] = 0;
							}
						}
					}
					else
					{
						for (int i = 0; i < LepesLista.Count; i++)
						{
							ertekek[i] = LepesLista[i].Josag;
						}
					}
					int minErtek = ertekek.Min();
					return LepesLista.Where(l => l.Josag == minErtek).FirstOrDefault();
				}
				//Ha nem tud lépni az ellenfél (TÉNYLEG SEMMIT SE, mivel a az összes lehetséges lépés meghatározása után van 0-a lehetséges lépés), de én igen
				//-> Ez egy nyertes lépés -> Jutalom
				else
				{
					//Lepes jutalmazott = Allapot.Masolas();
					Allapot.Josag = int.MaxValue-1;
					return Allapot;
				}
			}
			//Ez csak akkor lép életbe, ha a lépő nem tud lépni, ha az ellenfél nem, az nekem jó, ekkor azt a lépést jutalmazni kell. - Ez létható fentebb.
			else
			{
				return null;
			}
		}
	}

	//Egy lépést tárol. Ebben a bábu kiindulási, érkezési és lövési helye van megadva az adott táblán.
	class Lepes
	{
		public int[] JatekTer;
		public int Jatekos;
		public int Babu;
		public int Hova;
		public int Nyil;
		public int[] JatekosBabuk;
		public int[] MasBabuk;

		//Ezen a változón keresztül fogom tudni lekérni az értékét.
		public int Josag;

		public Lepes(int[] jatekTer, int jatekos, int[] jatekosBabuk, int[] masBabuk, int babu, int hova, int nyil)
		{
			JatekTer = jatekTer;
			Jatekos = jatekos;
			Babu = babu;
			Hova = hova;
			Nyil = nyil;
			JatekosBabuk = jatekosBabuk;
			MasBabuk = masBabuk;
		}

		public Lepes(int[] jatekTer, int jatekos, int[] jatekosBabuk, int[] masBabuk)
		{
			JatekTer = jatekTer;
			Jatekos = jatekos;
			JatekosBabuk = jatekosBabuk;
			MasBabuk = masBabuk;
		}

		public override string ToString()
		{
			return Babu + " -> " + Hova + " -> " + Nyil;
		}

		//Lépések megszámolása - MAX esetben a játékos lép, ezt akarom maximalizálni,
		//míg MIN esetben a másik játékos lép, és ő az első játékos lépését akarja minimalizálni.
		public int LepesekSzama(bool max)
		{
			int szum = 0;
			//"Csillag" alakú a lépések keresése.
			int ciklusVeg = max ? JatekosBabuk.Length : MasBabuk.Length;
			for (int i = 0; i < ciklusVeg; i++)
			{
				//Koordináták
				int n = (int)Math.Sqrt(JatekTer.Length);
				int x = (max ? JatekosBabuk[i] : MasBabuk[i]) % n;
				int y = (max ? JatekosBabuk[i] : MasBabuk[i]) / n;
				//Fel
				for (int b = y-1; b >= 0 && (JatekTer[b * n + x] == 0); b--)
					szum++;
				//Jobbra Fel
				for (int a = x+1, b = y-1; a < n && b >= 0 && (JatekTer[b * n + a] == 0); a++, b--)
					szum++;
				//Jobbra
				for (int a = x+1; a < n && (JatekTer[y * n + a] == 0); a++)
					szum++;
				//Jobbra Le
				for (int a = x+1, b = y+1; a < n && b < n && (JatekTer[b * n + a] == 0); a++, b++)
					szum++;
				//Le
				for (int b = y+1; b < n && (JatekTer[b * n + x] == 0); b++)
					szum++;
				//Balra Le
				for (int a = x-1, b = y+1; a >= 0 && b < n && (JatekTer[b * n + a] == 0); a--, b++)
					szum++;
				//Balra
				for (int a = x-1; a >= 0 && (JatekTer[y * n + a] == 0); a--)
					szum++;
				//Balra Fel
				for (int a = x-1, b = y-1; a >= 0 && b >= 0 && (JatekTer[b * n + a] == 0); a--, b--)
					szum++;
			}
			return szum;
		}

		//Lépés végrehajtása - Így a Jatekter továbbadásával a következő lépés alaphelyzetébe lépek.
		// + Heurisztikai érték számítása - MinMax-hoz kell.
		// Az érték a szabadon hagyott további lépések száma alapján rendelődik hozzá a lépéshez.
		//Lehetne az ellenfél lépéseinek a minimalizálása is a heurisztika.
		public void Vegrehajtas(bool max)
		{
			//Végrehajtás 2 lépésben - Elsőre a lépést hajtom végre, majd a lövést.
			if (Nyil < 0)
			{
				//Lépés meghozása
				JatekTer[Babu] = 0;
				JatekTer[Hova] = Jatekos;
				JatekosBabuk[Array.IndexOf(JatekosBabuk, Babu)] = Hova;
			}
			else
			{
				JatekTer[Nyil] = 3;
				//Lehetséges további lépések száma ebben az állapotban
				Josag = LepesekSzama(max);
			}
			//--Debug--
			////Console.WriteLine(ToString());
			////Console.WriteLine(string.Join(", ", JatekosBabuk));
			//--Debug--
		}

		//A következő lépést az ellenfél fogja meghozni.
		public Lepes Forgatas()
		{
			Lepes ertek = this.Masolas();
			ertek.Jatekos = (Jatekos == 1) ? 2 : 1;
			ertek.JatekosBabuk = MasBabuk;
			ertek.MasBabuk = JatekosBabuk;
			return ertek;
		}

		public Lepes Masolas()
		{
			int[] JatekTerCopy = new int[JatekTer.Length];
			JatekTer.CopyTo(JatekTerCopy, 0);
			int[] MasBabukCopy = new int[MasBabuk.Length];
			MasBabuk.CopyTo(MasBabukCopy, 0);
			int[] JatekosBabukCopy = new int[JatekosBabuk.Length];
			JatekosBabuk.CopyTo(JatekosBabukCopy, 0);
			return new Lepes(JatekTerCopy, Jatekos, JatekosBabukCopy, MasBabukCopy, Babu, Hova, Nyil);
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			int n = 10;
			Amazon Jatek = new Amazon(n);
			//TETSZŐLEGES ÁLLAPOT BEOLVASÁSA
			IList<int> feher = new List<int>();
			IList<int> fekete = new List<int>();
			//Tábla étékei: 0 - üre4s - 'o', 1/2 - fehér/fekete bábú - 'w/b', 3 - nyíl - 'x', szóköszözökkel vannak elválasztva a mezők egy soron belül.
			for (int i = 0; i < n; i++)
			{
				string sor = Console.ReadLine();
				string[] mezok = sor.Split(' ');
				for(int j = 0; j < mezok.Length; j++)
				{
					switch (mezok[j][0])
					{
						case 'w':
							Jatek.AddBabu(1, i * n + j);
							break;
						case 'b':
							Jatek.AddBabu(2, i * n + j);
							break;
						case 'x':
							Jatek.AddNyil(i * n + j);
							break;
						default:
							break;
					}
				}
			}
			
			//Az alapértelmezett kezdési helyek.
			//Jatek.KezdoPoziciok(new int[] { 60, 69, 93, 96 }, new int[] { 3, 6, 30, 39 });
			////--Debug--
			//Jatek.TablaNyomtat(Jatek.Tabla);
			//Console.WriteLine(string.Join(", ", Jatek.FeherBabuk));
			//Console.WriteLine(string.Join(", ", Jatek.FeketeBabuk));
			////--Debug--
			//Melyik játékos lép - általában "én"
			//Jatek.Jatekos = 1;
			Console.WriteLine("Melyik játékos lépése jön? [w - fehér / b - fekete]");
			Jatek.Jatekos = (Console.ReadLine()[0] == 'w') ? 1 : 2;
			//Ajánlott lépés meghatározása
			Lepes JelenAllapot = new Lepes(
				Jatek.Tabla,
				Jatek.Jatekos,
				(Jatek.Jatekos == 1) ? Jatek.FeherBabuk.ToArray() : Jatek.FeketeBabuk.ToArray(),
				(Jatek.Jatekos == 1) ? Jatek.FeketeBabuk.ToArray() : Jatek.FeherBabuk.ToArray()
			);

			//ERROR: Ha valamelyik bábu nem tud lépni, akkor amiatt ne akadjon ki, ÉS ha egyik se tud lépni, akkor vesztett az ellenfél.

			//Stopper - Milyen gyors milyen mélyen?
			var watch = System.Diagnostics.Stopwatch.StartNew();
			
			Lepes AjanlottLepes = Jatek.MaxKeres(JelenAllapot, 1);
			string jatekos = (Jatek.Jatekos == 1) ? "fehér" : "fekete";
			if (AjanlottLepes != null)
			{
				Console.WriteLine();
				string babu = "[" + (AjanlottLepes.Babu / n) + "," + (AjanlottLepes.Babu % n) + "]";
				string hova = "[" + (AjanlottLepes.Hova / n) + "," + (AjanlottLepes.Hova % n) + "]";
				string nyil = "[" + (AjanlottLepes.Nyil / n) + "," + (AjanlottLepes.Nyil % n) + "]";
				Console.WriteLine("A "+ jatekos + " játékos számára ajánlott lépés: " + babu + " -> " + hova + " -> " + nyil);
			}
			else
			{
				Console.WriteLine("A " + jatekos + " játékos nem tud már lépni, így vesztett.");
			}
			//Stopper vége
			watch.Stop();
			var elapsedMs = watch.ElapsedMilliseconds;
			Console.WriteLine();
			Console.WriteLine("Futásidő: " + elapsedMs + "ms = " + (elapsedMs / 1000) + "sec = " + ((elapsedMs / 1000) / 60d) + " min.");

			//--Debug--
			if (AjanlottLepes != null)
			{
				Console.WriteLine();
				Console.WriteLine("A tábla a lépés előtt: (az ajánlott bábú kékkel van megjelölve)");
				Jatek.TablaNyomtat(Jatek.Tabla, true, false, AjanlottLepes.Babu, AjanlottLepes.Hova, AjanlottLepes.Nyil);
				Console.WriteLine("A tábla kinézete a lépés után: (a bábú kékkel, a nyíl piropssal van megjelölve)");
				Jatek.TablaNyomtat(AjanlottLepes.JatekTer, false, true, AjanlottLepes.Babu, AjanlottLepes.Hova, AjanlottLepes.Nyil);
			}
			//--Debug--
		}
	}
}
