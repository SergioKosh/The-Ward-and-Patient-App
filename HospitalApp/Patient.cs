using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalApp
{
    public class Patient
    {
        public string Name { get; set; }
        public BloodType Blood { get; set; }

        public DateTime Birthday { get; set; }

        public int YearsOld { get; set; }

        //constructor 
        public Patient(string name, BloodType blood, DateTime birthday)
        {
            DateTime today = DateTime.Today;

            Name = name;
            Blood = blood;
            Birthday = birthday;
            YearsOld = today.Year - birthday.Year;
        }

       

        public override string ToString()
        {
            return $"{Name} ({YearsOld}) Type:{Blood}";
        }

    }



}
