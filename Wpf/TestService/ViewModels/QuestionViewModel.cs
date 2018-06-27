using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestService.ViewModels
{
    [DataContract]
    public class QuestionViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public string Complexity { get; set; }

        [DataMember]
        public bool Active { get; set; }

        [DataMember]
        public List<AnswerViewModel> Answers { get; set; }
    }
}
