using BrainScrew.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainScrew
{
    class Program
    {
        static void Main(string[] args)
        {
            BrainScrewParser brainScrewParser = new BrainScrewParser(@"..\..\Examples\HelloWorld.bs");
            brainScrewParser.Compile();
            Console.ReadLine();
        }
    }
}
