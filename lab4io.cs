using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace lab4io
{
    class Program
    {//ZADANIA DODATKOWE Z PREZENTACJI ZROBIC < SLAJD 87 >
        //MUSZA BYC WSZYSTKIE PROGRAMY DO OCENY :(
        #region Zadanie 4-8
        class Server
        {
            #region Variables
            TcpListener server;
            int port;
            IPAddress address;
            bool running = false;
            CancellationTokenSource cts = new CancellationTokenSource();
            Task serverTask;
            public Task ServerTask {
                get { return serverTask; }
            }
            #endregion
            #region Properties
            public IPAddress Address {
                get { return address; }
                set {
                    if (!running) address = value;
                    else;
                }
            }
            public int Port {
                get { return port; }
                set {
                    if (!running)
                        port = value;
                    else;
                }
            }
            #endregion
            #region Constructors
            public Server()
            {
                Address = IPAddress.Any;
                port = 2048;
            }
            public Server(int port)
            {
                this.port = port;
            }
            public Server(IPAddress address)
            {
                this.address = address;
            }
            #endregion
            #region Methods

            public async Task RunAsync(CancellationToken ct)
            {

                server = new TcpListener(address, port);

                try
                {
                    server.Start();
                    Console.WriteLine("Serwer utworzony");
                    running = true;
                }
                catch (SocketException ex)
                {
                    throw (ex);
                }
                while (true && !ct.IsCancellationRequested)
                {

                    TcpClient client = await server.AcceptTcpClientAsync();
                    byte[] buffer = new byte[1024];
                    using (ct.Register(() => client.GetStream().Close()))
                    {
                        client.GetStream().ReadAsync(buffer, 0, buffer.Length, ct).ContinueWith(
                            async (t) =>
                            {
                                int i = t.Result;
                                while (true)
                                {
                                    client.GetStream().WriteAsync(buffer, 0, i, ct);
                                    try
                                    {
                                        i = await client.GetStream().ReadAsync(buffer, 0, buffer.Length, ct);
                                    }
                                    catch
                                    {
                                        break;
                                    }
                                }
                            });
                    }
                }

            }

            public void RequestCancellation()
            {
                cts.Cancel();
                //serverTask.Wait();
                //serverTask.Dispose();
                server.Stop();
            }
            public void Run()
            {
                serverTask = RunAsync(cts.Token);
            }
            public void StopRunning()
            {
                RequestCancellation();
                //serverTask.Dispose();
            }
            #endregion
        }

        class Client
        {
            string adresIP;
            int port;
            TcpClient client;

            public string AdresIP { get => adresIP; set => adresIP = value; }
            public int Port { get => port; set => port = value; }

            public void polaczenie()
            {
                client = new TcpClient();
                client.Connect("127.0.0.1", 2048);
            }

            public async Task<string> ping(string wiadomosc)
            {
                byte[] buffer = new ASCIIEncoding().GetBytes(wiadomosc);
                client.GetStream().WriteAsync(buffer, 0, buffer.Length);
                buffer = new byte[1024];
                var t = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, t);
            }

            public async Task<IEnumerable<string>> keepPinging(string wiadomosc, CancellationToken token)
            {
                List<string> wiadomosci = new List<string>();
                bool skonczone = false;
                while (!skonczone)
                {
                    if (token.IsCancellationRequested)
                        skonczone = true;
                    wiadomosci.Add(await ping(wiadomosc));//!!!!!!!!!!!!!!
                }
                return wiadomosci;
            }
        }
        #endregion
        static void Main(string[] args)
        {
            Server s = new Server();
            s.Run();
            Client c1 = new Client();
            Client c2 = new Client();
            Client c3 = new Client();
            CancellationTokenSource ct1 = new CancellationTokenSource();
            CancellationTokenSource ct2 = new CancellationTokenSource();
            CancellationTokenSource ct3 = new CancellationTokenSource();
            var client1T = c1.keepPinging("message", ct1.Token);
            var client2T = c2.keepPinging("message", ct2.Token);
            var client3T = c3.keepPinging("message", ct3.Token);
            ct1.CancelAfter(2000);
            ct2.CancelAfter(3000);
            ct3.CancelAfter(4000);
            Task.WaitAll(new Task[] { client1T, client2T, client3T,s.ServerTask }); //dodano do tego s.SeverTask aby poczekal na zakonczenie pracy serwera
            s.StopRunning();
            //klase klienta zmniejszyc i zaimplementowac samemu
        }
    }
}
