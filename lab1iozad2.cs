//Autor: Przemyslaw Lapicz
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
/*ZADANIE 2
 Napisz prosty program realizujący zadanie klient-serwer. 
 Metody obsługujące klienta i serwer umieść w osobnych wątkach. 
 Dodaj jeden serwer oraz dwóch klientów do puli wątków. 
 Do analizy swojego rozwiązania użyj trybu debugowania.
 Zdefiniuj problemy oraz błędy takiego rozwiązania. 
 Kod źródłowy oraz wnioski zapisz w swoich notatkach.*/
namespace lab1iozad2
{
    class Program
    {
        /*Wadami takiego rozwiązania są:
         do serwera próbują się dostać 2 wątki klientów
         serwer sie na to zgadza w tej sytuacji
         w tym samym czasie gdy klienci wykonuja kilka operacji
         np zmiana wartosci zmiennej w serwerze, wyswietlanie tekstu
         zmiana wlasciwosci, jeden klient zmieni ale jeszcze nie wyswietli
         potem drugi zmieni i pierwszy nei wyswietli swojej zmiany tylko tego drugiego*/
        static void ThreadServer(Object stateInfo)
        {
            string show;    //zmienna do wyswietlenia wiadomosci serwera (co odebral) na konsoli
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 2048);   //tworzenie obiektu serwer
            server.Start();     //uruchomienie
            while (true)
            {               
                string mess = Convert.ToString(stateInfo);      //argument watku - wiadomosc serwera
                TcpClient client = server.AcceptTcpClient();    //przyjecie klienta - nawiazanie polaczenia
                byte[] sender = new byte[1024];         //zmienna do konwersji typow - byte do wyslania przez serwer do klientow
                sender = Encoding.ASCII.GetBytes(mess);     //konwersja
                byte[] buffer = new byte[1024];         //zmienna do odebrania wiadomosci (np. od klienta)
                client.GetStream().Read(buffer, 0, 1024);   //0 - w ktorym miejscu tablicy zapisywac dane
                show = "[S]Otrzymalem: " + Encoding.UTF8.GetString(buffer);     //przypisanie wiadomosci do zmiennej string
                Console.WriteLine(show);    //wyswietlenie na konsoli
                client.GetStream().Write(sender, 0, sender.Length);  //wyslanie wiadomosci serwera (argumentu watku) do polaczonego klienta
                client.Close();         //zamkniecie polaczenia
                Thread.Sleep(1000);     //uspienie watku
            }
        }

        static void ThreadClient(Object stateInfo)
        {
            string show; //zmienna pomocnicza
            TcpClient client = new TcpClient();  //utworzneie obiektu klienta
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048)); //polaczenie klienta z serwerem lokalnym
            byte[] message = new byte[1024];  //zmienna do wiadomosci wysylanej
            byte[] buffer = new byte[1024];   //zmienna do wiadomosci odebranej
            var mes = (string)stateInfo;  //pobranie wartosci z argumentu watku - wiadomosci klienta
            message = new ASCIIEncoding().GetBytes(mes);  //zapis w tablicy bajtow
            client.GetStream().Write(message, 0, message.Length);  //wysylanie wiadomosci
            client.GetStream().Read(buffer, 0, 1024);  //odebranie wiadomosci
            show = "[K1]Otrzymalem: " + Encoding.UTF8.GetString(buffer);
            Console.WriteLine(show);
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
            Console.WriteLine(show);
            Thread.Sleep(500);
        }

        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(ThreadServer, "serwer");
            ThreadPool.QueueUserWorkItem(ThreadClient, "klient1");
            ThreadPool.QueueUserWorkItem(ThreadClient2, "klient2");
            Thread.Sleep(5000);
        }
    }

}
