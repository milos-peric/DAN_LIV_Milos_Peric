using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAN_LIV_Milos_Peric
{
    class Car : MotorVehicle
    {
        public string RegistrationNumber { get; set; }
        public int NumberOfDoors { get; set; }
        public int FuelTankCapacity { get; set; }
        public string TransmissionType { get; set; }
        public string Producer { get; set; }
        public int DriversLicenceNumber { get; set; }

        public void ChangeColor(string color)
        {
            Color = color;
        }

    }
}
