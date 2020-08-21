using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DAN_LIV_Milos_Peric
{
    class Car : MotorVehicle
    {
        private static Random random = new Random();
        private static readonly object lockObj = new object();
        private static readonly object lockObj2 = new object();
        private static readonly object lockObj3 = new object();
        private static readonly object lockObj4 = new object();
        private static System.Timers.Timer raceTimer = new System.Timers.Timer();
        private static System.Timers.Timer semaphoreTimer = new System.Timers.Timer();
        private static System.Timers.Timer fuelTimer = new System.Timers.Timer();
        private static int positionCounter;
        private static int timer;
        private static SemaphoreSlim carSynchronizationSemaphore1 = new SemaphoreSlim(0, 3);
        private static SemaphoreSlim carSynchronizationSemaphore2 = new SemaphoreSlim(0, 3);
        private static SemaphoreSlim carSynchronizationSemaphore3 = new SemaphoreSlim(0, 3);
        private static SemaphoreSlim carSynchronizationSemaphore4 = new SemaphoreSlim(0, 3);
        private static string raceSemaphore = "Red";
        private static Dictionary<string, Car> racingCars = new Dictionary<string, Car>();
        private static int ranOutFuelCounter = 0;
        private static int firstPlaceCounter = 0;
        private static int outOfFuelCounter = 0;
        public string RegistrationNumber { get; set; }
        public int NumberOfDoors { get; set; }
        public int FuelTankCapacity { get; set; }
        public string TransmissionType { get; set; }
        public string Producer { get; set; }
        public int DriversLicenceNumber { get; set; }

        public Car()
        {

        }

        public Car(int fuel)
        {
            FuelTankCapacity = fuel;
        }

        /// <summary>
        /// Changes car color.
        /// </summary>
        /// <param name="color">Car object color.</param>
        public void ChangeColor(string color)
        {
            Color = color;
        }

        /// <summary>
        /// Main method for race simulation.
        /// </summary>
        /// <param name="obj">Car object passed through as thread parameter.</param>
        public static void Race(object obj)
        {
            
            Console.WriteLine($"{Thread.CurrentThread.Name} has started a race");
            racingCars[Thread.CurrentThread.Name] = (Car)obj;
            positionCounter++;
            if (positionCounter == 3)
            {
                StartRaceCounterParameters();
                ChangeFuelCounterParameters();
            }
            carSynchronizationSemaphore1.Wait();
            positionCounter = 0;
            Console.WriteLine($"{Thread.CurrentThread.Name} is waiting at semaphore - light: {raceSemaphore}.");
            positionCounter++;
            if (positionCounter == 3)
            {
                SwitchSemaphoreEventParameters();
            }
            carSynchronizationSemaphore2.Wait();
            raceTimer.Enabled = true;
            Console.WriteLine($"{Thread.CurrentThread.Name} has passed semaphore.");         
            carSynchronizationSemaphore3.Wait();
            Console.WriteLine($"{Thread.CurrentThread.Name} has reched gas station.");
            lock (lockObj2)
            {
                AbortOutOfFuelCars();
            }
            //fuelTimer.Enabled = false;
            lock (lockObj)
            {
                Refuel();
            }
            carSynchronizationSemaphore4.Wait();
            lock (lockObj4)
            {
                RemoveOutOfFuelCarFromRace();
            }
            Console.WriteLine($"{Thread.CurrentThread.Name} has reached the finish line.");
            lock (lockObj3)
            {
                CheckFirstPlace();
            }
        }

        /// <summary>
        /// Checks if car is out of fuel and stops current thread.
        /// </summary>
        private static void AbortOutOfFuelCars()
        {
            foreach (var item in racingCars)
            {
                if (item.Key == Thread.CurrentThread.Name)
                {
                    if (item.Value.FuelTankCapacity <= 0)
                    {
                        Console.WriteLine($"{Thread.CurrentThread.Name} is out of fuel, and is out of race.");
                        Thread.CurrentThread.Abort();
                    }

                }
            }
        }

        /// <summary>
        /// Checks if car needs a refuel. Condition FuelTankCapacity <= 15.
        /// </summary>
        private static void Refuel()
        {
            if (racingCars.ContainsKey(Thread.CurrentThread.Name))
            {
                for (int i = 0; i < racingCars.Count; i++)
                {
                    if (racingCars.ElementAt(i).Key == Thread.CurrentThread.Name && racingCars.ElementAt(i).Value.FuelTankCapacity <= 15)
                    {
                        Console.WriteLine($"{Thread.CurrentThread.Name} is refueling at the gas station.");
                        racingCars.ElementAt(i).Value.FuelTankCapacity = 50;
                        fuelTimer.Enabled = true;
                        Console.WriteLine($"{Thread.CurrentThread.Name} has refueled and continues race.");
                    }
                    else if(racingCars.ElementAt(i).Key == Thread.CurrentThread.Name && racingCars.ElementAt(i).Value.FuelTankCapacity > 15)
                    {
                        Console.WriteLine($"{Thread.CurrentThread.Name} has enough fuel and continues race.");
                    }
                }
            }
        }

        /// <summary>
        /// Removes out of fuel car from race.
        /// </summary>
        private static void RemoveOutOfFuelCarFromRace()
        {
            if (racingCars.ContainsKey(Thread.CurrentThread.Name))
            {
                foreach (var item in racingCars)
                {
                    if (item.Key == Thread.CurrentThread.Name && item.Value.FuelTankCapacity <= 0)
                    {
                        outOfFuelCounter++;
                        if (outOfFuelCounter == 3)
                        {
                            Console.WriteLine($"No car has reached the finish line.");
                        }
                        Console.WriteLine($"{Thread.CurrentThread.Name} is out of fuel and is out of race.");
                        Thread.Sleep(Timeout.Infinite);
                    }
                }
            }
        }

        private static void CheckFirstPlace()
        {
            if (firstPlaceCounter == 0)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} has won the race!");
                firstPlaceCounter = 1;
            }
        }

        /// <summary>
        /// Initiates race timer event logic.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void RaceTimerEvent(Object source, ElapsedEventArgs e)
        {
            timer += 1;
            Console.WriteLine("Cars are racing for {0} seconds.", timer);
            if (timer == 10)
            {
                carSynchronizationSemaphore1.Release(3);
                //raceTimer.Enabled = false;
            }
            if (timer == 13)
            {
                carSynchronizationSemaphore3.Release(3);
            }
            if (timer == 20)
            {
                carSynchronizationSemaphore4.Release(3);
                raceTimer.Enabled = false;
                semaphoreTimer.Enabled = false;
                fuelTimer.Enabled = false;
                if (racingCars.Count == 0)
                {
                    Console.WriteLine("No car arrived at finish line. Race has no winner.");
                }
            }
        }

        /// <summary>
        /// Initiate semapfore switch logic.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void SemaphoreSwitcherEvent(Object source, ElapsedEventArgs e)
        {
            if (raceSemaphore == "Red")
            {
                raceSemaphore = "Green";
                Console.WriteLine("The semaphore light is {0}.", raceSemaphore);
                carSynchronizationSemaphore2.Release(3);
                semaphoreTimer.Enabled = false;
            }
            else
            {
                raceSemaphore = "Red";
            }
            
        }

        /// <summary>
        /// Initiates fuel spending logic.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void SpendFuelEvent(Object source, ElapsedEventArgs e)
        {
            lock (lockObj)
            {
                int dictSize = racingCars.Count;
                for (int i = 0; i < dictSize; i++)
                {
                    if (racingCars.ElementAt(i).Value.FuelTankCapacity <= 0)
                    {
                        Thread.Sleep(1000);
                        racingCars.Remove(racingCars.ElementAt(i).Key);
                        dictSize--;
                    }
                }
                foreach (var item in racingCars)
                {
                    item.Value.FuelTankCapacity -= random.Next(1, 5);
                    if (item.Value.FuelTankCapacity >= 0)
                    {
                        if (item.Value.FuelTankCapacity < 0)
                        {
                            Console.WriteLine($"{item.Key} fuel: 0");
                        }
                        else
                        {
                            Console.WriteLine($"{item.Key} fuel: {item.Value.FuelTankCapacity}");
                        }                      
                    }
                    else
                    {
                        if (ranOutFuelCounter < 3)
                        {
                            ranOutFuelCounter++;
                        }
                    }
                }
            }           
        }

        /// <summary>
        /// Sets event paremeters for fuel spending.
        /// </summary>
        public static void ChangeFuelCounterParameters()
        {
            fuelTimer.Interval = 1000;
            fuelTimer.Elapsed += SpendFuelEvent;
            fuelTimer.AutoReset = true;
            fuelTimer.Enabled = true;
        }

        /// <summary>
        /// Sets event parameters for race counter.
        /// </summary>
        public static void StartRaceCounterParameters()
        {
            raceTimer.Interval = 1000;
            raceTimer.Elapsed += RaceTimerEvent;
            raceTimer.AutoReset = true;
            raceTimer.Enabled = true;
        }

        /// <summary>
        /// Sets event paremeters for semaphore switch.
        /// </summary>
        public static void SwitchSemaphoreEventParameters()
        {
            semaphoreTimer.Interval = 2000;
            semaphoreTimer.Elapsed += SemaphoreSwitcherEvent;
            semaphoreTimer.AutoReset = true;
            semaphoreTimer.Enabled = true;
        }
    }
}
