using System;
using Zadanie_4.Common;
using Zadanie_4.Enums;
using Zadanie_4.Interfaces;

namespace Zadanie_4.Models
{
    public class Appointment : ILocalDbEntity
    {
        public Guid Id { get; set; }
        
        public Guid SpecialistId { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
    }
}