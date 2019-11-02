using System;
using System.Collections.Generic;

namespace Hospital.Resources
{
    public class DoctorResource
    {
        public string Name { get; set; }
        public List<string> Roles { get; set; }
        public List<DateTime> FullyBookedDays { get; set; }

        public DoctorResource(string name, List<string> roles)
        {
            Name = name;
            Roles = roles;
            FullyBookedDays = new List<DateTime>();
        }
    }
}