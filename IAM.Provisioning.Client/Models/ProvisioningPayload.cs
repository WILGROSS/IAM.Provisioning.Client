using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.Provisioning.Client.Models
{
    public classs ProvisioningPayload
    {
        // G-krav: Läs kravspecen, finns alla fält som ska skickas till API:t med här?
        public string name { get; set; }
        public string email { get; set; }
    }
}
