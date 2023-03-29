using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomerInArrears.Shared
{

    public class CustomerData
    {
        public int TenancyNumber { get; set; }
        public string PropertyId { get; set; }
        public string ClientName { get; set; }
        public string MobileNumber { get; set; }
        public decimal TenancyBalance { get; set; }
    }
}
