using DpApiSDK.Representation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DpApiSDK
{
    public sealed class Globals
    {
        public const string DATE_FORMAT     = "yyyy-MM-dd";
        public const string DATETIME_FORMAT = "yyyy-MM-ddTHH:mm:sszzz";

        private static object locker = new object();
        private static AuthorizationToken _authorizationToken;
        public static AuthorizationToken AuthorizationToken
        {
            get
            {
                lock (locker)
                {
                    return _authorizationToken;
                }
            }
            internal set
            {
                lock (locker)
                {
                    _authorizationToken = value;
                }
            }
        }

        public static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
        {
            DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
            DateParseHandling    = DateParseHandling.DateTimeOffset,
            DateFormatHandling   = DateFormatHandling.IsoDateFormat,
            DateFormatString     = DATETIME_FORMAT,
            ContractResolver     = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy(true, false)
            }
        };
    }
}
