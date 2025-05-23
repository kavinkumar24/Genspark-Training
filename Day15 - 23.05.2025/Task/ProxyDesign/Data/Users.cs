using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyDesign.Models;

namespace ProxyDesign.Data
{
    public class Users
    {
        public static User? GetUserByName(string username, string role)
        {
            var userRoleMapping = new Dictionary<string, Role>(StringComparer.OrdinalIgnoreCase)
        {
            { "kavin", Role.Admin },
            { "ramu", Role.Guest },
            { "somu", Role.User },
            { "siva", Role.User },
            { "karthik", Role.Admin },
            { "raj", Role.Guest }
        };

            if(userRoleMapping.TryGetValue(username, out Role mappedRole))
            {
                if(Enum.TryParse(role, ignoreCase:true, out Role inputRole) && inputRole == mappedRole)
                {
                    return new User(username, inputRole.ToString());
                }
            }

            return null;
        }
    }
}
