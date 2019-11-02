using System;

namespace Hospital.App.Models
{
    public class ConsultationModel
    {
        public DateTime Date { get; set; }
        public string DoctorName { get; set; }
        public string RoomName { get; set; }
        public string PatientName { get; set; }
        public Guid PatientUniqueId { get; set; }

        public ConsultationModel(DateTime date, string doctorName, string roomName, string patientName, Guid patientUniqueId)
        {
            Date = date;
            DoctorName = doctorName;
            RoomName = roomName;
            PatientName = patientName;
            PatientUniqueId = patientUniqueId;
        }
    }
}