using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barangay_Office.Models
{
    public class RegionData
    {
        public List<Province>? Provinces { get; set; }
    }
    public class Province
    {
        public string? ProvinceName { get; set; }
        public List<Municipality>? Municipalities { get; set; }
    }


    public class Municipality
    {
        public string? MunicipalityName { get; set; }
        public List<string>? Barangays { get; set; }
    }


}
