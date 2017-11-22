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

namespace lab3io
{
    /*wyrazenie lambda
    (   ) => {  }
    x => x+1
    int f(x)
    {
     return x+1;
    }
    int f2()      () =>6
    {
     return 6
    }*/

    //DO ZROBIENIA ZAD 12, 
    class Program
    {
        delegate int myDelegateType(int i, int j);
        static void zad1() //wyrazenie lambda do sumy 2 el
        {
            myDelegateType foo = (i, j) => { return i + j; };

            Console.WriteLine("wynik: " + foo(5, 2));
        }

        //dzieki async w Tasku mozemy wykonywac metody takie jak await
        //podstawowe wykorzystanie taska
        public static async Task OperationTask(object data)
        {
            Console.WriteLine("begin task");
            await Task.Run(() =>
            {
                Console.WriteLine("begin async");
                Thread.Sleep(2000);
                //kod operacji asynchronicznej
                Console.WriteLine("end async");
            });
            Console.WriteLine("end task");
        }

        //task do czytania pliku
        public static async Task OperationTask2(byte[] buffer)
        {
            //TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            FileStream fs = new FileStream("test.txt", FileMode.Open);
            Console.WriteLine("task before await");
            await fs.ReadAsync(buffer, 0, 128);
            Console.WriteLine("task after await");
        }

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //od prowadzacego
        //zad12

        //---------------
        /*1.    Błąd  „public static Task<int> OperationTask(byte[] buffer, out int i)” 
         * został zamieniony na „public Task<TResultDataStructure> OperationTask(byte[] buffer)”. 
         * Zaimplementuj strukturę „TResultDataStructure”, 
         * która spełnia założenia błędnego zapisu (użyj Properties).*/
        #region Zadanie 1
        public struct TResultDataStructure
        {
            int i, j;
            public TResultDataStructure(int gi, int gj) 
            {
                i = gi;
                j = gj;
            }
            //uwaga! tylko w visual studio
            //enkapsulacja- ctrl+r, potem dalej trzymac ctrl, puscic r i kliknac e
         /*   public int I
            {
                get => i; set => i = value; }
            }
            public int J
            {
                get => j; set => j = value; }
            }*/
        }

        public Task<TResultDataStructure> AsyncMethod1()
        {
            TaskCompletionSource<TResultDataStructure> tcs = new TaskCompletionSource<TResultDataStructure>();
            Task.Run(() =>
            {
                tcs.SetResult(new TResultDataStructure());
            });
            return tcs.Task;
        }

        public Task<TResultDataStructure> AsyncMethod1(byte[] buffer)
        {
            TaskCompletionSource<TResultDataStructure> tcs = new TaskCompletionSource<TResultDataStructure>();
            Task.Run(() =>
            {
                tcs.SetResult(new TResultDataStructure(/*fragment zadania 1*/));
            });
            return tcs.Task;
        }

        public TResultDataStructure Zadanie1()
        {
            var task = AsyncMethod1(null);
            task.Wait();
            return task.GetAwaiter().GetResult();
        }
        #endregion
        /*2.	Popraw poniżej przedstawiony kod:*/
        #region Zadanie 2
        private bool zadanie2 = false;
        public bool Z2
        {
            get { return zadanie2; }
            set { zadanie2 = value; }
        }
        public void Zadanie2()
        {
        //ZADANIE 2. ODKOMENTUJ I POPRAW  
        //foo = (i, j) => { return i + j; };
            Task.Run(() => { Z2 = true; });
        }
        #endregion
        /*3.	Zaimplementuj metodę oczekującą (asynchronicznie) na pobranie informacji ze 
         * strony podanej w argumencie i zwracającej pobraną zawartość w postaci ciągu znaków. 
         * Do tego celu wykorzystaj zaimplementowaną w ramach klasy WebClient asynchroniczną metodę DownloadStringAsyncTask(Uri)
         *  Przetestuj działanie metody na adresie „http://www.feedforall.com/sample.xml”.
        */
        #region Zadanie 3
        public async Task<XmlDocument> Zadanie3(string address)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.DownloadStringTaskAsync(new Uri("http://www.feedforall.com/sample.xml"));
            return new XmlDocument();
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string text = e.Result;
            // … do something with result
        }
        #endregion
        /*5.	Zaimplementuj clientTask zgodnie ze wzorcem TAP. 
         Posłuż się w implementacji kodem źródłowym dostarczonym przez prowadzącego. 
         Posługując się metodą Task.WaitAll uruchom jedno zadanie serwera i kilka zadań klienta.
         6.	    Dodaj opcję anulowania zadania KeepPinging oraz RunAsync. 
         Wykorzystaj CancellationToken (w pierwszym wypadku musisz samodzielnie 
         zaimplementować obsługę tokenu, w drugim przypadku konsumujesz implementację ReadAsync i tam wykorzystujesz token)
         7.	    Zaimplementuj klasę obsługującą tzw. Log Serwera. 
         Wiadomości mogą być przepisywane do podanego w argumencie wyjścia, 
         zapisywane bezpośrednio na konsoli lub zapisywane w pliku. (Zadanie polega na synchronizacji zapisywanych danych)
         8.	    Stwórz program czytający bitmapy z plików i wyświetlający wczytane obrazy na ekranie(obsługa kontrolek jest bardzo prosta, 
         nie bój się dokumentacji, a w najprostszych przypadkach, po prostu posłuż się IntelliSense)
         9.	    Niech wczytywanie odbywa się po naciśnięciu przycisku – zwróć uwagę na brak responsywności GUI
         10.	Zaimplementuj tą samą metodę wykorzystująć TAP
         11.    Zaimplementuj metodę wczytywania dużego rozmiaru macierzy z pliku do zmiennej
         12.	Zaimplementuj metodę zapisywania tablicy bajtów do pliku
         13.	Zaproponuj implementację prostego protokołu komunikacyjnego (HI, ACK, BYE) – jak bardzo abstrakcyjnym pojęciem jest komunikat, jak powinien być interpretowany, czy zasługuje na własny typ, czy ciąg znaków jest „OK”?
         14.	Zamień obsługę serwera Echo na implementację swojego protokołu
         15.	Zapoznaj się z protokołem FTP, jak dużo pracy musiałbyś wykonać, aby stworzyć prosty serwer FTP? 
        */
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
            public Task ServerTask
            {
                get { return serverTask; }
            }
            #endregion
            #region Properties
            public IPAddress Address
            {
                get { return address; }
                set
                {
                    if (!running) address = value;
                    else;
                }
            }
            public int Port
            {
                get { return port; }
                set
                {
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
            #region variables
            TcpClient client;
            #endregion
            #region properties
            #endregion
            #region Constructors
            #endregion
            #region Methods
            public void Connect()
            {
                client = new TcpClient();
                client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            }
            public async Task<string> Ping(string message)
            {
                byte[] buffer = new ASCIIEncoding().GetBytes(message);
                client.GetStream().WriteAsync(buffer, 0, buffer.Length);
                buffer = new byte[1024];
                var t = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, t);
            }
            public async Task<IEnumerable<string>> keepPinging(string message, CancellationToken token)
            {
                List<string> messages = new List<string>();
                bool done = false;
                while (!done)
                {
                    if (token.IsCancellationRequested)
                        done = true;
                    messages.Add(await Ping(message));
                }
                return messages;
            }
            #endregion
        }
        public void Zadanie48()
        {
            Server s = new Server();
            s.Run();
            Client c1 = new Client();
            Client c2 = new Client();
            Client c3 = new Client();
            c1.Connect();
            c2.Connect();
            c3.Connect();
            CancellationTokenSource ct1 = new CancellationTokenSource();
            CancellationTokenSource ct2 = new CancellationTokenSource();
            CancellationTokenSource ct3 = new CancellationTokenSource();
            var client1T = c1.keepPinging("message", ct1.Token);
            var client2T = c2.keepPinging("message", ct2.Token);
            var client3T = c3.keepPinging("message", ct3.Token);
            ct1.CancelAfter(2000);
            ct2.CancelAfter(3000);
            ct3.CancelAfter(4000);
            Task.WaitAll(new Task[] { client1T, client2T, client3T });
            s.StopRunning();
        }
        #endregion
        static void Main(string[] args)
        {
            //zad1();
            //var task = OperationTask(0);
            //task.Wait();
            /*
            int test = 500;
            byte[] buffer = new byte[128];
            Console.WriteLine("begin main");
            Task task = OperationTask(buffer);
            Thread.Sleep(test);
            Console.WriteLine("progress main");
            task.Wait();
            Console.WriteLine("end main"); //do podstawowego taska
            */
            /*
            byte[] buffer = new byte[128];
            Task task = OperationTask2(buffer);
            Console.WriteLine("main");
            Console.ReadKey();
            */
            }
        }
    }



