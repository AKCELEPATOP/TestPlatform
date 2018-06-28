using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestService.ViewModels
{
    [DataContract]
    public class PatternCategoryViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int PatternId { get; set; }

        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public double Complex { get; set; }

        [DataMember]
        public double Middle { get; set; }

        [DataMember]
        public double Easy { get; set; }

        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public List<PatternQuestionViewModel> PatternQuestions { get; set; }
    }
}
