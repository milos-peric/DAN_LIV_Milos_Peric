using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAN_LIV_Milos_Peric
{
    class Truck : MotorVehicle
    {
        public double CargoCapacity { get; set; }
        public double Height { get; set; }
        public int NumberOfSeats { get; set; }
        public void Load() { }
        public void Unload() { }
    }
}
