using DoctorAppointmentTDD.Entities;
using DoctorAppointmentTDD.Infrastructur.Application;
using DoctorAppointmentTDD.Infrastructur.Test;
using DoctorAppointmentTDD.Persistence.EF;
using DoctorAppointmentTDD.Persistence.EF.Doctors;
using DoctorAppointmentTDD.Service.Doctors;
using DoctorAppointmentTDD.Service.Doctors.Contracts;
using DoctorAppointmentTDD.Service.Doctors.Exceptions;
using DoctorAppointmentTDD.Test.Tools.Doctors;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DoctorAppointmentTDD.Services.Test.Doctors
{
    public class DoctorServiceTests
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly DoctorService _sut;
        private readonly DoctorRepository _repository;

        public DoctorServiceTests()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<ApplicationDbContext>();

            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFDoctorRepository(_dataContext);
            _sut = new DoctorAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_doctor_properly()
        {
            AddDoctorDto dto = DoctorFactory.GenerateAddDoctorDto();

            _sut.Add(dto);

            _dataContext.Doctors.Should()
                .Contain(_ => _.FirstName == dto.FirstName);
        }

        [Fact]
        public void Add_throw_DoctorNationalCodeAlreadyExistException_when_add_new_doctor()
        {

            var doctor =DoctorFactory.CreateDoctor("2380132933");
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));
            AddDoctorDto dto = DoctorFactory.GenerateAddDoctorDto();

            Action expected = () => _sut.Add(dto);

            expected.Should().Throw<DoctorAlreadyExistException>();

        }

        [Fact]
        public void GetAll_returns_all_doctors()
        {
          List<Doctor> doctors = DoctorFactory.CreateDoctorsInDataBase();
            _dataContext.Manipulate(_ =>
            _.Doctors.AddRange(doctors));

            var expected = _sut.GetAll();

            expected.Should().HaveCount(3);
            expected.Should().Contain(_ => _.FirstName == "dummy1");
            expected.Should().Contain(_ => _.FirstName == "dummy2");
            expected.Should().Contain(_ => _.FirstName == "dummy3");
        }

        [Fact]
        public void Get_return_doctor_with_dto()
        {
            var doctor =DoctorFactory.CreateDoctor("2380132933");
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));

           GetDoctorDto expected = _sut.GetByDto(doctor.Id);

            expected.NationalCode.Should().Be("2380132933");
        }

        [Fact]
        public void Update_update_doctor_properly()
        {
            var doctor = DoctorFactory.CreateDoctor("2380132933");
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));
            UpdateDoctorDto dto = DoctorFactory.GenerateUpdateDoctorDto("editedName");

            _sut.Update(doctor.Id, dto);

            var expected = _dataContext.Doctors
                .FirstOrDefault(_ => _.Id == doctor.Id);
            expected.FirstName.Should().Be(dto.FirstName);
        }

        [Fact]
        public void Update_throw_DoctorNotFoundException_when_Doctor_with_given_id_is_not_exist()
        {
            var dummyDoctorId = 1000;
            var dto =DoctorFactory.GenerateUpdateDoctorDto("EditedName");

            Action expected = () => _sut.Update(dummyDoctorId, dto);

            expected.Should().ThrowExactly<DoctorNotFoundException>();

        }

        [Fact]
        public void Update_throw_DoctorNationalCodeAlreadyExistException_when_update_doctor()
        {
            var doctor1 = DoctorFactory.CreateDoctor("2380132932");
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor1));

            var doctor2 = DoctorFactory.CreateDoctor("2380132933");
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor2));

            var dto =DoctorFactory.GenerateUpdateDoctorDto("EditedName");

            Action expected = () => _sut.Update(doctor1.Id, dto);

            expected.Should().ThrowExactly<DoctorAlreadyExistException>();

        }

        [Fact]
        public void Delete_delete_doctor_properly()
        {
            var doctor = DoctorFactory.CreateDoctor("2380132933");
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));

            _sut.Delete(doctor.Id);

            _dataContext.Doctors.Should().
                NotContain(_ => _.Id == doctor.Id);

        }

        [Fact]
        public void Delete_throw_DoctorNotFoundException_when_Doctor_with_given_id_is_not_exist()
        {
            var dummyDoctorId = 1000;

            Action expected = () => _sut.Delete(dummyDoctorId);

            expected.Should().ThrowExactly<DoctorNotFoundException>();
        }
    }
}
