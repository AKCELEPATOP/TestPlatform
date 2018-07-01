using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestService.ViewModels
{
    [DataContract]
    public class UserViewModel
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string FIO { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public int? GroupId { get; set; }
    }
}
