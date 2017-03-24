using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LA.Classes;

namespace LA
{
    class Program
    {
        static void Main(string[] args)
        {
            //  testovanie
            //var dka = new DKA("test.txt");
            //dka.AnalyzeText("input.txt");

            //  odovzdavanie
            try
            {
                var dka = new DKA(args[0]);
                dka.AnalyzeText(args[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();
        }
    }
}
