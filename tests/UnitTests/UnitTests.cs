using Moq;
using Hospital.App;
using Hospital.App.Interfaces;
using Hospital.App.Models;
using Hospital.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitAndIntegrationTests
{
    public class UnitTests
    {
        private HospitalResource _hospitalResources = new HospitalResource();
        private Mock<IPatientRegistrationImport> _mockedPatient = new Mock<IPatientRegistrationImport>();

        [Theory]
        [InlineData("erik", "cancer", "breast", "Oncologist", "Room1")]
        [InlineData("erika", "cancer", "head_neck", "Oncologist", null)]
        [InlineData("hans", "flu", "", "GeneralPractitioner", null)]
        [InlineData("hampus", "embola", "", "unknown", "unknown")]

        public void IdentifyResourcesTests(string patientName, string condition, string topology,
            string expectedDoctorSpecialization, string expectedRoom)
        {
            _hospitalResources.Doctors = new List<DoctorResource>()
            {
                new DoctorResource("DoctorWho", new List<string>() { "Oncologist", "Cosmologist" })
            };
            _hospitalResources.TreatmentRooms = new List<TreatmentRoomResource>()
            {
                new TreatmentRoomResource() { Name = "Room1", TreatmentMachine = "Machine1", FullyBookedDays = new List<DateTime>() }
            };
            _hospitalResources.TreatmentMachines = new List<TreatmentMachineResource>()
            {
                new TreatmentMachineResource("Machine1", "Simple")
            };

            var logicApplication = new LogicApplication(_hospitalResources);
            _mockedPatient.Setup(p => p.GetPatientsRegistered())
                .Returns(new List<PatientRegistrationModel>()
                {
                    new PatientRegistrationModel() { Name = patientName, Condition = condition, Topography = topology}
                });
            var patients = _mockedPatient.Object.GetPatientsRegistered();

            var sut = patients.Select(x => logicApplication.IdentifyResources(x)).ToList();

            Assert.Equal(expectedDoctorSpecialization, sut[0].DoctorsSpecialization);
            Assert.Equal(expectedRoom, sut[0].TreatmentRoom.FirstOrDefault());

        }

        [Theory]
        [InlineData("erik", "Oncologist", "Room1", true)]
        [InlineData("erika", "Oncologist", null, false)]
        [InlineData("hans", "GeneralPractitioner", null, false)]
        [InlineData("hampus", "unknown", "unknown", false)]
        public void GetConsultationTests(string patientName,
                string expectedDoctorSpecialization, string expectedRoom, bool willGetAnAppointment)
        {
            _hospitalResources.Doctors = new List<DoctorResource>()
            {
                new DoctorResource("DoctorWho", new List<string>() { "Oncologist", "Cosmologist" })
            };
            _hospitalResources.TreatmentRooms = new List<TreatmentRoomResource>()
            {
                new TreatmentRoomResource() { Name = "Room1", TreatmentMachine = "Machine1", FullyBookedDays = new List<DateTime>() }
            };
            _hospitalResources.TreatmentMachines = new List<TreatmentMachineResource>()
            {
                new TreatmentMachineResource("Machine1", "Simple")
            };

            var logicApplication = new LogicApplication(_hospitalResources);
            var test = new List<PatientAndRequirements>()
            {
                new PatientAndRequirements(){DoctorsSpecialization = expectedDoctorSpecialization, Name = patientName,
                UniqueId = Guid.NewGuid(), TreatmentRoom = new List<string>(){expectedRoom }}
            };

            var sut = logicApplication.GetConsultation(test);

            Assert.Equal(sut.Any(), willGetAnAppointment);
        }
    }
}
