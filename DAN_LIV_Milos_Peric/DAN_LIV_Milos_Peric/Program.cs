using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAN_LIV_Milos_Peric
{
    class Program
    {
        static void Main(string[] args)
        {
            Car car1 = new Car(50);
            Car car2 = new Car(50);
            Tractor tractor1 = new Tractor();
            Tractor tractor2 = new Tractor();
            Truck truck1 = new Truck();
            Truck truck2 = new Truck();
            car1.ChangeColor("Red");
            car2.ChangeColor("Red");
            List<Car> carList = new List<Car>
            {
                car1,
                car2
            };
            HashSet<Tractor> tractorList = new HashSet<Tractor>
            {
                tractor1,
                tractor2
            };
            LinkedList<Truck> truckList = new LinkedList<Truck>();
            truckList.AddLast(truck1);
            truckList.AddLast(truck2);
            StartRaceCounter();
            Car car3 = new Car(50);
            car3.ChangeColor("Orange");
            carList.Add(car3);
            Console.WriteLine("Cars arrived at start line.");
            for (int i = 1; i <= 3; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(Car.Race));
                t.Name = string.Format($"Car {i}");
                t.Start(carList.ElementAt(i - 1));
            }
            Console.ReadKey();
        }

        /// <summary>
        /// Initiates countdown for race start.
        /// </summary>
        private static void StartRaceCounter()
        {
            for (int i = 5; i >= 1; i--)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"{i} seconds to start.");
            }
        }
    }
}
