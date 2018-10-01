using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestService.ViewModels
{
    [DataContract]
    public class StatCategoryViewModel
    {
        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public int Total { get; set; }

        [DataMember]
        public int Right { get; set; }

        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public List<StatQuestionViewModel> Questions { get; set; }
    }
}
