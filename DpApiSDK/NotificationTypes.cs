using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK
{
    public sealed class NotificationTypes
    {
        public const string SLOT_CHANGED     = "slot-changed";
        public const string SLOT_BOOKED      = "slot-booked";
        public const string SLOT_BOOKING     = "slot-booking";
        public const string BOOKING_CANCELED = "booking-canceled";
        public const string BOOKING_MOVING   = "booking-moving";
        public const string BOOKING_MOVED    = "booking-moved";
        public const string SLOT_OVERBOOKED  = "slot-overbooked";
        public const string BREAK_CREATED    = "break-created";
        public const string BREAK_REMOVED    = "break-removed";

        public static string[] GetEventTypes()
        {
            return new string[]
            {
                SLOT_CHANGED,
                SLOT_BOOKED,
                SLOT_BOOKING,
                BOOKING_CANCELED,
                BOOKING_MOVING,
                BOOKING_MOVED,
                SLOT_OVERBOOKED,
                BREAK_CREATED,
                BREAK_REMOVED
            };
        }
    }
}
