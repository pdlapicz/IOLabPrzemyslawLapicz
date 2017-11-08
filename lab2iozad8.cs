//Autor: Przemyslaw Lapicz
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*ZADANIE 8
Napisz program, który asynchronicznie wywoła metody rekurencyjnego 
oraz iteracyjnego obliczania silni oraz kolejnych elementów ciągu Fibonacciego. 
Sprawdź, które metody zostaną wykonane jako pierwsze. W swoim rozwiązaniu użyj
metod BeginInvoke, EndInvoke oraz delegatów. Samodzielnie dobierz tryb APM. */

namespace lab2iozad8
{
    class Program
    {
        delegate int DelegateType(object arguments);
        static DelegateType delegateName;

        //FUNKCJE BEZ DELEGATU
        static void rekurencyjnaSilnia(IAsyncResult state)
        {
            object[] obj = (object[])state.AsyncState;
            int wynik = (int)obj[1];
            int n = (int)obj[0];
            if (n < 2)
                wynik = 1;

    //        wynik = n * rekurencyjnaSilnia(n - 1);
        }

        static void iteracyjnaSilnia(IAsyncResult state)
        {
            object[] obj = (object[])state.AsyncState;
            int wynik = (int)obj[1];
            int n = (int)obj[0];
            int wynikTemp=1;
            if (n == 0) wynik = 1;
            else
            {
                while (n > 0)
                {
                    wynikTemp *= n;
                    n--;
                }
                wynik = wynikTemp;
            }
        }

        static void rekurencyjnyFibonacci(IAsyncResult state)
        {
            //IAsyncResult[] args = new IAsyncResult[2];
            //int n1,n2;
            object[] obj = (object[])state.AsyncState;
            int wynik = (int)obj[1];
            int n = (int)obj[0];
            if ((n == 1) || (n == 2))
                wynik = 1;
            else
            {
    /*            n1 = n - 1;
                n2 = n - 2;
                args[0] = (IAsyncResult)n1;
                args[1] = (IAsyncResult)n2;*/
            //    wynik = rekurencyjnyFibonacci(args[1]) + rekurencyjnyFibonacci(n - 2);
            }
        }

        static void iteracyjnyFibonacci(IAsyncResult state)
        {
            object[] obj = (object[])state.AsyncState;
            int wynik = (int)obj[1];
            int n = (int)obj[0];
            int a = 0, b = 1;
            for (int i = 0; i < n; i++)
            {
                b += a;
                a = b - a;
            }
            wynik = a;
        }

        //FUNKCJE Z DELEGATEM
        static int rekSilnia(object arguments)
        {
            int n = (int)arguments;
            if (n < 2)
                return 1;

            return n * rekSilnia(n - 1);
        }

        static int iterSilnia(object arguments)
        {
            int n = (int)arguments;
            int wynik = 1;
            if (n == 0) return wynik;
            else
            {
                while (n > 0)
                {
                    wynik *= n;
                    n--;
                }
                return wynik;
            }
        }

        static int rekFibonacci(object arguments)
        {
            int n = (int)arguments;
            if ((n == 1) || (n == 2))
                return 1;
            else
                return rekFibonacci(n - 1) + rekFibonacci(n - 2);
        }

        static int iterFibonacci(object arguments)
        {
            int n = (int)arguments;
            int a = 0, b = 1;
            for (int i = 0; i < n; i++)
            {
                b += a;
                a = b - a;
            }
            return a;
        }
        static void Main(string[] args)
        {
            //        IAsyncResult  = fs.BeginRead(buffer, 0, buffer.Length, null, null);

            Console.WriteLine("\nDELEGATY");
            //wersja z delegatami
            delegateName = new DelegateType(rekSilnia);
            IAsyncResult ar1 = delegateName.BeginInvoke(5, null, null);
            int result1 = delegateName.EndInvoke(ar1);

            delegateName = new DelegateType(iterSilnia);
            IAsyncResult ar2 = delegateName.BeginInvoke(6, null, null);
            int result2 = delegateName.EndInvoke(ar2);

            delegateName = new DelegateType(rekFibonacci);
            IAsyncResult ar3 = delegateName.BeginInvoke(5, null, null);
            int result3 = delegateName.EndInvoke(ar3);

            delegateName = new DelegateType(iterFibonacci);
            IAsyncResult ar4 = delegateName.BeginInvoke(6, null, null);
            int result4 = delegateName.EndInvoke(ar4);

            Console.WriteLine("Silnia z 5 rekurencyjnie: " + result1);
            Console.WriteLine("Silnia z 6 rekurencyjnie: " + result2);
            Console.WriteLine("Silnia z 5 rekurencyjnie: " + result3);
            Console.WriteLine("Silnia z 6 rekurencyjnie: " + result4);
        }
    }
}
