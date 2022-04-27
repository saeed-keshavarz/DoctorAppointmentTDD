using DoctorAppointmentTDD.Entities;
using DoctorAppointmentTDD.Service.Doctors.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentTDD.Test.Tools.Doctors
{
    public static class DoctorFactory
    {
        public static UpdateDoctorDto GenerateUpdateDoctorDto(string firstName)
        {
            return new UpdateDoctorDto
            {
                FirstName = firstName,
                LastName = "editeddummy",
                NationalCode = "2380132933",
                Field = "editeddummyfield"
            };
        }

        public static Doctor CreateDoctor(string nationalCode)
        {
            return new Doctor
            {
                FirstName = "dummyName",
                LastName = "dummyLastname",
                NationalCode = nationalCode,
                Field = "dummyfield",
            };
        }

        public static List<Doctor> CreateDoctorsInDataBase()
        {
            return new List<Doctor>
            {
                new Doctor { FirstName = "dummy1", LastName="dummy1",NationalCode="2380132933", Field="dummy1"},
                new Doctor { FirstName = "dummy2", LastName="dummy2",NationalCode="2380257515", Field="dummy2"},
                new Doctor { FirstName = "dummy3", LastName="dummy4",NationalCode="2380132934", Field="dummy3"},
            }.ToList();

        }

        public static AddDoctorDto GenerateAddDoctorDto()
        {
            return new AddDoctorDto
            {
                FirstName = "dummyFirst",
                LastName = "dummyLast",
                NationalCode = "2380132933",
                Field = "dummyField",
            };
        }
    }
}
