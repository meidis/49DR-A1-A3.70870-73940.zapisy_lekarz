using System;
using System.Collections.Generic;
using System.Linq;
using Zadanie_4.Enums;

namespace Zadanie_4.Helpers
{
    public static class EnumMapper
    {
        public static IEnumerable<string> MapLocalizationsEnum()
        {
            return Enum.GetNames(typeof(Location));
        }
        
        public static IEnumerable<string> MapSpecializationEnum()
        {
            return Enum.GetNames(typeof(Specialization))
                .Select(EnumTranslator.TranslateSpecializationEnum);
        }
    }
}