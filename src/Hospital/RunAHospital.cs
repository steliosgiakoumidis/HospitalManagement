using Hospital.App.Interfaces;
using Hospital.App.Models;
using Hospital.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital.App
{
    public class RunAHospital : IRunAHospital
    {
        private IHospitalData _hospitalData;
        private ILogicApplication _logicApplication;
        private IPatientRegistrationImport _patientRegistrationImport;

        public RunAHospital(IHospitalData hospitalData, ILogicApplication logicApplication, IPatientRegistrationImport patientRegistrationImport)
        {
            _hospitalData = hospitalData;
            _logicApplication = logicApplication;
            _patientRegistrationImport = patientRegistrationImport;
        }

        public List<ConsultationModel> BookAppointments()
        {
            try
            {
                _hospitalData.InitiateReadHospitalDataFromFile();
                var registrations = _patientRegistrationImport.GetPatientsRegistered();
                var patientsRequiredResources = registrations
                          .Select(x => _logicApplication
                            .IdentifyResources(x))
                          .ToList();

                return _logicApplication.GetConsultation(patientsRequiredResources);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unknown error occured. Exception details: {ex}");
                throw ex;
            }

        }
    }
}
