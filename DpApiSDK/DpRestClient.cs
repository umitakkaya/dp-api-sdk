using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Authenticators;
using DpApiSDK.Representation;
using DpApiSDK.Exceptions;
using System.Net;

namespace DpApiSDK
{
    public sealed class DpRestClient
    {
        public const string DATE_FORMAT     = Globals.DATE_FORMAT;
        public const string DATETIME_FORMAT = Globals.DATETIME_FORMAT;

        private const string GRANT_TYPE     = "client_credentials";
        private const string SCOPE          = "integration";
        private const string TOKEN_ENDPOINT = "oauth/v2/token";
        private const string PREFIX         = "api/v3/integration";
        private const string TOKEN_TYPE     = "Bearer";

        private readonly string[] BOOKING_SCOPES = { "booking.patient", "booking.address_service" };

        private string _clientId;
        private string _clientSecret;
        private RestClient _client;

        public DpRestClient(string clientId, string clientSecret, Locale locale)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _client = new RestClient(LocaleSettings.GetBaseUrl(locale));

            SetupSerializer();
            AddDefaultHeaders();
            CheckAndGetToken();
        }

        public List<ServiceItem> GetServiceItems()
        {
            var request  = CreateRequest("/services");
            var response = _client.Execute<DpCollection<ServiceItem>>(request);

            return response.Data.Items;
        }

        public List<DpFacility> GetFacilities()
        {
            var request  = CreateRequest("/facilities");
            var response = _client.Execute<DpCollection<DpFacility>>(request);

            return response.Data.Items;
        }

        public DpFacility GetFacility(string facilityId)
        {
            var request  = CreateRequest("/facilities/{facilityId}");
            request.AddUrlSegment("facilityId", facilityId);

            var response = _client.Execute<DpFacility>(request);

            return response.Data;
        }

        public List<DpDoctor> GetDoctors(string facilityId)
        {
            var request  = CreateRequest("/facilities/{facilityId}/doctors");
            request.AddUrlSegment("facilityId", facilityId);

            var response = _client.Execute<DpCollection<DpDoctor>>(request);

            return response.Data.Items;
        }

        public DpDoctor GetDoctor(string facilityId, string doctorId)
        {
            var request  = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}");
            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);

            var response = _client.Execute<DpDoctor>(request);

            return response.Data;
        }

        public List<DpAddress> GetAddresses(string facilityId, string doctorId)
        {
            var request  = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses");

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);

            var response = _client.Execute<DpCollection<DpAddress>>(request);

            return response.Data.Items;
        }

        public DpAddress GetAddress(string facilityId, string doctorId, string addressId)
        {
            var request  = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}");

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);

            var response = _client.Execute<DpAddress>(request);

            return response.Data;
        }

        /// <summary>
        /// Get all services for an address
        /// </summary>
        public List<AddressService> GetAddressServices(string facilityId, string doctorId, string addressId)
        {
            return GenericGetAddressServices(facilityId, doctorId, addressId);
        }

        /// <summary>
        /// Get list of address services assigned for a slot
        /// </summary>
        /// <param name="slotDateTime">Exact date and time of the slot</param>
        public List<AddressService> GetAddressServices(string facilityId, string doctorId, string addressId, DateTimeOffset slotDateTime)
        {
            return GenericGetAddressServices(facilityId, doctorId, addressId, slotDateTime);
        }

        public AddressService GetAddressService(string facilityId, string doctorId, string addressId, string addressServiceId)
        {
            var request  = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/services/{addressServiceId}");

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);
            request.AddUrlSegment("addressServiceId", addressServiceId);

            var response = _client.Execute<AddressService>(request);

            return response.Data;
        }

        public AddressService AddAddressService(string facilityId, string doctorId, string addressId, string serviceItemId, int price, bool isPriceFrom)
        {
            var request  = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/services", Method.POST);

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);

            request.AddJsonBody(new { ServiceId = serviceItemId, Price = price, IsPriceFrom = isPriceFrom });

            var response = CreateExecute<AddressService>(request);

            return response.Data;
        }

        public bool DeleteAddressService(string facilityId, string doctorId, string addressId, string addressServiceId)
        {
            var request  = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/services/{addressServiceId}", Method.DELETE);

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);
            request.AddUrlSegment("addressServiceId", addressServiceId);

            var response = _client.Execute<AddressService>(request);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public AddressService PatchAddressService(string facilityId, string doctorId, string addressId, string addressServiceId, int price, bool isPriceFrom)
        {
            var request  = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/services/{addressServiceId}", Method.PATCH);

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);
            request.AddUrlSegment("addressServiceId", addressServiceId);

            request.AddJsonBody(new { Price = price, IsPriceFrom = isPriceFrom });

            var response = _client.Execute<AddressService>(request);

            return response.Data;
        }

        public DpCollection<CalendarBreak> GetCalendarBreaks(string facilityId, string doctorId, string addressId, DateTimeOffset start, DateTimeOffset end)
        {
            var request  = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/breaks");

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);

            request.AddQueryParameter("since", start.ToString(DATETIME_FORMAT));
            request.AddQueryParameter("till", end.ToString(DATETIME_FORMAT));

            var response = _client.Execute<DpCollection<CalendarBreak>>(request);

            return response.Data;
        }

        public CalendarBreak GetCalendarBreak(string facilityId, string doctorId, string addressId, string calendarBreakId)
        {
            var request  = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/breaks/{calendarBreakId}");

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);
            request.AddUrlSegment("calendarBreakId", calendarBreakId);

            var response = _client.Execute<CalendarBreak>(request);

            return response.Data;
        }

        public CalendarBreak AddCalendarBreak(string facilityId, string doctorId, string addressId, DateTimeOffset since, DateTimeOffset till)
        {
            var request  = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/breaks", Method.POST);

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);

            var jsonBody = new
            {
                Since = since,
                Till  = till
            };

            request.AddJsonBody(jsonBody);

            var response = CreateExecute<CalendarBreak>(request);

            return response.Data;
        }

        public bool DeleteCalendarBreak(string facilityId, string doctorId, string addressId, string calendarBreakId)
        {
            var request  = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/breaks/{calendarBreakId}", Method.DELETE);

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);
            request.AddUrlSegment("calendarBreakId", calendarBreakId);

            var response = _client.Execute<CalendarBreak>(request);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public DpCollection<Slot> GetSlots(string facilityId, string doctorId, string addressId, DateTimeOffset start, DateTimeOffset end)
        {
            var request = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/slots");

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);

            request.AddQueryParameter("start", start.ToString(DATETIME_FORMAT));
            request.AddQueryParameter("end", end.ToString(DATETIME_FORMAT));

            var response = _client.Execute<DpCollection<Slot>>(request);

            return response.Data;
        }

        /// <summary>
        /// Deletes slots in the specified date.
        /// </summary>
        /// <param name="date">Time portion of this param has no effect</param>
        /// <returns></returns>
        public bool DeleteSlots(string facilityId, string doctorId, string addressId, DateTime date)
        {
            var request = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/slots/{date}", Method.DELETE);

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);
            request.AddUrlSegment("date", date.ToString(DATE_FORMAT));

            var response = _client.Execute<BaseResponse>(request);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public Booking BookSlot(string facilityId, string doctorId, string addressId, DateTimeOffset start, BookingRequest bookingRequest)
        {
            var request = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/slots/{start}/book", Method.POST);

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);
            request.AddUrlSegment("start", start.ToString(DATETIME_FORMAT));

            request.AddJsonBody(bookingRequest);

            var response = _client.Execute<Booking>(request);

            return response.Data;
        }

        /// <summary>
        /// Override schedules for the specified dates
        /// For detailed explanation visit: http://znanylekarz.github.io/integrations-api-docs/v3/#slots-replace-slots
        /// </summary>
        public bool ReplaceSlots(string facilityId, string doctorId, string addressId, ReplaceSlotsRequest replaceSlotsRequest)
        {
            var request = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/slots", Method.PUT);

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);

            request.AddJsonBody(replaceSlotsRequest);

            var response = _client.Execute<BaseResponse>(request);

            return response.StatusCode == HttpStatusCode.Created;
        }


        public DpCollection<Booking> GetBookings(string facilityId, string doctorId, string addressId, DateTimeOffset start, DateTimeOffset end)
        {
            var request = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/bookings");

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);

            request.AddQueryParameter("start", start.ToString(DATETIME_FORMAT));
            request.AddQueryParameter("end", end.ToString(DATETIME_FORMAT));
            request.AddQueryParameter("with", string.Join(",", BOOKING_SCOPES));

            var response = _client.Execute<DpCollection<Booking>>(request);

            return response.Data;
        }

        public bool CancelVisit(string facilityId, string doctorId, string addressId, string visitId)
        {
            var request = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/bookings/{visitId}", Method.DELETE);

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);
            request.AddUrlSegment("visitId", visitId);

            var response = _client.Execute<BaseResponse>(request);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public Booking MoveVisit(string facilityId, string doctorId, string addressId, string visitId, string addressServiceId, DateTimeOffset target)
        {
            var request = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/bookings/{visitId}", Method.POST);

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);
            request.AddUrlSegment("visitId", visitId);

            var jsonBody = new
            {
                Start = target,
                AddressServiceId = addressServiceId
            };

            request.AddJsonBody(jsonBody);

            var response = CreateExecute<Booking>(request);

            return response.Data;
        }

        /// <summary>
        /// Report that a patient showed up to a visit.
        /// </summary>
        /// <param name="facilityId"></param>
        /// <param name="doctorId"></param>
        /// <param name="addressId"></param>
        /// <param name="visitId"></param>
        /// <param name="isPatientPresent">If patient came to visit true, otherwise false</param>
        /// <returns>if successfully reported returns true, otherwise means reporting failed</returns>
        public bool ReportPresence(string facilityId, string doctorId, string addressId, string visitId, bool isPatientPresent)
        {
            var method = isPatientPresent ? Method.POST : Method.DELETE;

            var request = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/bookings/{visitId}/presence/presence", method);

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);
            request.AddUrlSegment("visitId", visitId);

            var response = _client.Execute<BaseResponse>(request);

            return response.StatusCode == HttpStatusCode.NoContent;
        }


        public Notification GetNotification()
        {
            var request = CreateRequest("/notifications");
            var response = _client.Execute<Notification>(request);

            return response.Data;
        }

        /// <summary>
        /// This function will fetch notifications until there is no notification left
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Notification> GetNotifications()
        {
            var request = CreateRequest("/notifications");

            IRestResponse<Notification> response;
            while ((response = _client.Execute<Notification>(request)).StatusCode != HttpStatusCode.NotFound)
            {
                yield return response.Data;
            }
        }

        public Notification ParseNotification(string notificationJson)
        {
            return JsonConvert.DeserializeObject<Notification>(notificationJson);
        }


        private List<AddressService> GenericGetAddressServices(string facilityId, string doctorId, string addressId, DateTimeOffset? slotDateTime = null)
        {
            var request  = CreateRequest("/facilities/{facilityId}/doctors/{doctorId}/addresses/{addressId}/services");

            request.AddUrlSegment("facilityId", facilityId);
            request.AddUrlSegment("doctorId", doctorId);
            request.AddUrlSegment("addressId", addressId);

            if (slotDateTime.HasValue)
            {
                request.AddQueryParameter("start", slotDateTime.Value.ToString(DATETIME_FORMAT));
            }

            var response = _client.Execute<DpCollection<AddressService>>(request);

            return response.Data.Items;
        }

        private IRestResponse<T> CreateExecute<T>(RestRequest request) where T : new()
        {
            var response = _client.Execute<T>(request);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                return response;
            }

            var locationHeader = response.Headers.SingleOrDefault(h => h.Type == ParameterType.HttpHeader && h.Name == "Location");

            if (locationHeader == null)
            {
                return response;
            }

            string resourceUrl = locationHeader.Value.ToString();

            var newRequest = CreateRequestWithAbsoluteUrl(resourceUrl);

            return _client.Execute<T>(newRequest);
        }


        private RestRequest CreateRequestWithAbsoluteUrl(string absoluteResourceUrl, Method method = Method.GET)
        {
            int resourceStartIndex = absoluteResourceUrl.IndexOf(PREFIX) + PREFIX.Length;
            string normalizedUrl   = absoluteResourceUrl.Substring(resourceStartIndex);

            return CreateRequest(normalizedUrl, method, PREFIX);
        }

        private RestRequest CreateRequest(string resource, Method method = Method.GET, string endpoint = PREFIX)
        {
            if(endpoint != TOKEN_ENDPOINT)
            {
                CheckAndGetToken();
            }
                
            var serializer = new RestSharp.Newtonsoft.Json.NewtonsoftJsonSerializer(JsonSerializer.CreateDefault());
            var request = new RestRequest(endpoint + resource, method)
            {
                DateFormat     = DATETIME_FORMAT,
                JsonSerializer = serializer
            };

            return request;
        }

        private void GetToken()
        {
            var request = CreateRequest(string.Empty, Method.POST, TOKEN_ENDPOINT);

            request.AddParameter("client_id", _clientId);
            request.AddParameter("client_secret", _clientSecret);
            request.AddParameter("grant_type", GRANT_TYPE);
            request.AddParameter("scope", SCOPE);

            var tokenResponse = _client.Post<AuthorizationToken>(request);

            AuthorizationToken token = tokenResponse.Data;

            if (token == null || token.AccessToken == null || token.Error != null)
            {
                throw new AuthenticationException();
            }

            Globals.SetToken(_clientId, token);

            _client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token.AccessToken, TOKEN_TYPE);
        }

        private void CheckAndGetToken()
        {
            var token = Globals.GetToken(_clientId);
            if (token != null && token.ExpiresAt > DateTime.Now)
            {
                return;
            }

            GetToken();
        }

        private void SetupSerializer()
        {
            JsonConvert.DefaultSettings = () => Globals.SerializerSettings;
        }

        private void AddDefaultHeaders()
        {
            _client.AddDefaultHeader("Content-Type", "application/json; charset=utf-8");
            _client.AddDefaultHeader("Accept", "application/json");
        }
    }
}
