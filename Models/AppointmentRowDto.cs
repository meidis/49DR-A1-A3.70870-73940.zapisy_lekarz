using System;
using Zadanie_4.Common;
using Zadanie_4.Enums;

namespace Zadanie_4.Models
{
    public class AppointmentRowDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Specialization { get; set; }
        
        public Status Status => this.ResolveStatus();

        private Status ResolveStatus()
        {
            if (this.StartDate.Date <= DateTime.Now.Date) return Status.Overdue;
            return this.StartDate.AddDays(-WellKnownConstants.DaysToWarn).Date == DateTime.Now.Date ? Status.CloseToOverdue : Status.Active;
        }
    }
}