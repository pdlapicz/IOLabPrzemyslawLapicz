//w klient server dostep do serwera maja dwoje klientow jednoczesnie

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab1io
{
    class Program
    {
        static void zad1()
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
            Thread.Sleep(2000);
        }

        /*Wadami takiego rozwiązania są:
         do serwera próbują się dostać 2 wątki klientów
         w tym samym czasie gdy klienci wykonuja kilka operacji
         np zmiana wartosci zmiennej w serwerze, wyswietlanie tekstu
         zmiana wlasciwosci, jeden klient zmieni ale jeszcze nie wyswietli
         potem drugi zmieni i pierwszy nei wyswietli swojej zmiany tylko tego drugiego*/
        static void zad2()
        {
            ThreadPool.QueueUserWorkItem(ThreadServer, "serwer");
            ThreadPool.QueueUserWorkItem(ThreadClient, "klient");
            ThreadPool.QueueUserWorkItem(ThreadClient2, "klient2");
            Thread.Sleep(5000);
        }

        static void zad3()
        {
            ThreadPool.QueueUserWorkItem(ThreadServer, "serwer");
            ThreadPool.QueueUserWorkItem(ThreadClient, "klient");
            ThreadPool.QueueUserWorkItem(ThreadClient2, "klient2");
            Thread.Sleep(5000);
        }

        static void zad4()
        {
            ThreadPool.QueueUserWorkItem(ThreadServer, "serwer");
            ThreadPool.QueueUserWorkItem(ThreadClient, "klient");
            ThreadPool.QueueUserWorkItem(ThreadClient2, "klient2");
            Thread.Sleep(5000);
        }

        static void zad5()
        {
            Random rand = new Random();
            int rozmiarTablicy;
            string str;
            Console.WriteLine("Podaj rozmiar tablicy");
            str = Console.ReadLine();
            Int32.TryParse(str, out rozmiarTablicy);
            int[] tab = new int[rozmiarTablicy];
            for(int i=0;i<rozmiarTablicy;i++)
            {
                tab[i] = rand.Next(-50, 50);
            }
            Console.WriteLine("\nZAWARTOSC TABLICY:\n");
            for (int i = 0; i < rozmiarTablicy; i++)
                Console.Write(tab[i] + " ");
            Console.WriteLine("\n");

        }

        //do1
        static void ThreadProc(Object stateInfo)
        {
            var czas = (int)stateInfo;
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

        //do zad1 
        static void ThreadProc2(Object stateInfo)
        {
             var czas = (int)stateInfo;
             Thread.Sleep(czas);
             Console.WriteLine("Watek2: Czekalem {0}", czas);
             
        }

        //do zad4
        static Object thisLock = new Object();
        /*PROBLEY I WNIOSKI DO ZAD 4
         

        */

        //do zad 2,3,4
        static void ThreadServer(Object stateInfo)
        {
            
            Console.ForegroundColor = ConsoleColor.Red;
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 2048);
            server.Start();
            while (true)
            {
                //do zad4
                lock (thisLock)
                {
                    string wiadomosc = Convert.ToString(stateInfo);
                    byte[] sender = new byte[1024];
                    sender = Encoding.ASCII.GetBytes(wiadomosc);
                    TcpClient client = server.AcceptTcpClient();
                    byte[] buffer = new byte[1024];
                    client.GetStream().Read(buffer, 0, 1024); //0 - w ktorym miejscu tablicy zapisywac dane
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[S]Otrzymalem: " + Encoding.UTF8.GetString(buffer));
                    Console.ResetColor();
                    client.GetStream().Write(sender, 0, sender.Length);
                    client.Close();
                }
            }
            Thread.Sleep(500);
        }

        //do zad 2,3,4
        static void ThreadClient(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            //NetworkStream stream = client.GetStream();
            byte[] message = new byte[1024];
            byte[] buffer = new byte[1024];
            var mes = (string)stateInfo;
            message = new ASCIIEncoding().GetBytes(mes);
            // stream.Read(message, 0, message.Length);
            client.GetStream().Write(message, 0, message.Length);
            client.GetStream().Read(buffer, 0, 1024);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[K1]Otrzymalem: " + Encoding.UTF8.GetString(buffer));
            Console.ResetColor();
        }

        //do zad 2,3,4
        static void ThreadClient2(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            //NetworkStream stream = client.GetStream();
            byte[] message = new byte[1024];
            byte[] buffer = new byte[1024];
            var mes = (string)stateInfo;
            message = new ASCIIEncoding().GetBytes(mes);
            // stream.Read(message, 0, message.Length);
            client.GetStream().Write(message, 0, message.Length);
            client.GetStream().Read(buffer, 0, 1024);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[K2]Otrzymalem: " + Encoding.UTF8.GetString(buffer));
            Console.ResetColor();
        }

        static void ThreadSuma(Object stateInfo)
        {

        }

        static void Main(string[] args)
        {
            //zad1
            //zad1();

            //zad2
            //zad2();

            //zad3
            //zad3();

            //zad4
            //zad4();

            //zad5
            zad5();
            Console.ReadKey();
        }
    }

}
