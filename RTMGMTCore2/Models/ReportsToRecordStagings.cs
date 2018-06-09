using System;
using System.Collections.Generic;

namespace RTMGMTCore2.Models
{
    public partial class ReportsToRecordStagings
    {
        public int Id { get; set; }
        public string ReportingId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string ReportsToId { get; set; }
        public string EmployeeId { get; set; }
    }
}
