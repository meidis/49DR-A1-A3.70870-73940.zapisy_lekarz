using System;
using Zadanie_4.Enums;

namespace Zadanie_4.Helpers
{
    public static class EnumTranslator
    {
        public static string TranslateSpecializationEnum(string specialization)
        {
            switch (specialization)
            {
                case "Anesthesiologist":
                    return "Anestezjolog";
                case "Dentist":
                    return "Dentysta";
                case "Dermatologist":
                    return "Dermataolog";
                case "Doctor":
                    return "Doktor";
                case "CardiacSurgeon":
                    return "Kardiochirurg";
                case "Cardiologist":
                    return "Kardiolog";
                case "FamilyDoctor":
                    return "Lekarz rodzinny";
                case "GeneralPractitioner":
                    return "Lekarz pierwszego kontaktu";
                case "Geriatrician":
                    return "Geriatra";
                case "Gynaecologist":
                    return "Ginekolog";
                case "Internist":
                    return "Internista";
                case "Laryngologist":
                    return "Laryngolog";
                case "Nephrologist":
                    return "Nefrolog";
                case "Neurologist":
                    return "Neurolog";
                case "Oculist":
                    return "Okulista";
                case "Orthopaedist":
                    return "Ortopeda";
                case "Paediatrician":
                    return "Pediatra";
                case "Psychiatrist":
                    return "Psychiatra";
                case "Surgeon":
                    return "Chirurg";
                default:
                    throw new ArgumentOutOfRangeException(nameof(specialization), specialization, null);
            }
        }
    }
}