using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestService.ViewModels
{
    [DataContract]
    public class PatternQuestionViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int PatternId { get; set; }

        [DataMember]
        public int QuestionId { get; set; }

        [DataMember]
        public string QuestionText { get; set; }

        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public string Complexity { get; set; }
    }
}
