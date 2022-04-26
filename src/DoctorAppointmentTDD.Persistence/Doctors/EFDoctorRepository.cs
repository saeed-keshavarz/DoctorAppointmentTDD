using DoctorAppointmentTDD.Entities;
using DoctorAppointmentTDD.Service.Doctors.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentTDD.Persistence.EF.Doctors
{
    public class EFDoctorRepository : DoctorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EFDoctorRepository(ApplicationDbContext dbcontext)
        {
            _dbContext =dbcontext;
        }

        public void Add(Doctor doctor)
        {
            _dbContext.Doctors.Add(doctor);
        }

        public List<GetDoctorDto> GetAll()
        {
            return _dbContext.Doctors.Select(_ => new GetDoctorDto
            {
                Id = _.Id,
                LastName = _.LastName,
                FirstName = _.FirstName,
                NationalCode = _.NationalCode,
                Field = _.Field,
            }).ToList();
        }

        public GetDoctorDto GetByDto(int id)
        {
            return _dbContext.Doctors
             .Select(_ => new GetDoctorDto
             {
                 Id = _.Id,
                 LastName = _.LastName,
                 FirstName = _.FirstName,
                 NationalCode = _.NationalCode,
                 Field = _.Field,
             }).FirstOrDefault(_ => _.Id == id);
        }

        public Doctor GetById(int id)
        {
            return _dbContext.Doctors
                .Include(_ => _.Appointments)
                .FirstOrDefault(_ => _.Id == id);
        }


        public void Delete(Doctor doctor)
        {
            _dbContext.Remove(doctor);
        }

        public bool IsExistNationalCode(string nationalCode)
        {
            return _dbContext.Doctors.Any(_ => _.NationalCode == nationalCode);
        }
    }
}
