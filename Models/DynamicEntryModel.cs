using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barangay_Office.Models
{
    public class DynamicEntryModel
    {
        public string Label { get; set; } = string.Empty;
        public string PlaceHolder { get; set; } = string.Empty;
        public bool IsVisible { get; set; }
        public string BindingProperty { get; set; } = string.Empty;
        public string InputType { get; set; } = string.Empty;
    }
}
