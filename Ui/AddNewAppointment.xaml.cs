using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Zadanie_4.Enums;
using Zadanie_4.Helpers;
using Zadanie_4.Models;
using Zadanie_4.Services;

namespace Zadanie_4.Ui
{
    public partial class AddNewAppointment : Window
    {
        private List<Specialist> _currentSpecialists;

        public AddNewAppointment()
        {
            this._currentSpecialists = new List<Specialist>();
            this.InitializeComponent();
            this.GetSpecializationOptions();
            this.GetLocalizationOptions();
            this.OccurenceDate.Minimum = DateTime.Now;
        }
        
        private void GetLocalizationOptions()
        {
            this.Specializations.ItemsSource = EnumMapper.MapSpecializationEnum();
        }

        private void GetSpecializationOptions()
        {
            this.Localizations.ItemsSource = EnumMapper.MapLocalizationsEnum();
        }

        private void GetSpecialists()
        {
            var selectedLocalization = (Location) this.Localizations.SelectedIndex;
            var selectedSpecialization = (Specialization) this.Specializations.SelectedIndex;

            if (selectedLocalization < 0 || selectedSpecialization < 0)
                return;

            var specialistsService = new SpecialistsService();
            var specialists = specialistsService.GetSpecialistsByLocationAndSpecialization
                (selectedLocalization, selectedSpecialization).ToList();

            this._currentSpecialists = specialists;
            this.Specialists.ItemsSource = specialists.Select(x => $"{x.FullName}");
        }

        private void Localizations_OnSelected(object sender, RoutedEventArgs e)
        {
            var selectedValue = this.Localizations.SelectedIndex;

            if (selectedValue > -1)
            {
                this.Specializations.IsEnabled = true;
                this.GetSpecialists();
                return;
            }

            this.Specializations.IsEnabled = false;
        }

        private void Specializations_OnSelected(object sender, RoutedEventArgs e)
        {
            var selectedValue = this.Specializations.SelectedIndex;

            if (selectedValue > -1)
            {
                this.Specialists.IsEnabled = true;
                this.GetSpecialists();
                return;
            }

            this.Specialists.IsEnabled = false;
            this.Specialists.ItemsSource = null;
        }

        private void Specialists_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedValue = this.Specialists.SelectedIndex;
            if (selectedValue > -1)
            {
                this.OccurenceDate.IsEnabled = true;
                return;
            }

            this.OccurenceDate.IsEnabled = false;
        }

        private void OccurenceDate_OnSelectedDateChanged(object sender, RoutedPropertyChangedEventArgs<object> routedPropertyChangedEventArgs)
        {
            var selectedDate = this.OccurenceDate.Value;
            var selectedSpecialistIndex = this.Specialists.SelectedIndex;

            if (selectedSpecialistIndex < 0) return;
            if (!selectedDate.HasValue || selectedDate == DateTime.MinValue ||
                selectedDate == DateTime.MaxValue)
                return;

            var occupiedDatesForSpecialist = this.GetOccupiedDatesForSpecialist((DateTime) selectedDate);
            var appointmentCalculator = new AppointmentCalculator(this._currentSpecialists[selectedSpecialistIndex], occupiedDatesForSpecialist);
            var availableDates = appointmentCalculator.CalculateAvailability((DateTime) selectedDate).Select( x => new AvailableAppointmentRowDto
            {
                StartDate = x.startDate,
                EndDate =  x.endDate
            });
            this.AvailableAppointments.ItemsSource = availableDates;
        }

        private IEnumerable<Appointment> GetOccupiedDatesForSpecialist(DateTime dateTime)
        {
            var appointmentService = new AppointmentsService();

            return appointmentService.GetSchedulesInDateRange(dateTime);
        }

        private void AddAppointment_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedAppointment = this.AvailableAppointments.SelectedItem as AvailableAppointmentRowDto;
            var selectedSpecialist = this.Specialists.SelectedIndex;
            
            
            if (selectedSpecialist < 0 || selectedAppointment == null) return;
            
            var specialist = this._currentSpecialists[selectedSpecialist];

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                StartDate = (DateTime) selectedAppointment.StartDate,
                EndDate = (DateTime) selectedAppointment.EndDate,
                SpecialistId = specialist.Id
            };

            var appointmentService = new AppointmentsService();
            
            appointmentService.AddNewAppointment(appointment);
            MessageBox.Show("Dodano nową wizytę");
        }
    }
}