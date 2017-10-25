//Autor: Przemyslaw Lapicz
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
/*ZADANIE 1
 * Napisz program, który doda dwa zadania do puli wątków.
 * Każde z zadań po czasie przekazanym w parametrze (stateInfo) 
 * wypisze na konsoli wiadomość, która zawiera informacje o tym, ile poczekało.
 * Wykorzystaj operację uśpienia wątku głównego. Wykorzystaj ThreadPool.
 */

namespace lab1iozad1
{
    class Program
    {
        static void ThreadProc(Object stateInfo)
        {
            var czas = (int)stateInfo; //ze stateInfo pobierana jest wartosc ktora zostala 
                                       //przypisana do czas ThreadPool.QueueUserWorkItem(ThreadProc, 600);
            Thread.Sleep(czas);
            Console.WriteLine("Watek1: Czekalem {0}", czas);
            /* //4. Rozpoczecie wykonywania nowego wątku
             Console.WriteLine("hello");
             var message = ((object[])stateInfo)[0];
             var integer = ((object[])stateInfo)[1];
             var character = ((object[])stateInfo)[2];
             Console.WriteLine(character.GetType());
             Console.WriteLine(message);*/
        }
        
        static void ThreadProc2(Object stateInfo)
        {
            var czas = (int)stateInfo;
            Thread.Sleep(czas);
            Console.WriteLine("Watek2: Czekalem {0}", czas);

        }

        static void Main(string[] args)
        {
            /*
             //1. Watek glowny
             //2. Dodanie do kolejki obslugujacej watki nowej pozycji
             ThreadPool.QueueUserWorkItem(ThreadProc);
             //3. Kolejka obslugujaca watki uruchamia dodany watek
             //5. Watek glowny czeka na zakonczenie dodanego watku po czym zamyka program
             Thread.Sleep(1000);
             ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { "wiadomosc 1", 2, 'a' });
             Thread.Sleep(1000);
             */
            ThreadPool.QueueUserWorkItem(ThreadProc, 600);
            ThreadPool.QueueUserWorkItem(ThreadProc2, 1000);
            Thread.Sleep(2000); //uspienie watku glownego wymagane zeby dodane watki mogly sie wykonac
            Console.ReadKey();
        }
    }

}
