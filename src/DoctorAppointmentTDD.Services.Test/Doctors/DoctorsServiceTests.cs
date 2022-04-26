using DoctorAppointmentTDD.Entities;
using DoctorAppointmentTDD.Infrastructur.Application;
using DoctorAppointmentTDD.Infrastructur.Test;
using DoctorAppointmentTDD.Persistence.EF;
using DoctorAppointmentTDD.Persistence.EF.Doctors;
using DoctorAppointmentTDD.Service.Appointments.Exceptions;
using DoctorAppointmentTDD.Service.Doctors;
using DoctorAppointmentTDD.Service.Doctors.Contracts;
using DoctorAppointmentTDD.Service.Doctors.Exceptions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            AddDoctorDto dto = GenerateAddDoctorDto();

            _sut.Add(dto);

            _dataContext.Doctors.Should()
                .Contain(_ => _.FirstName == dto.FirstName);
        }

        [Fact]
        public void Add_throw_DoctorNationalCodeAlreadyExistException_when_add_new_doctor()
        {

            var doctor = CreateDoctor("2380132933");
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));
            AddDoctorDto dto = GenerateAddDoctorDto();          
            Action expected =()=> _sut.Add(dto);
            expected.Should().Throw<DoctorAlreadyExistException>();

        }

        [Fact]
        public void GetAll_returns_all_doctors()
        {
            CreateDoctorsInDataBase();

            var expected = _sut.GetAll();

            expected.Should().HaveCount(3);
            expected.Should().Contain(_ => _.FirstName == "dummy1");
            expected.Should().Contain(_ => _.FirstName == "dummy2");
            expected.Should().Contain(_ => _.FirstName == "dummy3");
        }

        [Fact]
        public void Update_update_doctor_properly()
        {
            var doctor = CreateDoctor("2380132933");
            _dataContext.Manipulate(_ => _.Doctors.Add(doctor));
            UpdateDoctorDto dto = GenerateUpdateDoctorDto("editedName");

            _sut.Update(doctor.Id, dto);

            var expected = _dataContext.Doctors
                .FirstOrDefault(_ => _.Id == doctor.Id);

            expected.FirstName.Should().Be(dto.FirstName);


        }

        [Fact]
        public void Update_throw_DoctorNotFoundException_when_Doctor_with_given_id_is_not_exist()
        {
            var dummyDoctorId = 1000;
            var dto = GenerateUpdateDoctorDto("EditedName");
            Action expected = () => _sut.Update(dummyDoctorId, dto);
            expected.Should().ThrowExactly<DoctorNotFoundException>();

        }


        private static UpdateDoctorDto GenerateUpdateDoctorDto(string firstName)
        {
            return new UpdateDoctorDto
            {
                FirstName = firstName,
                LastName = "editeddummy",
                NationalCode = "2380132933",
                Field = "editeddummyfield"
            };
        }

        private static Doctor CreateDoctor(string nationalCode)
        {
            return new Doctor
            {
                FirstName = "dummyName",
                LastName = "dummyLastname",
                NationalCode = nationalCode,
                Field = "dummyfield",

            };
        }

        private void CreateDoctorsInDataBase()
        {
            var doctors = new List<Doctor>
            {
                new Doctor { FirstName = "dummy1", LastName="dummy1",NationalCode="2380132933", Field="dummy1"},
                new Doctor { FirstName = "dummy2", LastName="dummy2",NationalCode="2380257515", Field="dummy2"},
                new Doctor { FirstName = "dummy3", LastName="dummy4",NationalCode="2380132934", Field="dummy3"},
            };
            _dataContext.Manipulate(_ =>
            _.Doctors.AddRange(doctors));
        }


        private static AddDoctorDto GenerateAddDoctorDto()
        {
            return new AddDoctorDto
            {
                FirstName = "dummyFirst",
                LastName = "dummyLast",
                NationalCode = "2380132933",
                Field = "dummyField",
            };
        }

    }
}
