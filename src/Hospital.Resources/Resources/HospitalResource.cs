using System.Collections.Generic;

namespace Hospital.Resources
{
  public class HospitalResource
  {
    public List<DoctorResource> Doctors { get; set; }
    public List<TreatmentRoomResource> TreatmentRooms { get; set; }
    public List<TreatmentMachineResource> TreatmentMachines { get; set; }
    }
}