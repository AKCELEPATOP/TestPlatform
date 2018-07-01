using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    [DataContract]
    public class PatternCategory
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Complex { get; set; }

        [DataMember]
        public int Middle { get; set; }

        [DataMember]
        public int Easy { get; set; }

        [DataMember]
        public int PatternId { get; set; }

        [DataMember]
        public int CategoryId { get; set; }

        public virtual Pattern Pattern  { get; set; }

        public virtual Category Category { get; set; }
    }
}
