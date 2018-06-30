using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TestModels;

namespace TestService.ViewModels
{
    [DataContract]
    public class QuestionViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public QuestionComplexity Complexity { get; set; }

        [DataMember]
        public string ComplexityName { get; set; }

        [DataMember]
        public string Image { get; set; }

        [DataMember]
        public long Time { get; set; }

        [DataMember]
        public bool Active { get; set; }

        [DataMember]
        public List<AnswerViewModel> Answers { get; set; }

        [DataMember]
        public string CategoryName { get;  set; }
    }
}
