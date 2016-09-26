using DpApiSDK.Representation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace DpApiSDK
{
    public sealed class Globals
    {
        public const string DATE_FORMAT     = "yyyy-MM-dd";
        public const string DATETIME_FORMAT = "yyyy-MM-ddTHH:mm:sszzz";

        private static object locker = new object();
        private static Dictionary<string, AuthorizationToken> _tokenStorage = new Dictionary<string, AuthorizationToken>();
        public static Dictionary<string, AuthorizationToken> TokenStorage
        {
            get
            {
                lock (locker)
                {
                    return _tokenStorage;
                }
            }
            internal set
            {
                lock (locker)
                {
                    _tokenStorage = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static AuthorizationToken GetToken(string clientId)
        {
            AuthorizationToken token = null;
            _tokenStorage.TryGetValue(clientId, out token);
            return token;
        }

        public static void SetToken(string clientId, AuthorizationToken token)
        {
            if (_tokenStorage.ContainsKey(clientId))
            {
                _tokenStorage[clientId] = token;
            }
            else
            {
                _tokenStorage.Add(clientId, token);
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
