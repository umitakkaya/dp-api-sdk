using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Representation
{
    public class Notification : BaseResponse
    {
        public string Name { get; set; }
        public NotificationData Data { get; set; }
        public string CreatedAt { get; set; }
    }
}
