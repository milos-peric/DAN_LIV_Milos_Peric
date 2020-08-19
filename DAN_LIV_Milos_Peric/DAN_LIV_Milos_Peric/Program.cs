using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAN_LIV_Milos_Peric
{
    class Program
    {
        static void Main(string[] args)
        {
            Car vehicle = new Car();
            vehicle.Color = "Blue";
            Console.WriteLine(vehicle.Color);
            vehicle.ChangeColor("Red");
            Console.WriteLine(vehicle.Color);
            Console.ReadKey();
        }
    }
}
