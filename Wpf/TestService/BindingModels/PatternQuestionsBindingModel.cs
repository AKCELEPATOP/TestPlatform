using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestService.BindingModels
{
    [DataContract]
    public class PatternQuestionsBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int QuestionId { get; set; }

        [DataMember]
        public int PatternId { get; set; }
    }
}
