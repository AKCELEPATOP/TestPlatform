using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestService.ViewModels
{
    [DataContract]
    public class TestViewModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int PatternId { get; set; }

        [DataMember]
        public List<QuestionViewModel> Questions { get; set; }

        [DataMember]
        public long Time { get; set; }
    }
}
