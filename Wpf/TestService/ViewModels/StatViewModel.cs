using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestService.ViewModels
{
    [DataContract]
    public class StatViewModel
    {
        [DataMember]
        public List<StatCategoryViewModel> StatCategories { get; set; }

        [DataMember]
        public int Total { get; set; }

        [DataMember]
        public int Right { get; set; }

        [DataMember]
        public int Mark { get; set; }

        [DataMember]
        public string PatternName { get; set; }

        [DataMember]
        public string DateCreate { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Email { get; set; }

    }
}
