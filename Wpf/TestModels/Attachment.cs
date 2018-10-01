using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    [DataContract]
    public class Attachment
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
