using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.Provisioning.Client.Models
{
    public class UserRecord
    {
        public string Id { get; set }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public bool IsActive { get; set }

        / Metod som ska kombinera firstname + lastname till en och samma sträng, t.ex. "Kalle Anka"
        public string GetFormattedFullName()
        {
            string formattedName = $"";
            return formattedName;
        }

        // G-KRAV: Denna metod måste du uppdatera så den kombinerar firstname + lastname till en e-postaddress, t.ex. "kalle.anka@tssab.com" 
        public string GetFormattedEmail()
        {
            string formattedEmail = 
            return formattedEmail;
        }
    }
}
