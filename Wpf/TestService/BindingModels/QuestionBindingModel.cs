using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TestModels;

namespace TestService.BindingModels
{
    [DataContract]
    public class QuestionBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public long Time { get; set; }

        [DataMember]
        public string ImagesPath { get; set; }

        [DataMember]
        public List<string> Images { get; set; }

        [DataMember]
        public QuestionComplexity Complexity { get; set; }

        [DataMember]
        public bool Active { get; set; }

        [DataMember]
        public List<AnswerBindingModel> Answers { get; set; }

        [DataMember]
        public int CategoryId { get; set; }
    }
}
