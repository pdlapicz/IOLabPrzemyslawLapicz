//Autor: Przemyslaw Lapicz
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
/* ZADANIE 5
 * Napisz program, który pozwala na sumowanie liczb w znajdujących się w tablicy.
 * Rozmiar tablicy podany jest jako argument programu (dla uproszczenia można w ramach laboratorium zapisać ją w zmiennej). 
 * Liczby wybierane są losowo. Sumowanie odbywa się na przestrzeni wielu wątków. 
 * Liczba wątków zależy od wielkości fragmentu tablicy, która jest również podana jako argument programu. 
 * Wykorzystaj następujące narzędzia: AutoResetEvent, WaitHandle.WaitAll, lock, ThreadPool.QueueUserWorkItem, WaitCallback.
 * Poeksperymentuj z rozmiarem tablicy oraz jej przetwarzanego przez jeden wątek fragmentu. Wnioski oraz kod źródłowy zapisz.
 */
namespace lab1io
{
    class Program
    {
        public static int ilosc = 0;
        public static int suma = 0;

        static void ThreadProc(Object stateInfo)
        {
            ++ilosc;
            var wartosc = (int)stateInfo;
            Console.WriteLine("Watek " + ilosc + " zostal utworzony\nDodaje: " + wartosc);
            Console.WriteLine();
            suma += wartosc;
            Thread.Sleep(100);
        }

        static void Main(string[] args)
        {
            Random rand = new Random();
            int rozmiarTablicy;
            int index;
            string str;
            Console.WriteLine("Podaj rozmiar tablicy");
            str = Console.ReadLine();
            Int32.TryParse(str, out rozmiarTablicy);
            int[] tab = new int[rozmiarTablicy];
            for (int i = 0; i < rozmiarTablicy; i++)
            {
                tab[i] = rand.Next(-50, 50);
            }
            Console.WriteLine("\nZAWARTOSC TABLICY:\n");
            for (int i = 0; i < rozmiarTablicy; i++)
                Console.Write(tab[i] + " ");
            Console.WriteLine("\n");
            for(int i = 0; i < rozmiarTablicy; i++)
            {
                index = rand.Next(0, rozmiarTablicy - 1);
                ThreadPool.QueueUserWorkItem(ThreadProc, tab[index]);
            }
            Thread.Sleep(5000);
            Console.WriteLine("SUMA: " + suma);
            Console.ReadKey();
        }
    }

}
