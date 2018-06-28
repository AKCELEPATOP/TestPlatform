using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestService.BindingModels
{
    [DataContract]
    public class TestResponseModel
    {
        [DataMember]
        public int PatternId { get; set; }

        [DataMember]
        public List<QuestionResponseModel> QuestionResponses { get; set; }
    }
}
