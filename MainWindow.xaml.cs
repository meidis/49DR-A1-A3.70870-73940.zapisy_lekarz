using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Zadanie_4.Enums;
using Zadanie_4.Helpers;
using Zadanie_4.Models;
using Zadanie_4.Services;
using Zadanie_4.Ui;

namespace Zadanie_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ObservableCollection<Appointment> _currentAppointments;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;
        private GridViewColumnHeader _lastHeaderClicked;

        public MainWindow()
        {
            InitializeComponent();
            this._currentAppointments = new ObservableCollection<Appointment>();
            this.GetCurrentAppointments();
        }


        private void GetCurrentAppointments()
        {
            var appointmentService = new AppointmentsService();
            var specialistService = new SpecialistsService();
            var allCurrentAppointments = appointmentService.GetAllSchedules();

            var retVal = (from appointment in allCurrentAppointments
                let specialist = specialistService.GetSingleSpecialist(appointment.SpecialistId)
                select new AppointmentRowDto
                {
                    Id = appointment.Id, StartDate = appointment.StartDate, EndDate = appointment.EndDate,
                    FullName = specialist.FullName,
                    Specialization = EnumTranslator.TranslateSpecializationEnum
                        (Enum.GetName(typeof(Specialization), specialist.Specialization))
                }).ToList();

            this.AllAppointments.ItemsSource = retVal;
        }

        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is GridViewColumnHeader headerClicked)) return;
            if (headerClicked.Role == GridViewColumnHeaderRole.Padding) return;

            ListSortDirection direction;
            if (headerClicked != this._lastHeaderClicked)
                direction = ListSortDirection.Ascending;
            else
            {
                direction = this._lastDirection == ListSortDirection.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;
            }

            var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
            var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

            this.Sort(sortBy, direction);

            this._lastHeaderClicked = headerClicked;
            this._lastDirection = direction;
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            var dataView =
                CollectionViewSource.GetDefaultView(this.AllAppointments.ItemsSource);

            dataView.SortDescriptions.Clear();
            var sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private void OnAddSpecialistClick(object sender, RoutedEventArgs e)
        {
            var addNewSpecialistWindow = new AddNewSpecialist();
            addNewSpecialistWindow.ShowDialog();
        }

        private void AddNewAppointment_OnClick(object sender, RoutedEventArgs e)
        {
            var addNewAppointmentWindow = new AddNewAppointment();
            addNewAppointmentWindow.ShowDialog();
            this.GetCurrentAppointments();
        }

        private void DeleteAppointment_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedRow = this.AllAppointments.SelectedItem as AppointmentRowDto;
            
            if (selectedRow == null) return;

            var appointmentService = new AppointmentsService();
            
            appointmentService.DeleteAppointment(selectedRow.Id);
            
            this.GetCurrentAppointments();
        }
    }
}