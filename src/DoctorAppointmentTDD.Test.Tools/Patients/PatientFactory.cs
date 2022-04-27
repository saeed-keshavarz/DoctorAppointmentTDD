using DoctorAppointmentTDD.Entities;
using DoctorAppointmentTDD.Service.Patients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentTDD.Test.Tools.Patients
{
    public static class PatientFactory
    {
        public static UpdatePatientDto GenerateUpdatePatientDto(string firstName)
        {
            return new UpdatePatientDto
            {
                FirstName = firstName,
                LastName = "editedDummy",
                NationalCode = "2380132933"
            };
        }

        public static Patient CreatePatient(string nationalCode)
        {
            return new Patient
            {
                FirstName = "dummyName",
                LastName = "dummyLastname",
                NationalCode = nationalCode,
            };
        }

        public static AddPatientDto GenerateAddPatientDto()
        {
            return new AddPatientDto
            {
                FirstName = "dummy",
                LastName = "dummy",
                NationalCode = "2380132933",

            };
        }

        public static List<Patient> CreatePatientsInDatabase()
        {
            return new List<Patient>()
            {
                new Patient {FirstName="dummy1", LastName="dummy1", NationalCode="2380132931"},
                new Patient {FirstName="dummy2", LastName="dummy2", NationalCode="2380132932"},
                new Patient {FirstName="dummy3", LastName="dummy3", NationalCode="2380132933"},
            }.ToList();

        }
    }
}
