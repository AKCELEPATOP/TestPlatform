using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    [DataContract]
    public class Answer
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public bool True { get; set; }

        [DataMember]
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
