using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Hospital.Resources
{
    public class HospitalData:IHospitalData
    {
        private HospitalResource _hospitalData;

        public HospitalData(HospitalResource hospitalData)
        {
            _hospitalData = hospitalData;
        }

        public void InitiateReadHospitalDataFromFile()
        {
            InitiateHospitalResources();
            var resource = JsonConvert.DeserializeObject<HospitalResource>(File.ReadAllText("./Data/resources.json"));
            foreach (var doctor in resource.Doctors)
            {
                AddDoctor(doctor.Name, doctor.Roles);
            }
            foreach (var machine in resource.TreatmentMachines)
            {
                AddMachine(machine.Name, machine.Capability);
            }
            foreach (var room in resource.TreatmentRooms)
            {
                AddRoom(room.Name, room.TreatmentMachine);
            }
        }

        private void InitiateHospitalResources()
        {
            _hospitalData.Doctors = new List<DoctorResource>();
            _hospitalData.TreatmentRooms = new List<TreatmentRoomResource>();
            _hospitalData.TreatmentMachines = new List<TreatmentMachineResource>();
        }

        private void AddRoom(string name, string treatmentMachine)
        {
            _hospitalData.TreatmentRooms.Add(new TreatmentRoomResource(name, treatmentMachine));
        }

        private void AddMachine(string name, string capability)
        {
            _hospitalData.TreatmentMachines.Add(new TreatmentMachineResource(name, capability));
        }

        private void AddDoctor(string name, List<string> roles)
        {
            _hospitalData.Doctors.Add(new DoctorResource(name, roles));
        }
    }
}
