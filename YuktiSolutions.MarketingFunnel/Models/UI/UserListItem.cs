using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YuktiSolutions.MarketingFunnel.Models.UI
{
    public class UserListItem
    {
        public String ID { get; set; }

        public String Name { get; set; }

        public String Email { get; set; }

        public String PhoneNumber { get; set; }

        public String Roles { get; set; }

        public Boolean IsActive { get; set; }
    }
}