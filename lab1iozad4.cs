//Autor: Przemyslaw Lapicz
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
/* ZADANIE 4
Popraw działanie programu z Zadania 3 używając wyrażenia „lock” 
(poszukaj sposobu wykorzystania takiej metody w dokumentacji MSDN; statement lock).
Jaki problem próbujemy rozwiązać? Jakie są problemy związane z tym rozwiązaniem. 
Wnioski oraz kod źródłowy umieść w swoich notatkach.
 */
namespace lab1iozad4
{
    class Program
    {
        /*PROBLEMY I WNIOSKI DO ZAD 4
         Do serwera ma dostep w jednej chwili tylko jeden klient, nie ma mozliwosci
         komunikacji serwera z dwoma klientami jednoczesnie. Gdy zostanie ustanowione lock
         po stronie serwera po nawiazaniu polaczenia z klientem to dopoki nie zakonczy polaczenia
         bedzie w lock. Nie ma mozliwosci zmiany w trakcie, np. wywlaszczenia
        */
        static void ThreadServer(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 2048); //utworzenie serwera 
            server.Start(); //uruchomienie serwera
            while (true)
            {
                    string mess = Convert.ToString(stateInfo); //argument do watku - wiadomosc serwera
                    TcpClient client = server.AcceptTcpClient();  //ustanowienie polaczenia z klientem
                lock (client)
                {
                    ThreadPool.QueueUserWorkItem(ThreadConnect, new object[] { client, mess }); //utworzenie watku polaczenia
                }
            }
        }

        static void ThreadClient(Object stateInfo)
        {
            string show; //zmienna pomocnicza
            TcpClient client = new TcpClient();  //utworzenie obiektu klienta
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048)); //polaczenie klienta z serwerem lokalnym
            byte[] message = new byte[1024]; //zmienna do wiadomosci wysylanej
            byte[] buffer = new byte[1024];  //zmienna do wiadomosci odebranej
            var mes = (string)stateInfo; //pobranie wartosci z argumentu watku - wiadomosci klienta
            message = new ASCIIEncoding().GetBytes(mes); //zapis w tablicy bajtow
            client.GetStream().Write(message, 0, message.Length); //wyslanie wiadomosci
            client.GetStream().Read(buffer, 0, 1024);  //odebranie wiadomosci
            show = "[K1]Otrzymalem: " + Encoding.UTF8.GetString(buffer);
            writeConsoleMessage(show, ConsoleColor.Green);  //wyswietlenie komuniaktu na konsoli w kolorze zielonym
            Thread.Sleep(500);
        }

        static void ThreadClient2(Object stateInfo)
        {
            string show;
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
            show = "[K2]Otrzymalem: " + Encoding.UTF8.GetString(buffer);
            writeConsoleMessage(show, ConsoleColor.Green);
            Thread.Sleep(500);
        }

        static void ThreadConnect(Object stateInfo)
        {
            string show; //zmienna pomocnicza
            var varClient = ((object[])stateInfo)[0];   //pobranie obiektu klienta
            var varMessage = ((object[])stateInfo)[1];  //pobrane wiadomosci od serwera
            TcpClient client = (TcpClient)varClient;    //utworzenie obiektu klienta w watku
            string message = (string)varMessage;        //konwersja wiadomosci
            Console.WriteLine("Nowe polaczenie");       //wiadomosc informacyjna w konsoli
            byte[] sender = new byte[1024];             //zmienna do wyslania wiadomosci
            sender = Encoding.ASCII.GetBytes(message);  //zapis w tablicy bajtow
            byte[] buffer = new byte[1024];             //zmienna do odebrania wiadomosci
            client.GetStream().Read(buffer, 0, 1024);   //0 - w ktorym miejscu tablicy zapisywac dane, pobieranie wiadomosci <czytanie>
            show = "[S]Otrzymalem: " + Encoding.UTF8.GetString(buffer);     //utworzenie calego komunikatu do konsoli
            writeConsoleMessage(show, ConsoleColor.Red);    //wyswietlenie na konsoli wiadomosci w kolorze czerwonym
            client.GetStream().Write(sender, 0, sender.Length); //wyslanie wiadomosci od serwera do klienta
            client.Close();                             //zamkniecie polaczenia
            Thread.Sleep(1000);
        }

        //metoda do zmiany koloru wyswietlanej wiadomosci w konsoli
        static void writeConsoleMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;  //przypisanie koloru z argumentu metody
            Console.WriteLine(message);       //wyswietlenie na konsoli wiadomosci argumentu metody
            Console.ResetColor();             //przywrocenie koloru (bialy)
        }

        static Object thisLock = new Object();
        
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(ThreadServer, "serwer");
            ThreadPool.QueueUserWorkItem(ThreadClient, "klient1");
            ThreadPool.QueueUserWorkItem(ThreadClient2, "klient2");
            Thread.Sleep(5000);
            Console.ReadKey();
        }
    }

}
