using Newtonsoft.Json;
using Hospital.App.Interfaces;
using Hospital.App.Models;
using Hospital.Resources;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hospital.App
{
    class PatientRegistrationImport : IPatientRegistrationImport
    {
        public List<PatientRegistrationModel> GetPatientsRegistered()
        {
            return JsonConvert.DeserializeObject<PatientRegistration[]>(File.ReadAllText("./Data/registrations.json"))
                .Select(x => x.Patient).ToList();
        }
    }
}
