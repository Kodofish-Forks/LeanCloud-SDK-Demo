using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.LeanCloud.WindowsForms
{
    public class ContactService
    {
        /// <summary>
        /// Gets the contact list.
        /// </summary>
        /// <param name="clientClientId">The client client identifier.</param>
        /// <returns></returns>
        public static List<string> GetContacts(string clientClientId)
        {
            var defaultUser = new List<string> { "Kodofish", "Andy", "Neo", "Jerry" };
            return defaultUser.Where(it => it.ToLower() != clientClientId.ToLower()).ToList();
        }
    }
}
