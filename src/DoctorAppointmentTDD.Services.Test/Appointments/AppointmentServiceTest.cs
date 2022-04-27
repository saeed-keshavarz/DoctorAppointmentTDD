using DoctorAppointmentTDD.Infrastructur.Test;
using DoctorAppointmentTDD.Persistence.EF;
using DoctorAppointmentTDD.Persistence.EF.Appointments;
using DoctorAppointmentTDD.Service.Appointments;
using DoctorAppointmentTDD.Service.Appointments.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DoctorAppointmentTDD.Services.Test.Appointments
{
    public class AppointmentServiceTest
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly EFUnitOfWork _unitOfWork;
        private readonly AppointmentRepository _repository;
        private readonly AppointmentService _sut;

        public AppointmentServiceTest()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<ApplicationDbContext>();

            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFAppointmentRepository(_dataContext);
            _sut = new AppointmentAppService(_repository, _unitOfWork);
        }


    }
}
