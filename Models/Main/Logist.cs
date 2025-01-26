using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Main
{
    public class Logist : BaseEntity
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public short PasswordState { get; set; }
        public bool IsExpired { get; set; }
    }
}
