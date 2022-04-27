using DoctorAppointmentTDD.Entities;
using DoctorAppointmentTDD.Service.Appointments.Contracts;
using DoctorAppointmentTDD.Service.Doctors.Contracts;
using DoctorAppointmentTDD.Service.Patients.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentTDD.Persistence.EF.Appointments
{
    public class EFAppointmentRepository : AppointmentRepository
    {
        private readonly DbSet<Appointment> _appointments;

        public EFAppointmentRepository(ApplicationDbContext dbContext)
        {
            _appointments = dbContext.Set<Appointment>();
        }
        public void Add(Appointment appointment)
        {
            _appointments.Add(appointment);
        }

        public List<GetAppointmentDto> GetAll()
        {
            return _appointments.Select(_ => new GetAppointmentDto
            {
                Date = _.Date,
                doctor = new GetDoctorDto()
                {
                    FirstName = _.Doctor.FirstName,
                    LastName = _.Doctor.LastName,
                    NationalCode = _.Doctor.NationalCode,
                    Field = _.Doctor.Field,
                },
                patient = new GetPatientDto()
                {
                    FirstName = _.Patient.FirstName,
                    LastName = _.Patient.LastName,
                    NationalCode = _.Patient.NationalCode,

                },

            }).ToList();
        }

        public GetAppointmentDto GetAppointmentDto(int id)
        {
            return _appointments.Select(_ => new GetAppointmentDto
            {
                Date = _.Date,
                doctor = new GetDoctorDto()
                {
                    FirstName = _.Doctor.FirstName,
                    LastName = _.Doctor.LastName,
                    NationalCode = _.Doctor.NationalCode,
                    Field = _.Doctor.Field,
                },
                patient = new GetPatientDto()
                {
                    FirstName = _.Doctor.FirstName,
                    LastName = _.Doctor.LastName,
                    NationalCode = _.Doctor.NationalCode,

                },

            }).FirstOrDefault(_ => _.Id == id);
        }

        public Appointment GetById(int id)
        {
            return _appointments.FirstOrDefault(_ => _.Id == id);
        }

        public void Delete(Appointment appointment)
        {
            _appointments.Remove(appointment);
        }

        public bool Isvisited(int doctorId, int patientId, DateTime date)
        {
            return _appointments.Any(_ => _.PatientId == patientId && _.DoctorId == doctorId && _.Date == date);
        }

        public int Appointmentcount(int doctorId, DateTime date)
        {
            return _appointments.Where(_ => _.DoctorId == doctorId && _.Date == date).Count();
        }
    }
}
