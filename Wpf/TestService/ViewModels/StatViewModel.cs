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
    }
}
