using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAN_LIV_Milos_Peric
{
    abstract class MotorVehicle
    {
        public double EngineCapacity { get; set; }
        public int Weight { get; set; }
        public string Category { get; set; }
        public string EngineType { get; set; }
        public string Color { get; set; }
        public int EngineNumber { get; set; }

        public void Start() { }
        public void Stop() { }
    }
}
