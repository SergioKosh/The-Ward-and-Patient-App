using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalApp
{
    public class Ward
    {
        public string WardName { get; set; }

        public int Capacity { get; set; }

        public List<Patient> Patients { get; set; } = new List<Patient>();

        //constructor 
        public Ward() { }
        public Ward(string name, int capacity) {
            WardName = name;
            Capacity = capacity;
            Patients = new List<Patient>();
        }

        public override string ToString()
        {
            return $"{WardName} (Limit: {Capacity})";
        }
    }
}
