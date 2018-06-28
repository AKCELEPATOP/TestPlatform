using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestService.BindingModels
{
    [DataContract]
    public class PatternBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int UserGroupId { get; set; }

        [DataMember]
        public List<PatternCategoriesBindingModel> PatternCategories { get; set; }

        [DataMember]
        public List<PatternQuestionsBindingModel> PatternQuestions { get; set; }
    }
}
