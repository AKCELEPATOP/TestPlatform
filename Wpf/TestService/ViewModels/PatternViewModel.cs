using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestService.ViewModels
{
    [DataContract]
    public class PatternViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public int UserGroupId { get; set; }

        [DataMember]
        public string UserGroupName { get; set; }

        [DataMember]
        public List<PatternCategoryViewModel> PatternCategories { get; set; }

    }
}
