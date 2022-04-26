using DoctorAppointmentTDD.Entities;
using DoctorAppointmentTDD.Infrastructur.Application;
using DoctorAppointmentTDD.Service.Appointments.Contracts;
using DoctorAppointmentTDD.Service.Appointments.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentTDD.Service.Appointments
{
    public class AppointmentAppService : AppointmentService
    {
        private readonly AppointmentRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public AppointmentAppService(
            AppointmentRepository appointmentRepository,
            UnitOfWork unitOfWork)
        {
            _repository = appointmentRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddAppointmentDto dto)
        {
            var appointment = new Appointment
            {
                Date = dto.Date,
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
            };

            var alreadyExists = _repository
                .Isvisited(appointment.DoctorId, appointment.PatientId, appointment.Date);

            var appointmentcountIsFull = _repository
                .Appointmentcount(appointment.DoctorId, appointment.Date);

            if (alreadyExists)
            {
                throw new AlreadyExists();
            }

            if (appointmentcountIsFull > 4)
            {
                throw new AppointmentCountIsFull();
            }

            _repository.Add(appointment);
            _unitOfWork.Commit();

        }

        public List<GetAppointmentDto> GetAll()
        {
            return _repository.GetAll();
        }

        public GetAppointmentDto GetByDto(int id)
        {
            return _repository.GetAppointmentDto(id);
        }

        public void Update(int id, UpdateAppointmentDto dto)
        {
            var appointment = _repository.GetById(id);

            var alreadyExists = _repository
             .Isvisited(dto.DoctorId, dto.PatientId, dto.Date);

            if (alreadyExists)
            {
                throw new AlreadyExists();
            }

            if (appointment.DoctorId != dto.DoctorId)
            {
                var appointmentcountIsFull = _repository
                               .Appointmentcount(dto.DoctorId, dto.Date);

                if (appointmentcountIsFull > 4)
                {
                    throw new AppointmentCountIsFull();
                }
            }

            appointment.Date = dto.Date;
            appointment.DoctorId = dto.DoctorId;
            appointment.PatientId = dto.PatientId;

            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var appointment = _repository.GetById(id);
            _repository.Delete(appointment);
            _unitOfWork.Commit();
        }
    }
}
