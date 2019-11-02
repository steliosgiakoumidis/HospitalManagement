using Moq;
using Hospital.App;
using Hospital.App.Interfaces;
using Hospital.App.Models;
using Hospital.Resources;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitAndIntegrationTests
{
    public class IntegrationTests
    {
        private HospitalResource _hospitalResources =  new HospitalResource();
        private Mock<IHospitalData> _mockedHospitalData = new Mock<IHospitalData>();
        private Mock<IPatientRegistrationImport>_mockedPatient = new Mock<IPatientRegistrationImport>();

        [Fact]
        public void SinglePatientSameDate()
        {
            _hospitalResources.Doctors = new List<DoctorResource>() { new DoctorResource("DoctorWho", new List<string>() { "Oncologist", "Cosmologist" })};
            _hospitalResources.TreatmentRooms = new List<TreatmentRoomResource>() { new TreatmentRoomResource() { Name = "Room1", TreatmentMachine = "Machine1", FullyBookedDays = new List<DateTime>() }};
            _hospitalResources.TreatmentMachines = new List<TreatmentMachineResource>() { new TreatmentMachineResource("Machine1", "Simple")};

            var logic = new LogicApplication(_hospitalResources);
            _mockedPatient.Setup(p => p.GetPatientsRegistered()).Returns(new List<PatientRegistrationModel>() { new PatientRegistrationModel() { Name = "Stelios", Condition = "cancer", Topography = "breast" } });

            var runAHospital = new RunAHospital(_mockedHospitalData.Object, logic, _mockedPatient.Object);
            var sut = runAHospital.BookAppointments();

            Assert.NotEmpty(sut);
            Assert.Equal("DoctorWho", sut[0].DoctorName);
            Assert.Equal("Room1", sut[0].RoomName);
            Assert.Equal(DateTime.Now.Date, sut[0].Date);
        }

        [Fact]
        public void SinglePatientRegistrationTwoNextDaysIsBooked()
        {
            _hospitalResources.Doctors = new List<DoctorResource>() { new DoctorResource("DoctorWho", new List<string>() { "Oncologist", "Cosmologist" }) };
            _hospitalResources.TreatmentRooms = new List<TreatmentRoomResource>() { new TreatmentRoomResource() { Name = "Room1", TreatmentMachine = "Machine1", FullyBookedDays = new List<DateTime>() { DateTime.Now.Date, DateTime.Now.Date.AddDays(1)} } };
            _hospitalResources.TreatmentMachines = new List<TreatmentMachineResource>() { new TreatmentMachineResource("Machine1", "Simple") };

            var logic = new LogicApplication(_hospitalResources);
            _mockedPatient.Setup(p => p.GetPatientsRegistered()).Returns(new List<PatientRegistrationModel>() { new PatientRegistrationModel() { Name = "Stelios", Condition = "cancer", Topography = "breast" } });

            var runAHospital = new RunAHospital(_mockedHospitalData.Object, logic, _mockedPatient.Object);
            var sut = runAHospital.BookAppointments();

            Assert.NotEmpty(sut);
            Assert.Equal("DoctorWho", sut[0].DoctorName);
            Assert.Equal("Room1", sut[0].RoomName);
            Assert.Equal(DateTime.Now.Date.AddDays(2), sut[0].Date);
        }

        [Fact]
        public void TwoPatientsRegistration()
        {
            _hospitalResources.Doctors = new List<DoctorResource>() { new DoctorResource("DoctorWho", new List<string>() { "Oncologist", "Cosmologist" }) };
            _hospitalResources.TreatmentRooms = new List<TreatmentRoomResource>() { new TreatmentRoomResource() { Name = "Room1", TreatmentMachine = "Machine1", FullyBookedDays = new List<DateTime>() { DateTime.Now.Date, DateTime.Now.Date.AddDays(1) } } };
            _hospitalResources.TreatmentMachines = new List<TreatmentMachineResource>() { new TreatmentMachineResource("Machine1", "Simple") };

            var logic = new LogicApplication(_hospitalResources);
            _mockedPatient.Setup(p => p.GetPatientsRegistered()).Returns(new List<PatientRegistrationModel>() { new PatientRegistrationModel() { Name = "Stelios", Condition = "cancer", Topography = "breast" }, new PatientRegistrationModel() { Name = "Erika", Condition = "cancer", Topography = "breast" } });

            var runAHospital = new RunAHospital(_mockedHospitalData.Object, logic, _mockedPatient.Object);
            var sut = runAHospital.BookAppointments();

            Assert.NotEmpty(sut);
            Assert.Equal("DoctorWho", sut[0].DoctorName);
            Assert.Equal("Room1", sut[0].RoomName);
            Assert.Equal(DateTime.Now.Date.AddDays(2), sut[0].Date);
        }

        [Fact]
        public void TwoCancerPatientsSharingOneRoomAndAFluPatient()
        {
            _hospitalResources.Doctors = new List<DoctorResource>()
            {
                new DoctorResource("DoctorWho", new List<string>() { "Oncologist", "Cosmologist" }),
                new DoctorResource("DrJenkils", new List<string>(){ "GeneralPractitioner"})
            };
            _hospitalResources.TreatmentRooms = new List<TreatmentRoomResource>()
            {
                new TreatmentRoomResource() { Name = "Room1", FullyBookedDays = new List<DateTime>()},
                new TreatmentRoomResource() { Name = "Room2", TreatmentMachine = "Machine1", FullyBookedDays =  new List<DateTime>(){ DateTime.Now.Date, DateTime.Now.Date.AddDays(1)}}
            };
            _hospitalResources.TreatmentMachines = new List<TreatmentMachineResource>()
            {
                new TreatmentMachineResource("Machine1", "Advanced"),
            };

            var logic = new LogicApplication(_hospitalResources);
            _mockedPatient.Setup(p => p.GetPatientsRegistered()).Returns(new List<PatientRegistrationModel>()
            {
                new PatientRegistrationModel() { Name = "Stelios", Condition = "cancer", Topography = "breast" },
                new PatientRegistrationModel() { Name = "Erika", Condition = "cancer", Topography = "head_neck" },
                new PatientRegistrationModel() { Name = "Rasmus", Condition = "flu"}
            });

            var runAHospital = new RunAHospital(_mockedHospitalData.Object, logic, _mockedPatient.Object);
            var sut = runAHospital.BookAppointments();

            Assert.NotEmpty(sut);
            Assert.Equal("DoctorWho", sut[0].DoctorName);
            Assert.Equal("Room2", sut[0].RoomName);
            Assert.Equal(DateTime.Now.Date.AddDays(2), sut[0].Date);
            Assert.Equal(DateTime.Now.Date.AddDays(3), sut[1].Date);
            Assert.Equal("DrJenkils", sut[2].DoctorName);
            Assert.Equal("Room1", sut[2].RoomName);
            Assert.Equal(DateTime.Now.Date, sut[2].Date);
        }
    }
}
