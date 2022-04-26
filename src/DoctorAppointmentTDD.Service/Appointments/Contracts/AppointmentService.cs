using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentTDD.Service.Appointments.Contracts
{
    public interface AppointmentService
    {
        void Add(AddAppointmentDto dto);

        List<GetAppointmentDto> GetAll();

        GetAppointmentDto GetByDto(int id);

        void Update(int id, UpdateAppointmentDto dto);

        void Delete(int id);
    }
}
