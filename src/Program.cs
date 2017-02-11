using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synaptic_driver_reviver
{
    class Program
    {
        static void Main(string[] args)
        {
            var synDriver = new SynapticDriver();
            synDriver.Revive();
            Console.WriteLine("Done!");
        }

    }
}
