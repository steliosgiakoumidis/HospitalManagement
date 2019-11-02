using Hospital.App.Models;
using Hospital.Resources;
using System.Collections.Generic;

namespace Hospital.App.Interfaces
{
    public interface ILogicApplication
    {
        PatientAndRequirements IdentifyResources(PatientRegistrationModel registration);
        List<ConsultationModel> GetConsultation(List<PatientAndRequirements> patientRequirements);
    }
}
