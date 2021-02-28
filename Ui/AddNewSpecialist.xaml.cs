using System;
using System.Linq;
using System.Windows;
using Zadanie_4.Enums;
using Zadanie_4.Helpers;
using Zadanie_4.Models;
using Zadanie_4.Services;

namespace Zadanie_4.Ui
{
    public partial class AddNewSpecialist : Window
    {
        public AddNewSpecialist()
        {
            this.InitializeComponent();
            this.GetSpecializationOptions();
            this.GetLocalizationOptions();
        }

        private void GetLocalizationOptions()
        {
            this.Specializations.ItemsSource = EnumMapper.MapSpecializationEnum();
        }

        private void GetSpecializationOptions()
        {
            this.Localizations.ItemsSource = EnumMapper.MapLocalizationsEnum();
        }

        private void AddNewSpecialistOnClick(object sender, RoutedEventArgs e)
        {
            var fullName = this.FullName.Text;
            var localization = this.Localizations.SelectedIndex;
            var specialization = this.Specializations.SelectedIndex;

            if (!this.ValidateData(fullName, localization, specialization, out var errorMessage))
            {
                MessageBox.Show(errorMessage);
                return;
            }

            
            this.AddNewSpecialistEntity(fullName, localization, specialization);
        }

        private void AddNewSpecialistEntity(string fullName, int localization, int specialization)
        {
            var specialistsService = new SpecialistsService();
            var specialist = new Specialist
            {
                Id = Guid.NewGuid(),
                FullName = fullName,
                Location = (Location) localization,
                Specialization = (Specialization) specialization,
            };

            specialistsService.AddNewSpecialist(specialist);
            MessageBox.Show("Specjalista dodany");
            this.ClearForm();
        }

        private void ClearForm()
        {
            this.FullName.Text = "";
            this.Localizations.SelectedIndex = -1;
            this.Specializations.SelectedIndex = -1;
        }


        private bool ValidateData(string fullName, int localization, int specialization, out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrEmpty(fullName))
            {
                errorMessage = "Proszę podać imię i nazwisko specjalisty";
                return false;
            }
            if (localization < 0)
            {
                errorMessage = "Proszę wybrać lokalizację specjalisty";
                return false;
            }
            if (specialization < 0)
            {
                errorMessage = "Proszę podać specjalizację";
                return false;
            }

            return true;
        }
    }
}