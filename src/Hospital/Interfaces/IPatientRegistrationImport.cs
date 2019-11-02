using Hospital.App.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.App.Interfaces
{
    public interface IPatientRegistrationImport
    {
        List<PatientRegistrationModel> GetPatientsRegistered();
    }
}
