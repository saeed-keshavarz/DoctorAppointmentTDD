using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentTDD.Service.Doctors.Contracts
{
    public interface DoctorService
    {
        void Add(AddDoctorDto dto);

        List<GetDoctorDto> GetAll();

        GetDoctorDto GetByDto(int id);

        void Update(int id, UpdateDoctorDto dto);

        void Delete(int id);

    }
}
