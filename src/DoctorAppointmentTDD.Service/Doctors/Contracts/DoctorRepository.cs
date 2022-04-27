using DoctorAppointmentTDD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentTDD.Service.Doctors.Contracts
{
    public interface DoctorRepository
    {
        void Add(Doctor doctor);

        bool IsExistNationalCode(string nationalCode);

        List<GetDoctorDto> GetAll();

        Doctor GetById(int id);

        GetDoctorDto GetByDto(int id);

        void Delete(Doctor doctor);
        bool IsExistNationalCodeExceptSelf(int id, string nationalCode);
    }
}
