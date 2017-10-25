//Autor: Przemyslaw Lapicz
using System;
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
        static void ThreadProc(Object stateInfo)
        {
            var index = (int)stateInfo;
            Thread.Sleep(100);
        }

        static void ThreadSuma(Object stateInfo)
        {

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


            Console.ReadKey();
        }
    }

}
