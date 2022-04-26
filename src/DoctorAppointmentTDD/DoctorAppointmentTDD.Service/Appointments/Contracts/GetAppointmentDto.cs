using DoctorAppointmentTDD.Service.Doctors.Contracts;
using DoctorAppointmentTDD.Service.Patients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentTDD.Service.Appointments.Contracts
{
    public class GetAppointmentDto
    {
        public GetAppointmentDto()
        {
            doctor = new GetDoctorDto();
            patient = new GetPatientDto();
        }
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public GetDoctorDto doctor { get; set; }
        public GetPatientDto patient { get; set; }
    }
}
