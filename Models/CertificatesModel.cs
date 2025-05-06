using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barangay_Office.Models
{
    public class CertificatesModel
    {
        public string Title { get; set; } = string.Empty;
        public Stream CertificateImage { get; set; } = Stream.Null;
    }

    public static class CertificateStorage
    {
        //centralized storage for certificates
        public static ObservableCollection<CertificatesModel> Certificates { get; } = [];
    }
}
