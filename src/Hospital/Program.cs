using System;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Hospital.App.Interfaces;
using Hospital.Resources;

namespace Hospital.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                    .AddSingleton<HospitalResource>()
                    .AddSingleton<IHospitalData, HospitalData>()
                    .AddSingleton<IPatientRegistrationImport, PatientRegistrationImport>()
                    .AddSingleton<ILogicApplication, LogicApplication>()
                    .AddSingleton<IRunAHospital, RunAHospital>()
                    .BuildServiceProvider();

            var consultations = serviceProvider.GetService<IRunAHospital>().BookAppointments();
            Console.WriteLine(JsonConvert.SerializeObject(consultations, Formatting.Indented, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" }));

            Console.ReadKey();
        }
    }
}
