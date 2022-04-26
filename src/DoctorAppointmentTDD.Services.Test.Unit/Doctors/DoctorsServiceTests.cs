using DoctorAppointmentTDD.Entities;
using DoctorAppointmentTDD.Infrastructur.Application;
using DoctorAppointmentTDD.Infrastructur.Test;
using DoctorAppointmentTDD.Persistence.EF;
using DoctorAppointmentTDD.Persistence.EF.Doctors;
using DoctorAppointmentTDD.Service.Doctors;
using DoctorAppointmentTDD.Service.Doctors.Contracts;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DoctorAppointmentTDD.Services.Test.Unit.Doctors
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
        public void GetAll_returns_all_doctors()
        {
            CreateDoctorsInDataBase();

            var expected = _sut.GetAll();

            expected.Should().HaveCount(3);
            expected.Should().Contain(_ => _.FirstName == "dummy1");
            expected.Should().Contain(_ => _.FirstName == "dummy2");
            expected.Should().Contain(_ => _.FirstName == "dummy3");
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
                NationalCode = "228",
                Field = "dummyField",
            };
        }

    }
}
