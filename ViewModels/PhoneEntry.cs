using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.ViewModels
{
    public class PhoneEntry
    {
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";

        public string Display => $"{Name}:{Phone}";
    }
}