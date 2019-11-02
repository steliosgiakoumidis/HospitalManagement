using Hospital.App.Interfaces;
using Hospital.App.Models;
using Hospital.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital.App
{
    public class LogicApplication: ILogicApplication
    {
        private HospitalResource _hospitalResource;

        public LogicApplication(HospitalResource hospitalResource)
        {
            _hospitalResource = hospitalResource;
        }

        public PatientAndRequirements IdentifyResources(PatientRegistrationModel patient)
        {
            var patientRequirements = new PatientAndRequirements();
            patientRequirements.Name = patient.Name;
            patientRequirements.UniqueId = patient.UniqueId;
            PopulateTreatmentRoom(patient, patientRequirements);
            if (patient.Condition == "cancer")
            {
                patientRequirements.DoctorsSpecialization = "Oncologist";
            }
            else if (patient.Condition == "flu")
            {
                patientRequirements.DoctorsSpecialization = "GeneralPractitioner";
            }
            else
            {
                Console.WriteLine($"Unexpected doctor specialization, {patient.Condition}");
                patientRequirements.DoctorsSpecialization = "unknown";
            }

            return patientRequirements;
        }

        public List<ConsultationModel> GetConsultation(List<PatientAndRequirements> patientRequirements)
        {
            var consultationList = new List<ConsultationModel>();
            foreach (var patient in patientRequirements)
            {
                if (patient.DoctorsSpecialization == "unknown" || patient.TreatmentRoom.Contains("unknown"))
                    continue;
                var matchingDoctors = _hospitalResource.Doctors.Where(x => x.Roles.Contains(patient.DoctorsSpecialization));
                var matchingRooms = _hospitalResource.TreatmentRooms.Where(x => patient.TreatmentRoom.Contains(x.Name));
                int offset = 0;
                while (offset < 90)
                {
                    var dateToBeBooked = DateTime.Now.Date.AddDays(offset);
                    if (matchingDoctors.Any(x => !x.FullyBookedDays.Contains(dateToBeBooked))
                        && matchingRooms.Any(x => !x.FullyBookedDays.Contains(dateToBeBooked)))
                    {
                        var room = matchingRooms.First(x => !x.FullyBookedDays.Contains(dateToBeBooked));
                        var doctor = matchingDoctors.First(x => !x.FullyBookedDays.Contains(dateToBeBooked));
                        consultationList.Add(new ConsultationModel(DateTime.Now.Date.AddDays(offset),
                            doctor.Name, room.Name, patient.Name, patient.UniqueId));
                        doctor.FullyBookedDays.Add(dateToBeBooked);
                        room.FullyBookedDays.Add(dateToBeBooked);
                        break;
                    }
                    offset++;
                }
            }

            return consultationList;
        }

        private void PopulateTreatmentRoom(PatientRegistrationModel patient, PatientAndRequirements patientRequirements)
        {
            if (patient.Condition == "flu")
            {
                patientRequirements.TreatmentRoom = _hospitalResource.TreatmentRooms
                    .Where(x => String.IsNullOrEmpty(x.TreatmentMachine))
                    .Select(n => n.Name)
                    .ToList();
            }
            else if (patient.Topography == "head_neck")
            {
                patientRequirements.TreatmentRoom = _hospitalResource.TreatmentRooms
                    .Where(x => _hospitalResource.TreatmentMachines
                        .Where(t => t.Capability == "Advanced")
                        .Select(n => n.Name)
                        .Contains(x.TreatmentMachine))
                    .Select(name => name.Name)
                    .ToList();
            }
            else if (patient.Topography == "breast")
            {
                patientRequirements.TreatmentRoom = _hospitalResource.TreatmentRooms
                    .Where(x => _hospitalResource.TreatmentMachines
                        .Where(t => t.Capability == "Advanced" || t.Capability == "Simple")
                        .Select(n => n.Name)
                        .Contains(x.TreatmentMachine))
                    .Select(name => name.Name)
                    .ToList();
            }
            else
            {
                Console.WriteLine($"No matching treatment room found for customer, " +
                    $"ID: {patient.UniqueId}, Condition: {patient.Condition}, Topography: {patient.Topography}");
                patientRequirements.TreatmentRoom = new List<string>() { "unknown" };
            }
        }
    }
}
