using System;

namespace Hospital.App.Models
{
    public class PatientRegistrationModel
    {
        public Guid UniqueId => Guid.NewGuid();
        public DateTime RegistrationDate => DateTime.Now.Date;
        public string Name { get; set; }
        public string Condition { get; set; }
        public string Topography { get; set; }
    }
}