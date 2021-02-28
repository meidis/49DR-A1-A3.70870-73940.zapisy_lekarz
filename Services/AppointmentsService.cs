using System;
using System.Collections.Generic;
using System.Linq;
using Zadanie_4.Models;
using Zadanie_4.Repositories;

namespace Zadanie_4.Services
{
    public class AppointmentsService
    {
        private readonly LocalDbRepository<Appointment> _localDbRepository;
        
        public AppointmentsService()
        {
            this._localDbRepository = new LocalDbRepository<Appointment>();
        }

        public void AddNewAppointment(Appointment appointment)
        {
            this._localDbRepository.Add(appointment);
        }

        public IEnumerable<Appointment> GetAllSchedules()
        {
            return this._localDbRepository.GetAll();
        }

        public IEnumerable<Appointment> GetAllSchedulesForDoctor(Guid doctorsId)
        {
            return this._localDbRepository.GetAll().Where(x => x.SpecialistId == doctorsId);
        }

        public IEnumerable<Appointment> GetSchedulesInDateRange(DateTime startDate)
        {
            return this._localDbRepository.GetAll().Where
                (x => x.StartDate.Date == startDate.Date);
        }

        public Appointment GetScheduleById(Guid id)
        {
            return this._localDbRepository.GetById(id);
        }

        public void DeleteAppointment(Guid id)
        {
            this._localDbRepository.Delete(id);
        }
    }
}