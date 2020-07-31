using System;

namespace RCN.Polly
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Inicializando...");

            var api = new ChamaAPI();

            for (int i = 1; i <= 100; i++)
            {
                System.Console.WriteLine($"Realizando chamada {i}...");

                /*metodo 1*/
               // var result1 = api.Retentativa_get1().Result;
            //    System.Console.WriteLine($"Retorno API:{result1}");

                /*metodo 2*/
                var result2 = api.Get2().Result;
                System.Console.WriteLine($"Retorno API:{result2}");

                /*metodo 2*/
               // var result3 = api.Get3().Result;
              //  System.Console.WriteLine($"Retorno API:{result3}");

            }
        }


    }
}
