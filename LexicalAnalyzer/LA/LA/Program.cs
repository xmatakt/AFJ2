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
            var dka = new DKA("test.txt");
            //dka.PrintDKA();
            dka.AnalyzeText("input.txt");

            Console.ReadKey();
        }
    }
}
