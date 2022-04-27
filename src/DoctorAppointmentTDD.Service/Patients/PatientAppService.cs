using DoctorAppointmentTDD.Entities;
using DoctorAppointmentTDD.Infrastructur.Application;
using DoctorAppointmentTDD.Service.Patients.Contracts;
using DoctorAppointmentTDD.Service.Patients.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentTDD.Service.Patients
{
    public class PatientAppService : PatientService
    {
        private readonly PatientRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public PatientAppService(
            PatientRepository patientRepository,
            UnitOfWork unitOfWork)
        {
            _repository = patientRepository;
            _unitOfWork = unitOfWork;
        }
        public void Add(AddPatientDto dto)
        {
            var patient = new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                NationalCode = dto.NationalCode,
            };
            var isExistNationalCode = _repository
                .IsExistNationalCode(patient.NationalCode);
            if (isExistNationalCode)
            {
                throw new PatientAlreadyExistException();
            }
            _repository.Add(patient);
            _unitOfWork.Commit();
        }

        public List<GetPatientDto> GetAll()
        {
            return _repository.GetAll();
        }

        public GetPatientDto GetByDto(int id)
        {
            return _repository.GetByDto(id);
        }

        public void Update(UpdatePatientDto dto, int id)
        {
            var patient = _repository.GetById(id);

            PreventToUpdateWhenPatientNotExist(patient);
            var isExistNationalCode = _repository
               .IsExistNationalCodeExceptSelf(id,dto.NationalCode);
            if (isExistNationalCode)
            {
                throw new PatientAlreadyExistException();
            }

            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.NationalCode = dto.NationalCode;

            _unitOfWork.Commit();
        }


        private static void PreventToUpdateWhenPatientNotExist(Patient patient)
        {
            if (patient == null)
            {
                throw new PatientNotFoundException();
            }
        }

        public void Delete(int id)
        {
            var patient = _repository.GetById(id);
            if (patient == null)
            {
                throw new PatientNotFoundException();
            }
            _repository.Delete(patient);
            _unitOfWork.Commit();

        }

    }
}
