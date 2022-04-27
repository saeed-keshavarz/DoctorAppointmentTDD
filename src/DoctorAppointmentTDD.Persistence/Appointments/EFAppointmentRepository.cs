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
        private readonly ApplicationDbContext _datacontext;

        public EFAppointmentRepository(ApplicationDbContext dbContext)
        {
            _datacontext=dbContext;
        }
        public void Add(Appointment appointment)
        {
            _datacontext.Appointments.Add(appointment);
        }

        public List<GetAppointmentDto> GetAll()
        {
            return _datacontext.Appointments.Select(_ => new GetAppointmentDto
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

        public GetAppointmentDto GetAppointmentById(int id)
        {
            return _datacontext.Appointments.Select(_ => new GetAppointmentDto
            {
                Id = id,
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

            }).FirstOrDefault(_ => _.Id == id);
        }

        public Appointment GetById(int id)
        {
            return _datacontext.Appointments.FirstOrDefault(_ => _.Id == id);
        }

        public void Delete(Appointment appointment)
        {
            _datacontext.Appointments.Remove(appointment);
        }

        public bool Isvisited(int doctorId, int patientId, DateTime date)
        {
            return _datacontext.Appointments.Any(_ => _.PatientId == patientId && _.DoctorId == doctorId && _.Date == date);
        }

        public int Appointmentcount(int doctorId, DateTime date)
        {
            return _datacontext.Appointments.Where(_ => _.DoctorId == doctorId && _.Date == date).Count();
        }
    }
}
