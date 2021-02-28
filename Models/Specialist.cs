using System;
using Zadanie_4.Enums;
using Zadanie_4.Interfaces;

namespace Zadanie_4.Models
{
    public class Specialist : ILocalDbEntity
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public Specialization Specialization { get; set; }
        public Location Location { get; set; }
        
        /// <summary>
        /// This value says how much time one appointment lasts for specific Doctor
        /// </summary>
        public int AppointmentDuration { get; set; }
    }
}