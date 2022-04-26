using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentTDD.Service.Patients.Contracts
{
    public interface PatientService
    {
        void Add(AddPatientDto dto);

        GetPatientDto GetByDto(int id);

        List<GetPatientDto> GetAll();

        void Update(UpdatePatientDto dto, int id);

        void Delete(int id);
    }
}
