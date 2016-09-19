using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpApiSDK.Representation
{
    public class ReplaceSlotsRequest
    {
        private List<SlotConfiguration> _slots = new List<SlotConfiguration>();
        public List<SlotConfiguration> Slots
        {
            get
            {
                return _slots;
            }
            set
            {
                _slots = value;
            }
        }
    }
}
