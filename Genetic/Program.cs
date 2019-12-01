using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic
{
    class Program
    {
        static void Main(string[] args)
        {
            GeneticClasses.GeneticCommon geneticCommon = new GeneticClasses.GeneticCommon();
            geneticCommon.Execute();
            GeneticClasses.GeneticFerzin geneticFerzin = new GeneticClasses.GeneticFerzin(10, 50);
            geneticFerzin.Execute();
        }
    }
}
