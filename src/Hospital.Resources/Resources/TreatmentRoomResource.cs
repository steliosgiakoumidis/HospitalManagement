using System;
using System.Collections.Generic;

namespace Hospital.Resources
{
    public class TreatmentRoomResource
    {
        public string Name { get; set; }
        public string TreatmentMachine { get; set; }
        public List<DateTime> FullyBookedDays { get; set; }

        public TreatmentRoomResource(string name, string treatmentMachine)
        {
            Name = name;
            TreatmentMachine = treatmentMachine;
            FullyBookedDays = new List<DateTime>();
        }

        public TreatmentRoomResource()
        {
        }
    }
}