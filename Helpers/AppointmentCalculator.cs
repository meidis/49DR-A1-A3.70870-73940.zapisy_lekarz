using System;
using System.Collections.Generic;
using System.Linq;
using Zadanie_4.Common;
using Zadanie_4.Models;

namespace Zadanie_4.Helpers
{
    public class AppointmentCalculator
    {
        private readonly Specialist _specialist;
        private readonly IEnumerable<Appointment> _occupiedAppointments;
        private const int MinutesOfDay = 24 * 60;

        public AppointmentCalculator(Specialist specialist, IEnumerable<Appointment> occupiedOccupiedAppointments)
        {
            this._specialist = specialist;
            this._occupiedAppointments = occupiedOccupiedAppointments;
        }

        public IEnumerable<(DateTime startDate, DateTime endDate)> CalculateAvailability(DateTime selectedDate)
        {
            var retVal = new List<(DateTime startDate, DateTime endDate)>();
            var specialistAppointmentInterval = this._specialist.AppointmentDuration == default
                ? WellKnownConstants.DefaultDoctorsAppointmentDuration
                : this._specialist.AppointmentDuration;

            var reservedDates = this._occupiedAppointments.Where
                (x => x.StartDate.Date == selectedDate.Date).ToArray();

            var possibleAppointments = MinutesOfDay / specialistAppointmentInterval;

            var currentDate = selectedDate;
            for (var i = 0; i < possibleAppointments; i++)
            {
                if (reservedDates.Any(x => IsInRange(x.StartDate, x.EndDate, selectedDate)))
                    continue;

                retVal.Add((currentDate, currentDate.AddMinutes(specialistAppointmentInterval)));

                currentDate = currentDate.AddMinutes(specialistAppointmentInterval);
            }

            return retVal;
        }

        private static bool IsInRange(DateTime startDate, DateTime endDate, DateTime selectedDate)
        {
            if (selectedDate < startDate) return false;
            return selectedDate > startDate && selectedDate < endDate;
        }
    }
}