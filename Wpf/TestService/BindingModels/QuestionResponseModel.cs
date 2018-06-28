using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestService.BindingModels
{
    [DataContract]
    public class QuestionResponseModel
    {
        [DataMember]
        public int QuestionId { get; set; }

        [DataMember]
        public List<int> Answers { get; set; }
    }
}
