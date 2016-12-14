using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK
{

    public enum Locale
    {
        PL,
        TR,
        IT,
        CZ,
        DE
    }

    public static class LocaleSettings
    {
        private const string PL_URL = "https://www.znanylekarz.pl";
        private const string TR_URL = "https://www.doktortakvimi.com";
        private const string IT_URL = "https://www.docplanner.it";
        private const string CZ_URL = "https://www.znamylekar.cz";
        private const string DE_URL = "https://www.docplanner.de";

        private const string TZ_CENTRAL_EUROPE   = "Central Europe Standard Time";
        private const string TZ_CENTRAL_EUROPEAN = "Central European Standard Time";
        private const string TZ_GTB_STANDART     = "GTB Standard Time";
        private const string TZ_TURKEY_STANDART = "Turkey Standard Time";
        private const string TZ_WESTERN_EUROPE   = "W. Europe Standard Time";
        
        
        public static string GetBaseUrl(Locale locale)
        {
            switch (locale)
            {
                case Locale.PL:
                    return PL_URL;
                case Locale.TR:
                    return TR_URL;
                case Locale.IT:
                    return IT_URL;
                case Locale.CZ:
                    return CZ_URL;
                case Locale.DE:
                    return DE_URL;
                default:
                    throw new ArgumentException("Unkown locale");
            }
        }

        /// <summary>
        /// Get timezone of a locale
        /// </summary>
        /// <param name="locale"></param>
        /// <see cref="https://msdn.microsoft.com/en-us/library/ms912391(v=winembedded.11).aspx"/>
        public static string GetTimezone(Locale locale)
        {
            switch (locale)
            {
                
                case Locale.PL:
                    return TZ_CENTRAL_EUROPEAN;
                case Locale.TR:
                    return TZ_TURKEY_STANDART;
                case Locale.CZ:
                    return TZ_CENTRAL_EUROPE;
                case Locale.DE:
                case Locale.IT:
                    return TZ_WESTERN_EUROPE;
                default:
                    throw new ArgumentException("Unkown locale");
            }
        }
    }
}
