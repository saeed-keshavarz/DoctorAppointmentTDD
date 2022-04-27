using DoctorAppointmentTDD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentTDD.Service.Patients.Contracts
{
    public interface PatientRepository
    {
        void Add(Patient doctor);
        bool IsExistNationalCode(string nationalCode);

        List<GetPatientDto> GetAll();

        Patient GetById(int id);

        GetPatientDto GetByDto(int id);

        void Delete(Patient patient);
        bool IsExistNationalCodeExceptSelf(int id, string nationalCode);
    }
}
