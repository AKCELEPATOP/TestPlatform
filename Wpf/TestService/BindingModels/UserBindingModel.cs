using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestService.BindingModels
{
    [DataContract]
    public class UserBindingModel : IdentityUser
    {
        [DataMember]
        public override string Id { get; set; }

        [DataMember]
        public string FIO { get; set; }

        [DataMember]
        public override string UserName { get; set; }

        [DataMember]
        public override string PasswordHash { get; set; }

        [DataMember]
        public int GroupId { get; set; }
    }
}
