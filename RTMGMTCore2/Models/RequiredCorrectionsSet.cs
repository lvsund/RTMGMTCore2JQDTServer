using System;
using System.Collections.Generic;

namespace RTMGMTCore2.Models
{
    public partial class RequiredCorrectionsSet
    {
        public int Id { get; set; }
        public string ReportingId { get; set; }
        public string ReportsToId { get; set; }
    }
}
