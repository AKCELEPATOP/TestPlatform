using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    [DataContract]
    public class PatternQuestion
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int PatternId { get; set; }

        [DataMember]
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }

        public virtual Pattern Pattern { get; set; }
    }
}
