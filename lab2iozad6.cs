//Autor: Przemyslaw Lapicz
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*ZADANIE 6
Zaimplementuj w trybie Callback narzędzie czytające dane z pliku.Callback powinien być odpowiedzialny za zamykanie strumienia oraz wypisywanie wiadomości na ekranie.Podpowiedź: Użyj następujących narzędzi: FileStream, BeginRead, AsyncCallback, WaitHandle, AutoResetEvent, WaitOne.
Czy wątek główny czeka na zakończenie operacji czytania? Czy wątek główny czeka na zakończenie metody callback?*/

namespace lab2io
{
    //watek glowny czeka na zamonczenie metody callback ale nie czeka na zakonczenie operacji czytania, moze w tym czasie robic cos innego
    class Program
    {
        static void myAsyncCallback(IAsyncResult state)
        {
            object[] obj = (object[])state.AsyncState;
            byte[] buffer = (byte[])obj[1];
            FileStream fs = (FileStream)obj[0];
            fs.EndRead(state);
            Console.Write(Encoding.ASCII.GetString(buffer));
            fs.Close();
        }

        static void Main(string[] args)
        {
            byte[] buffer = new byte[1024];
            FileStream fs = new FileStream("test.txt", FileMode.Open, FileAccess.Read);
            AsyncCallback myAsync = new AsyncCallback(myAsyncCallback);
            fs.BeginRead(buffer, 0, buffer.Length, myAsync, new object[] { fs, buffer });
            Console.ReadKey();
        }
    }
}
