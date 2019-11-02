using Hospital.App.Models;
using System.Collections.Generic;

namespace Hospital.App.Interfaces
{
    public interface IRunAHospital
    {
        List<ConsultationModel> BookAppointments();
    }
}
