using System;
using System.IO;
using System.Text;

class Program
{
    
    const int RajzlapSzelesseg = 60;
    const int RajzlapMagassag = 20;
    const int RajzlapKezdoX = 26;   
    const int RajzlapKezdoY = 2;
    const int PanelSzel = 25;       
    const int UzenetSor = 24;       

    static char[,] rajzlap = new char[RajzlapSzelesseg, RajzlapMagassag];
    static ConsoleColor[,] szinek = new ConsoleColor[RajzlapSzelesseg, RajzlapMagassag];

    static int kurzoX = 0;
    static int kurzoY = 0;
    static char aktivKarakter = '#';
    static ConsoleColor aktivSzin = ConsoleColor.Cyan;
    static bool eszkozLeteve = false;

    
    static ConsoleColor[] szinLista = { ConsoleColor.Cyan, ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Yellow,
                                        ConsoleColor.Blue, ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Gray };
    static string[] szinNevek = { "cián", "piros", "zöld", "sárga", "kék", "magenta", "fehér", "szürke" };

    
    static string uzenet = "";

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        AlaphelyzetFeltoltes();
        Console.CursorVisible = false;
        Console.Clear();

        bool fut = true;
        bool menuMod = false;
        int menuIndex = 0;
        string[] menuPontok = {
            "Karakter választása",
            "Szín választása",
            "Minta betöltése",
            "Cella törlése",
            "Ugrás középre",
            "Kilépés"
        };

        while (fut)
        {
            Console.CursorVisible = false;
            RajzolFelulet(menuMod, menuIndex, menuPontok);

            if (!menuMod)
            {
                
                Console.SetCursorPosition(RajzlapKezdoX + kurzoX, RajzlapKezdoY + kurzoY);
                Console.CursorVisible = true;
            }

            ConsoleKeyInfo billInfo = Console.ReadKey(true);
            ConsoleKey bill = billInfo.Key;
            uzenet = "";    

            if (menuMod)
            {
                if (bill == ConsoleKey.UpArrow)
                {
                    if (menuIndex > 0) menuIndex--;
                    else Hiba("Ez az első menüpont.");
                }
                else if (bill == ConsoleKey.DownArrow)
                {
                    if (menuIndex < menuPontok.Length - 1) menuIndex++;
                    else Hiba("Ez az utolsó menüpont.");
                }
                else if (bill == ConsoleKey.Escape)
                {
                    menuMod = false;
                }
                else if (bill == ConsoleKey.Enter)
                {
                    switch (menuIndex)
                    {
                        case 0: 
                            KarakterValasztas();
                            break;
                        case 1: 
                            SzinValasztas();
                            break;
                        case 2: 
                            MintaBetoltese();
                            break;
                        case 3: 
                            rajzlap[kurzoX, kurzoY] = ' ';
                            szinek[kurzoX, kurzoY] = ConsoleColor.Gray;
                            uzenet = $"Törölve: ({kurzoX}, {kurzoY})";
                            break;
                        case 4: 
                            kurzoX = RajzlapSzelesseg / 2;
                            kurzoY = RajzlapMagassag / 2;
                            uzenet = $"Középpont: ({kurzoX}, {kurzoY})";
                            break;
                        case 5: 
                            fut = false;
                            break;
                    }
                    menuMod = false;
                }
                else
                {
                    Hiba("Hibás választás! Fel/Le, Enter vagy Esc kell.");
                }
            }
            else
            {
                bool nyil = false;      
                int ujX = kurzoX;
                int ujY = kurzoY;

                if (bill == ConsoleKey.LeftArrow) { ujX--; nyil = true; }
                else if (bill == ConsoleKey.RightArrow) { ujX++; nyil = true; }
                else if (bill == ConsoleKey.UpArrow) { ujY--; nyil = true; }
                else if (bill == ConsoleKey.DownArrow) { ujY++; nyil = true; }
                else if (bill == ConsoleKey.Spacebar)
                {
                    eszkozLeteve = !eszkozLeteve;
                    if (eszkozLeteve)
                    {
                        
                        rajzlap[kurzoX, kurzoY] = aktivKarakter;
                        szinek[kurzoX, kurzoY] = aktivSzin;
                    }
                }
                else if (bill == ConsoleKey.M)
                {
                    menuMod = true;
                    menuIndex = 0;
                }
                else if (bill == ConsoleKey.X)
                {
                    fut = false;
                }
                else if (bill >= ConsoleKey.F1 && bill <= ConsoleKey.F12)
                {
                    int index = bill - ConsoleKey.F1;
                    NyomdaBelyegzes(index);
                }

                if (nyil)
                {
                    if (ujX >= 0 && ujX < RajzlapSzelesseg && ujY >= 0 && ujY < RajzlapMagassag)
                    {
                        kurzoX = ujX;
                        kurzoY = ujY;
                        if (eszkozLeteve)
                        {
                            
                            rajzlap[kurzoX, kurzoY] = aktivKarakter;
                            szinek[kurzoX, kurzoY] = aktivSzin;
                        }
                    }
                    else
                    {
                        Hiba("A rajzlap szélén vagy, tovább nem lehet lépni!");
                    }
                }
            }
        }

        Console.ResetColor();
        Console.Clear();
        Console.CursorVisible = true;
        Console.WriteLine("A program véget ért.");
    }

    static void AlaphelyzetFeltoltes()
    {
        for (int y = 0; y < RajzlapMagassag; y++)
        {
            for (int x = 0; x < RajzlapSzelesseg; x++)
            {
                rajzlap[x, y] = ' ';
                szinek[x, y] = ConsoleColor.Gray;
            }
        }
    }

    static void RajzolFelulet(bool menuMod, int menuIndex, string[] menuPontok)
    {
        Console.SetCursorPosition(0, 0);

        
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("=== KONZOLOS RAJZOLÓ ===".PadRight(PanelSzel));

        if (menuMod)
        {
            Console.WriteLine(" [ MENÜ MÓD ]".PadRight(PanelSzel));
            for (int i = 0; i < menuPontok.Length; i++)
            {
                if (i == menuIndex)
                {
                    
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(("> " + menuPontok[i]).PadRight(PanelSzel));
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(("  " + menuPontok[i]).PadRight(PanelSzel));
                }
            }
            Console.WriteLine("".PadRight(PanelSzel));
            Console.WriteLine(" Fel/Le: választás".PadRight(PanelSzel));
            Console.WriteLine(" Enter: megnyitás".PadRight(PanelSzel));
            Console.WriteLine(" Esc: vissza a rajzhoz".PadRight(PanelSzel));
        }
        else
        {
            Console.WriteLine(" [ RAJZOLÓ MÓD ]".PadRight(PanelSzel));
            Console.WriteLine((" Pozíció: (" + kurzoX + ", " + kurzoY + ")").PadRight(PanelSzel));
            Console.WriteLine((" Karakter: " + aktivKarakter).PadRight(PanelSzel));

            Console.Write(" Szín: ");
            Console.ForegroundColor = aktivSzin;
            Console.WriteLine(SzinNev().PadRight(PanelSzel - 7));
            Console.ForegroundColor = ConsoleColor.White;

            string eszkoz = "felemelve";
            if (eszkozLeteve) eszkoz = "letéve";
            Console.WriteLine((" Eszköz: " + eszkoz).PadRight(PanelSzel));

            Console.WriteLine("".PadRight(PanelSzel));
            Console.WriteLine(" M: Menü".PadRight(PanelSzel));
            Console.WriteLine(" X: Kilépés".PadRight(PanelSzel));
            Console.WriteLine(" Szóköz: eszköz váltás".PadRight(PanelSzel));
            Console.WriteLine(" Nyilak: mozgás".PadRight(PanelSzel));
            Console.WriteLine(" F1-F12: nyomdák".PadRight(PanelSzel));
        }

        
        while (Console.CursorTop < 22)
        {
            Console.WriteLine("".PadRight(PanelSzel));
        }

        
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(RajzlapKezdoX - 1, RajzlapKezdoY - 1);
        Console.Write('┌' + new string('─', RajzlapSzelesseg) + '┐');

        for (int y = 0; y < RajzlapMagassag; y++)
        {
            Console.SetCursorPosition(RajzlapKezdoX - 1, RajzlapKezdoY + y);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write('│');
            for (int x = 0; x < RajzlapSzelesseg; x++)
            {
                Console.ForegroundColor = szinek[x, y];
                Console.Write(rajzlap[x, y]);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write('│');
        }

        Console.SetCursorPosition(RajzlapKezdoX - 1, RajzlapKezdoY + RajzlapMagassag);
        Console.Write('└' + new string('─', RajzlapSzelesseg) + '┘');

       
        UzenetKiir(uzenet, ConsoleColor.Red);
    }

    static string SzinNev()
    {
        for (int i = 0; i < szinLista.Length; i++)
        {
            if (szinLista[i] == aktivSzin) return szinNevek[i];
        }
        return "?";
    }

   

    static void Ir(int oszlop, int sor, string szoveg, ConsoleColor betuSzin)
    {
        Console.SetCursorPosition(oszlop, sor);
        Console.ForegroundColor = betuSzin;
        Console.Write(szoveg);
    }

    static void UzenetKiir(string szoveg, ConsoleColor betuSzin)
    {
        Ir(0, UzenetSor, szoveg.PadRight(85), betuSzin);
    }

    
    static void Hiba(string szoveg)
    {
        Hangeljes(300, 150);
        uzenet = szoveg;
        UzenetKiir(szoveg, ConsoleColor.Red);
    }

    

    static void KarakterValasztas()
    {
        UzenetKiir("Nyomd meg az új rajzkaraktert (Esc: mégse)", ConsoleColor.Gray);

        bool kesz = false;
        while (!kesz)
        {
            ConsoleKeyInfo b = Console.ReadKey(true);

            if (b.Key == ConsoleKey.Escape)
            {
                kesz = true;
            }
            else if (char.IsControl(b.KeyChar) || b.KeyChar == ' ')
            {
                Hiba("Ez nem használható rajzkarakternek, próbáld újra!");
            }
            else
            {
                aktivKarakter = b.KeyChar;
                kesz = true;
            }
        }
        uzenet = "";
    }

    static void SzinValasztas()
    {
        
        Ir(0, 12, " Szín (1-8):".PadRight(PanelSzel), ConsoleColor.Yellow);
        for (int i = 0; i < szinLista.Length; i++)
        {
            Ir(0, 13 + i, (" " + (i + 1) + " " + szinNevek[i]).PadRight(PanelSzel), szinLista[i]);
        }
        UzenetKiir("Nyomd meg a szín számát (1-8), Esc: mégse", ConsoleColor.Gray);

        bool kesz = false;
        while (!kesz)
        {
            ConsoleKeyInfo b = Console.ReadKey(true);

            if (b.Key == ConsoleKey.Escape)
            {
                kesz = true;
            }
            else if (b.KeyChar >= '1' && b.KeyChar <= '8')
            {
                aktivSzin = szinLista[b.KeyChar - '1'];   
                kesz = true;
            }
            else
            {
                Hiba("Hibás szín! Az 1-8 számok valamelyikét nyomd meg.");
            }
        }
        uzenet = "";
    }

    

    static void NyomdaBelyegzes(int index)
    {
        int tipus = index / 3;          
        int sugar = index % 3 + 1;      

        
        if (kurzoX - sugar < 0 || kurzoX + sugar >= RajzlapSzelesseg ||
            kurzoY - sugar < 0 || kurzoY + sugar >= RajzlapMagassag)
        {
            Hiba("A nyomda kilógna a rajzlapról, nem rajzoltam ki!");
            return;
        }

        for (int dy = -sugar; dy <= sugar; dy++)
        {
            for (int dx = -sugar; dx <= sugar; dx++)
            {
                bool irando = false;
                int ax = Math.Abs(dx);
                int ay = Math.Abs(dy);

                if (tipus == 0 && ax == ay) irando = true; 
                else if (tipus == 1 && (dx == 0 || dy == 0)) irando = true; 
                else if (tipus == 2 && (ax == sugar || ay == sugar)) irando = true; 
                else if (tipus == 3 && (ax + ay == sugar)) irando = true; 

                if (irando)
                {
                    rajzlap[kurzoX + dx, kurzoY + dy] = aktivKarakter;
                    szinek[kurzoX + dx, kurzoY + dy] = aktivSzin;
                }
            }
        }
    }

    

    static void MintaBetoltese()
    {
        bool kesz = false;
        while (!kesz)
        {
            string kerdes = "Fájlnév (üres Enter: mégse): ";
            UzenetKiir(kerdes, ConsoleColor.Gray);
            Console.SetCursorPosition(kerdes.Length, UzenetSor);
            Console.CursorVisible = true;
            string nev = Console.ReadLine();
            Console.CursorVisible = false;

            if (nev == null) nev = "";
            nev = nev.Trim();

            if (nev == "")
            {
                kesz = true;
            }
            else
            {
                string hiba = MintaBetolt(nev);
                if (hiba == "")
                {
                    kesz = true;
                }
                else
                {
                 
                    Hangeljes(300, 150);
                    UzenetKiir(hiba + " Enter: újra, Esc: mégse", ConsoleColor.Red);
                    ConsoleKeyInfo b = Console.ReadKey(true);
                    if (b.Key == ConsoleKey.Escape) kesz = true;
                }
            }
        }
        uzenet = "";
    }

    static string MintaBetolt(string nev)
    {
       
        string utvonal = nev;
        if (!File.Exists(utvonal))
        {
            utvonal = Path.Combine(AppContext.BaseDirectory, nev);
        }
        if (!File.Exists(utvonal))
        {
            return "A fájl nem található.";
        }

        string[] sorok;
        try
        {
            sorok = File.ReadAllLines(utvonal);
        }
        catch
        {
            return "A fájl nem olvasható.";
        }

        if (sorok.Length == 0)
        {
            return "A fájl üres.";
        }
        if (sorok.Length > RajzlapMagassag)
        {
            return "Túl sok sor (legfeljebb " + RajzlapMagassag + " lehet).";
        }
        for (int i = 0; i < sorok.Length; i++)
        {
            if (sorok[i].Length > RajzlapSzelesseg)
            {
                return "A " + (i + 1) + ". sor túl hosszú (legfeljebb " + RajzlapSzelesseg + " karakter).";
            }
        }

        AlaphelyzetFeltoltes();
        for (int y = 0; y < sorok.Length; y++)
        {
            for (int x = 0; x < sorok[y].Length; x++)
            {
                rajzlap[x, y] = sorok[y][x];
                szinek[x, y] = aktivSzin;
            }
        }
        return "";
    }

    static void Hangeljes(int frekvencia, int idotartam)
    {
        if (OperatingSystem.IsWindows())
        {
            try { Console.Beep(frekvencia, idotartam); } catch { }
        }
        else
        {
            Console.Write("\a");
        }
    }
}