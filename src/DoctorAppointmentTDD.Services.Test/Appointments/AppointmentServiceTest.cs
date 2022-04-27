﻿using DoctorAppointmentTDD.Entities;
using DoctorAppointmentTDD.Infrastructur.Test;
using DoctorAppointmentTDD.Persistence.EF;
using DoctorAppointmentTDD.Persistence.EF.Appointments;
using DoctorAppointmentTDD.Service.Appointments;
using DoctorAppointmentTDD.Service.Appointments.Contracts;
using DoctorAppointmentTDD.Test.Tools.Doctors;
using DoctorAppointmentTDD.Test.Tools.Patients;
using FluentAssertions;
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

        [Fact]
        public void Add_add_appointment_properly()
        {
            var doctor = DoctorFactory.CreateDoctor("2380132933");
            _dataContext.Manipulate(_ => _.Add(doctor));

            Patient patient = PatientFactory.CreatePatient("2380257515");
            _dataContext.Manipulate(_ => _.Add(patient));

            var dto = new AddAppointmentDto()
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Date = new DateTime(2022, 04, 27)
            };

            _sut.Add(dto);

            _dataContext.Appointments.Should().Contain(_ => _.DoctorId == dto.DoctorId &&
            _.PatientId == dto.PatientId &&
            _.Date == dto.Date);

        }

    }
}
