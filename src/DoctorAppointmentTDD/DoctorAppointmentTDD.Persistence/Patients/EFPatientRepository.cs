using DoctorAppointmentTDD.Entities;
using DoctorAppointmentTDD.Service.Patients.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentTDD.Persistence.EF.Patients
{
    public class EFPatientRepository : PatientRepository
    {
        private readonly DbSet<Patient> _patients;

        public EFPatientRepository(ApplicationDbContext dbcontext)
        {
            _patients = dbcontext.Set<Patient>();
        }

        public void Add(Patient doctor)
        {
            _patients.Add(doctor);
        }

        public List<GetPatientDto> GetAll()
        {
            return _patients.Select(_ => new GetPatientDto
            {
                FirstName = _.FirstName,
                LastName = _.LastName,
                NationalCode = _.NationalCode,
            }).ToList();
        }

        public GetPatientDto GetByDto(int id)
        {
            return _patients.Select(_ => new GetPatientDto
            {
                Id = _.Id,
                FirstName = _.FirstName,
                LastName = _.LastName,
                NationalCode = _.NationalCode,
            }).FirstOrDefault(_ => _.Id == id);
        }

        public Patient GetById(int id)
        {
            return _patients
                .Include(_ => _.Appointments)
                .FirstOrDefault(_ => _.Id == id);
        }

        public void Delete(Patient patient)
        {
            _patients.Remove(patient);
        }

        public bool IsExistNationalCode(string nationalCode)
        {
            return _patients.Any(_ => _.NationalCode == nationalCode);
        }

    }

}
