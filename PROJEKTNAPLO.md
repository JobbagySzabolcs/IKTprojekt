# Projektnapló – Konzolos rajzoló

## Csapat és munkamegosztás

| Név vagy GitHub-felhasználónév | Vállalt feladatok és szerep | Saját menüfunkció |
| --- | --- | --- |
| Farkas Márton Kende | Képernyőfelosztás, menürendszer, kurzormozgatás, szabadkézi rajzolás, állapotjelző és hibakezelés (`Beep` + alsó üzenetsor), karakter- és színválasztás | Ugrás középre (`kurzoX = RajzlapSzelesseg / 2`, `kurzoY = RajzlapMagassag / 2`) |
| Jobbágy Szabolcs | Nyomdák (`F1–F12`: 3×3, 5×5, 7×7-es X, kereszt, négyzetkeret és rombuszkeret) határellenőrzéssel, szöveges mintabetöltés méret- és fájl-ellenőrzéssel | Cella törlése (aktuális kurzorpozícióban lévő karakter törlése / szóközre állítása) |

## Közös megállapodások

- A rajzlap mérete és helye: 60×20-as rajzlap (`RajzlapSzelesseg = 60`, `RajzlapMagassag = 20`), bal felső sarok `(0, 0)` a `(26, 2)` konzolpozíción (`RajzlapKezdoX = 26`, `RajzlapKezdoY = 2`), bal oldali menü-/állapotpanel 25 oszlop széles (`PanelSzel = 25`), üzenetsor a 24. sorban (`UzenetSor = 24`).
- A két munkarész kapcsolódási pontja: Közös `rajzlap[,]` (`char`) és `szinek[,]` (`ConsoleColor`) tömbök, valamint a kurzoX, kurzoY, aktivKarakter, aktivSzin, eszkozLeteve állapotváltozók és a közös Hiba / Hangeljes metódusok.
- A módosítások egyeztetése a main ágon: alap Altestfile.cs arról hogy kb hogyan kéne kinéznia a programnak, majd a közös Program.cs fájlt egymás után szerkesztjük egyeztetéssel majd commitoltunk amikor változtattunk magunk, vagy megbeszéltük a változtatásokat.

## Munkanapló

| Dátum | Ki | Mi készült el vagy miben állapodtunk meg? | Következő lépés vagy commit |
| --- | --- | --- | --- |
| 2026-11-12	| Farkas Márton Kende, Jobbágy Szabolcs | Átnéztük a specifikációt, megállapodtunk a 60×20-as rajzlap helyében (`RajzlapKezdoX = 26`, `RajzlapKezdoY = 2`), kiosztottuk a feladatokat és a két saját menüfunkciót. | Kezdőprojekt és naplósablon feltöltése a közös repóra. |
| 2026-11-17	| Farkas Márton Kende | Elkészült a bal oldali panel, a menü (`M`), a nyilas mozgatás, a `Szóköz` eszközváltás, a státuszjelző, a `Beep` hibajelzés, a karakter-/színválasztás és az *Ugrás középre* funkció. | Nyomdázás és fájlbetöltés bekötése a főciklusba. |
| 2026-11-20	| Jobbágy Szabolcs | Elkészült a `NyomdaBelyegzes` (`F1–F12`) befoglaló négyzet-ellenőrzéssel, a `MintaBetolt` / `MintaBetoltese` fájlméret- és olvashatóság-ellenőrzéssel, valamint a *Cella törlése* funkció. | Közös összekötés, UTF-8 beállítás (`Console.OutputEncoding = Encoding.UTF8`) és tesztelés. |
| 2026-11-23    | Jobbágy Szabolcs, Farkas Márton Kende | Összeillesztettük és teszteltük a teljes `Program.cs`-t, finomítottuk a visszajelző üzeneteket és a képernyőfrissítést. | Másik pár tesztelése és a repó (`Program.cs`, `minta.txt`, `PROJEKTNAPLO.md`, `README.md`) véglegesítése. |
| 2026-11-24    | Farkas Márton Kende | Teszteltem a programot, és javítottam a hibákat. | Véglegesítés és dokumentáció elkészítése. |

## Másik pár tesztje

- Tesztelt commit: a nov 23. commit, amely a teljes `Program.cs`-t tartalmazza.
- Visszajelzés és saját javítás: Minden működött, kívéve pár speciális karakter, ezért utf-8 beállítást kellett alkalmazni a konzol kimenetére (`Console.OutputEncoding = Encoding.UTF8`).
- Újrateszt eredménye: Minden működik amint a specifikációban le van írva, a hibajelzés és az üzenetsor is jól működik, a rajzlap és a panel frissítése is megfelelő.

## Otthoni AI-használat

Aki használt AI-t, írja le a nevét, miben kért segítséget, és mit tanult meg belőle.

- Név: Farkas Márton Kende
- Miben kértem segítséget: Gemini, A konzol-újrarajzolás, a menü és a rajzlap elválasztásának hibakeresésében, valamint a visszajelző üzenetek kialakításában.
- Mit tanultam meg: Hogy a `Console.SetCursorPosition` és a `PadRight` használatával hogyan lehet a panel és a rajzlap sorait egymásra írás nélkül frissíteni, valamint hogyan érdemes `ConsoleKeyInfo` és `char.IsControl` segítségével szűrni a bevitt karaktereket.

- Név: Jobbágy Szabolcs
- Miben kértem segítséget: ChatGPT, A `F1–F12` nyomdák alakzatképleteinek (`dx`, `dy`, `sugar`) és a `File.ReadAllLines` előtti méret- és hibakezelésének áttekintésében.
- Mit tanultam meg: Hogy a nyomda befoglaló négyzetét és a betöltendő fájl sor- és karakterszámát még a `rajzlap` tömb módosítása előtt kell ellenőrizni, így hiba esetén a korábbi rajz sértetlen marad.