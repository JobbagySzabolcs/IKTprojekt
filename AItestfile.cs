using System;
using System.IO;
using System.Text;

class Program
{
    // Rajzlap méretei és pozíciója (hogy ne lógjanak egymásba)
    const int RajzlapSzelesseg = 40;
    const int RajzlapMagassag = 20;
    const int RajzlapKezdoX = 38; // Megnövelt margó a bal oldali panel miatt
    const int RajzlapKezdoY = 2;

    static char[,] rajzlap = new char[RajzlapSzelesseg, RajzlapMagassag];
    static ConsoleColor[,] szinek = new ConsoleColor[RajzlapSzelesseg, RajzlapMagassag];

    static int kurzoX = 0;
    static int kurzoY = 0;
    static char aktivKarakter = '#';
    static ConsoleColor aktivSzin = ConsoleColor.Cyan;
    static bool eszkozLeteve = false;

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        if (Console.IsInputRedirected || Console.IsOutputRedirected)
        {
            Console.WriteLine("A futtatáshoz nyiss interaktív konzolablakot.");
            return;
        }

        if (Console.WindowWidth < 85 || Console.WindowHeight < 25)
        {
            try
            {
                Console.SetWindowSize(95, 25);
                Console.SetBufferSize(95, 25);
            }
            catch
            {
                Console.WriteLine("Nagyítsd a konzolablakot legalább 95x25-ösre.");
                return;
            }
        }

        AlaphelyzetFeltoltes();
        Console.CursorVisible = false;
        Console.Clear();

        bool fut = true;
        bool menuMod = false;
        int menuIndex = 0;
        string[] menuPontok = {
            "Karakter választása (#, *, O, +)",
            "Szín választása (Cián, Zöld, Sárga, Fehér)",
            "Minta betöltése (minta.txt)",
            "Cella törlése",
            "Ugrás középre",
            "Kilépés"
        };

        while (fut)
        {
            if (!menuMod && eszkozLeteve)
            {
                rajzlap[kurzoX, kurzoY] = aktivKarakter;
                szinek[kurzoX, kurzoY] = aktivSzin;
            }

            RajzolFelulet(menuMod, menuIndex, menuPontok);

            if (!menuMod)
            {
                Console.SetCursorPosition(RajzlapKezdoX + kurzoX, RajzlapKezdoY + kurzoY);
                Console.CursorVisible = true;
            }
            else
            {
                Console.CursorVisible = false;
            }

            ConsoleKeyInfo billInfo = Console.ReadKey(true);
            ConsoleKey bill = billInfo.Key;

            if (menuMod)
            {
                if (bill == ConsoleKey.UpArrow)
                {
                    menuIndex = (menuIndex - 1 + menuPontok.Length) % menuPontok.Length;
                    Hangeljes(500, 50);
                }
                else if (bill == ConsoleKey.DownArrow)
                {
                    menuIndex = (menuIndex + 1) % menuPontok.Length;
                    Hangeljes(500, 50);
                }
                else if (bill == ConsoleKey.Escape)
                {
                    menuMod = false;
                }
                else if (bill == ConsoleKey.Enter)
                {
                    switch (menuIndex)
                    {
                        case 0: // Karakter választás
                            aktivKarakter = aktivKarakter == '#' ? '*' : (aktivKarakter == '*' ? 'O' : (aktivKarakter == 'O' ? '+' : '#'));
                            break;
                        case 1: // Szín választás
                            aktivSzin = aktivSzin == ConsoleColor.Cyan ? ConsoleColor.Green : (aktivSzin == ConsoleColor.Green ? ConsoleColor.Yellow : (aktivSzin == ConsoleColor.Yellow ? ConsoleColor.White : ConsoleColor.Cyan));
                            break;
                        case 2: // Minta betöltése
                            MintaBetoltese();
                            break;
                        case 3: // Saját funkció 1: Cella törlése
                            rajzlap[kurzoX, kurzoY] = ' ';
                            szinek[kurzoX, kurzoY] = ConsoleColor.Gray;
                            break;
                        case 4: // Saját funkció 2: Ugrás középre
                            kurzoX = RajzlapSzelesseg / 2;
                            kurzoY = RajzlapMagassag / 2;
                            break;
                        case 5: // Kilépés
                            fut = false;
                            break;
                    }
                    menuMod = false;
                }
            }
            else
            {
                int ujX = kurzoX;
                int ujY = kurzoY;

                if (bill == ConsoleKey.LeftArrow) ujX--;
                else if (bill == ConsoleKey.RightArrow) ujX++;
                else if (bill == ConsoleKey.UpArrow) ujY--;
                else if (bill == ConsoleKey.DownArrow) ujY++;
                else if (bill == ConsoleKey.Spacebar)
                {
                    eszkozLeteve = !eszkozLeteve;
                }
                else if (bill == ConsoleKey.M)
                {
                    menuMod = true;
                    menuIndex = 0;
                }
                else if (bill == ConsoleKey.Escape || bill == ConsoleKey.X)
                {
                    fut = false;
                }
                else if (bill >= ConsoleKey.F1 && bill <= ConsoleKey.F12)
                {
                    int index = bill - ConsoleKey.F1;
                    NyomdaBelyegzes(index);
                }

                if (ujX >= 0 && ujX < RajzlapSzelesseg && ujY >= 0 && ujY < RajzlapMagassag)
                {
                    kurzoX = ujX;
                    kurzoY = ujY;
                }
                else if (bill == ConsoleKey.LeftArrow || bill == ConsoleKey.RightArrow || bill == ConsoleKey.UpArrow || bill == ConsoleKey.DownArrow)
                {
                    Hangeljes(300, 100);
                }
            }
        }

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

        // Bal oldali panel törlése / frissítése soronként (szélesség: 35 karakter)
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("=== KONZOLOS RAJZOLÓ ===");

        if (menuMod)
        {
            Console.WriteLine(" [ MENÜ MÓD ]                        ");
            for (int i = 0; i < menuPontok.Length; i++)
            {
                if (i == menuIndex)
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"> {menuPontok[i],-32} ");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {menuPontok[i],-32} ");
                }
            }
            for (int i = menuPontok.Length; i < 8; i++) Console.WriteLine(new string(' ', 35));
        }
        else
        {
            Console.WriteLine(" [ RAJZOLÓ MÓD ]                     ");
            Console.WriteLine($" Pozíció: ({kurzoX,2}, {kurzoY,2})                 ");
            Console.WriteLine($" Karakter: {aktivKarakter}                       ");
            Console.WriteLine($" Szín: {aktivSzin,-10}                   ");
            Console.WriteLine($" Eszköz: {(eszkozLeteve ? "Letéve" : "Felemelve"),-10}              ");
            Console.WriteLine("                                     ");
            Console.WriteLine(" M: Menü / Opciók                    ");
            Console.WriteLine(" Szóköz: Eszköz váltás               ");
            Console.WriteLine(" Nyilak: Mozgás                      ");
            Console.WriteLine(" F1-F12: Nyomdák                     ");
            Console.WriteLine(" Esc / X: Kilépés                    ");
            for (int i = 11; i < 15; i++) Console.WriteLine(new string(' ', 35));
        }

        // Jobb oldali rajzlap kerete
        Console.SetCursorPosition(RajzlapKezdoX - 1, RajzlapKezdoY - 1);
        Console.Write('┌' + new string('─', RajzlapSzelesseg) + '┐');

        for (int y = 0; y < RajzlapMagassag; y++)
        {
            Console.SetCursorPosition(RajzlapKezdoX - 1, RajzlapKezdoY + y);
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
    }

    static void NyomdaBelyegzes(int index)
    {
        int meret = (index % 3 == 0) ? 3 : ((index % 3 == 1) ? 5 : 7);
        int tipus = index / 3;
        int sugar = meret / 2;

        if (kurzoX - sugar < 0 || kurzoX + sugar >= RajzlapSzelesseg ||
            kurzoY - sugar < 0 || kurzoY + sugar >= RajzlapMagassag)
        {
            Hangeljes(300, 100);
            return;
        }

        for (int dy = -sugar; dy <= sugar; dy++)
        {
            for (int dx = -sugar; dx <= sugar; dx++)
            {
                bool irando = false;
                int ax = Math.Abs(dx);
                int ay = Math.Abs(dy);

                if (tipus == 0 && ax == ay) irando = true; // X alakzat
                else if (tipus == 1 && (dx == 0 || dy == 0)) irando = true; // Kereszt
                else if (tipus == 2 && (ax == sugar || ay == sugar)) irando = true; // Négyzet
                else if (tipus == 3 && (ax + ay == sugar)) irando = true; // Rombusz

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
        string utvonal = Path.Combine(AppContext.BaseDirectory, "minta.txt");
        if (File.Exists(utvonal))
        {
            try
            {
                string[] sorok = File.ReadAllLines(utvonal);
                if (sorok.Length <= RajzlapMagassag)
                {
                    bool elfer = true;
                    foreach (string s in sorok)
                    {
                        if (s.Length > RajzlapSzelesseg) elfer = false;
                    }

                    if (elfer)
                    {
                        AlaphelyzetFeltoltes();
                        for (int y = 0; y < sorok.Length; y++)
                        {
                            for (int x = 0; x < sorok[y].Length; x++)
                            {
                                rajzlap[x, y] = sorok[y][x];
                                szinek[x, y] = aktivSzin;
                            }
                        }
                        return;
                    }
                }
            }
            catch { }
        }
        Hangeljes(300, 150);
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