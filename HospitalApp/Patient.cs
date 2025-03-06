using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalApp
{
    internal class Patient
    {
        public string Name { get; set; }
        public string BloodType { get; set; }

        public DateTime Birthday { get; set; }

        public int YearsOld { get; set; }

        //constructor 
        public Patient(string name, string bloodType, DateTime birthday)
        {
            DateTime today = DateTime.Today;

            Name = name;
            BloodType = bloodType;
            Birthday = birthday;
            YearsOld = today.Year - birthday.Year;
        }

        public override string ToString()
        {
            return $"{Name} ({YearsOld}) Type:{BloodType}";
        }

    }



}
