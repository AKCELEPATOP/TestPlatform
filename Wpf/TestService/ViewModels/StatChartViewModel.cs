using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestService.ViewModels
{
    [DataContract]
    public class StatChartViewModel
    {
        [DataMember]
        public List<double> Results { get; set; }

        [DataMember]
        public List<string> Dates { get; set; }

        [DataMember]
        public List<string> TestName { get; set; }
    }
}
