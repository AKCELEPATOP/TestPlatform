using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestService.BindingModels
{
    [DataContract]
    public class PatternCategoriesBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Copmlex { get; set; }

        [DataMember]
        public int Middle { get; set; }

        [DataMember]
        public int Easy { get; set; }

        [DataMember]
        public int PatternId { get; set; }

        [DataMember]
        public int CategoryId { get; set; }
    }
}
