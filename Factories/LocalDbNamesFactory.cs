using System;
using Zadanie_4.Common;
using Zadanie_4.Interfaces;
using Zadanie_4.Models;

namespace Zadanie_4.Factories
{
    public static class LocalDbNamesFactory
    {
        public static string ResolveLocalDbName(Type entityType)
        {
            switch (entityType.Name)
            {
                case nameof(Specialist):
                    return WellKnownConstants.DoctorsDatabase;
                case nameof(Appointment):
                    return WellKnownConstants.AppointmentDatabase;
                default:
                    throw new ArgumentException(@"Couldn't resolve database for entity", nameof(entityType));
            }
        }
    }
}