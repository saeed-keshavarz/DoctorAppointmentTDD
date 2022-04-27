using DoctorAppointmentTDD.Entities;
using DoctorAppointmentTDD.Infrastructur.Application;
using DoctorAppointmentTDD.Infrastructur.Test;
using DoctorAppointmentTDD.Persistence.EF;
using DoctorAppointmentTDD.Persistence.EF.Patients;
using DoctorAppointmentTDD.Service.Patients;
using DoctorAppointmentTDD.Service.Patients.Contracts;
using DoctorAppointmentTDD.Service.Patients.Exceptions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DoctorAppointmentTDD.Services.Test.Patients
{
    public class PatientServiceTests
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly PatientService _sut;
        private readonly PatientRepository _repository;

        public PatientServiceTests()
        {
            _dataContext =
                   new EFInMemoryDatabase()
                   .CreateDataContext<ApplicationDbContext>();

            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFPatientRepository(_dataContext);
            _sut = new PatientAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_add_patient_properly()
        {
            AddPatientDto dto = GenerateAddPatientDto();
            _sut.Add(dto);
            _dataContext.Patients.Should()
                .Contain(p => p.FirstName == dto.FirstName);
        }

        [Fact]
        public void Add_throw_PatientNationalCodeAlreadyExistException_when_add_new_patient()
        {
            var patient = CreatePatient("2380132933");
            _dataContext.Manipulate(_ => _.Patients.Add(patient));
            AddPatientDto dto = GenerateAddPatientDto();

            Action expected = () => _sut.Add(dto);

            expected.Should().Throw<PatientAlreadyExistException>();
        }

        [Fact]
        public void GetAll_returns_all_patients()
        {
            CreatePatientsInDatabase();

            var expected = _sut.GetAll();

            expected.Should().HaveCount(3);
            expected.Should().Contain(_ => _.FirstName == "dummy1");
            expected.Should().Contain(_ => _.FirstName == "dummy2");
            expected.Should().Contain(_ => _.FirstName == "dummy3");
        }

        [Fact]
        public void Get_return_patient_with_dto()
        {
            var patient = CreatePatient("2380132933");
            _dataContext.Manipulate(_ => _.Patients.Add(patient));

            GetPatientDto expected = _sut.GetByDto(patient.Id);

            expected.NationalCode.Should().Be("2380132933");
        }

        [Fact]
        public void Update_update_patient_properly()
        {
            var patient = CreatePatient("2380132933");
            _dataContext.Manipulate(_ => _.Patients.Add(patient));
            UpdatePatientDto dto = GenerateUpdatePatientDto("editedName");

            _sut.Update(dto, patient.Id);

            var expected = _dataContext.Patients
                .FirstOrDefault(_=>_.Id==patient.Id);

            expected.FirstName.Should().Be(dto.FirstName);

        }

        [Fact]
        public void Update_throw_PatientNationalCodeNotFoundException_when_update_patient()
        {
            var dummyPatientId = 1000;
            var dto = GenerateUpdatePatientDto("editedname");

            Action expected = () => _sut.Update(dto, dummyPatientId);

            expected.Should().ThrowExactly<PatientNotFoundException>();
        }

        [Fact]
        public void Update_throw_PatientNationalCodeAlreadyExistException_when_update_patient()
        {
            var patient = CreatePatient("2380132933");
            _dataContext.Manipulate(_ => _.Patients.Add(patient));

            var patient1 = CreatePatient("2380132936");
            _dataContext.Manipulate(_ => _.Patients.Add(patient1));
            
           
            var dto = GenerateUpdatePatientDto("editName");
            dto.NationalCode = patient.NationalCode;

            Action expected =()=> _sut.Update(dto, patient1.Id);

            expected.Should().ThrowExactly<PatientAlreadyExistException>();
        }

        [Fact]
        public void Delete_delete_patient_properly()
        {
            var patient = CreatePatient("2380132933");
            _dataContext.Manipulate(_ => _.Patients.Add(patient));

            _sut.Delete(patient.Id);

            _dataContext.Patients.Should()
                .NotContain(_=>_.Id==patient.Id);
        }

        [Fact]
        public void Delete_throw_PatientNotFoundException_when_Patient_with_given_id_is_not_exist()
        {
            var dummyPatientId = 1000;

            Action expected = () => _sut.Delete(dummyPatientId);

            expected.Should().ThrowExactly<PatientNotFoundException>();
        }

        private static UpdatePatientDto GenerateUpdatePatientDto(string firstName)
        {
            return new UpdatePatientDto
            {
                FirstName = firstName,
                LastName = "editedDummy",
                NationalCode = "2380132933"
            };
        }

        private static Patient CreatePatient(string nationalCode)
        {
            return new Patient
            {
                FirstName = "dummyName",
                LastName = "dummyLastname",
                NationalCode = nationalCode,
            };
        }

        private static AddPatientDto GenerateAddPatientDto()
        {
            return new AddPatientDto
            {
                FirstName = "dummy",
                LastName = "dummy",
                NationalCode = "2380132933",

            };
        }

        private void CreatePatientsInDatabase()
        {
            var patients = new List<Patient>()
            {
                new Patient {FirstName="dummy1", LastName="dummy1", NationalCode="2380132931"},
                new Patient {FirstName="dummy2", LastName="dummy2", NationalCode="2380132932"},
                new Patient {FirstName="dummy3", LastName="dummy3", NationalCode="2380132933"},
            };
            _dataContext.Manipulate(_ =>
            _.Patients.AddRange(patients));
        }
    }
}
