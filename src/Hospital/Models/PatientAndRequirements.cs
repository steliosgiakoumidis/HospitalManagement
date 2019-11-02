using System;
using System.Collections.Generic;

namespace Hospital.App
{
    public class PatientAndRequirements
    {
        public string DoctorsSpecialization { get; set; }
        public string Name { get; set; }
        public Guid UniqueId { get; set; }
        public List<string> TreatmentRoom { get; set; }
    }
}
