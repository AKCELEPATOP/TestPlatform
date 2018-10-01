using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestService.BindingModels
{
    [DataContract]
    public class GetListModel
    {
        [DataMember]
        public int Skip { get; set; }

        [DataMember]
        public int Take { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime? DateFrom { get; set; }

        [DataMember]
        public DateTime? DateTo { get; set; }
    }
}
