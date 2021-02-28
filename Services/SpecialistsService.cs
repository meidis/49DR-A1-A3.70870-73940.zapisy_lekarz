using System;
using System.Collections.Generic;
using System.Linq;
using Zadanie_4.Enums;
using Zadanie_4.Models;
using Zadanie_4.Repositories;

namespace Zadanie_4.Services
{
    public class SpecialistsService
    {
        private readonly LocalDbRepository<Specialist> _localDbRepository;
        
        public SpecialistsService()
        {
            this._localDbRepository = new LocalDbRepository<Specialist>();
        }

        public void AddNewSpecialist(Specialist specialist)
        {
            this._localDbRepository.Add(specialist);
        }

        public IEnumerable<Specialist> GetAllSpecialists()
        {
            return this._localDbRepository.GetAll();
        }

        public IEnumerable<Specialist> GetSpecialistsByLocationAndSpecialization(Location location, Specialization specialization)
        {
            return this._localDbRepository.GetAll().Where(x => x.Location == location && x.Specialization == specialization);
        }

        public Specialist GetSingleSpecialist(Guid id)
        {
            return this._localDbRepository.GetById(id);
        }
    }
}