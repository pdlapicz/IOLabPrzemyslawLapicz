//Autor: Przemyslaw Lapicz
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*ZADANIE 7
Zaimplementuj w trybie EndInvoke narzędzie czytające dane z pliku. 
Tym razem wątek główny powinien być odpowiedzialny za zamykanie strumienia 
oraz wypisywanie wiadomości na ekranie. Podpowiedź: nie potrzebujesz metody callback, 
w jej miejsce możesz podstawić null.
Czy wątek główny czeka na zakończenie operacji czytania? W porównaniu do czytania 
synchronicznego, co zyskujesz dzięki takiemu rozwiązaniu?*/
namespace lab2iozad7
{
 /*W porownaniu do czytania synchronicznego ta metoda pozwala czytac i w trakcie czytania z pliku mozna wykonywac inne operacje
   np obliczenia. W synchronicznym czytaniu program musi czekac az zakonczy czytac plik, nie moze nic innego zrobic*/
    class Program
    {
        static void Main(string[] args)
        {
            byte[] buffer = new byte[1024];
            FileStream fs = new FileStream("test.txt", FileMode.Open, FileAccess.Read);
            IAsyncResult result = fs.BeginRead(buffer, 0, buffer.Length, null, null);
            //w trakcie czytania mozna wykonywac inne operacje miedzy Begin a End
            fs.EndRead(result);
            fs.Close();
            Console.Write(Encoding.ASCII.GetString(buffer));
        }
    }
}
