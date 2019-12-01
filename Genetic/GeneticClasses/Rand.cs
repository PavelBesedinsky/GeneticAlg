using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic.GeneticClasses
{
    static class Rand
    {
        static Random random = new Random();

        static public int IntRand(double fMin, double fMax)
        {
            return random.Next((int)fMin, (int)fMax);
        }

        static public double DoubleRand(double fMin, double fMax)
        {
            return random.NextDouble() * (fMax - fMin) + fMin;
        }
    }
}
