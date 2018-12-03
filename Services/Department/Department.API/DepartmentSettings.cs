using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Department.API
{
    public class DepartmentSettings
    {
        public string PicBaseUrl { get; set; }
        public bool UseCustomizationData { get; set; }
        public bool AzureStorageEnabled { get; set; }
    }
}
