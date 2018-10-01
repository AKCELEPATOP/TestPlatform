using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    [DataContract]
    public class Pattern
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int? UserGroupId { get; set; }

        public virtual UserGroup UserGroup { get; set; }

        [ForeignKey("PatternId")]
        public virtual List<PatternCategory> PatternCategories { get; set; }

        [ForeignKey("PatternId")]
        public virtual List<Stat> Stats { get; set; }

        [ForeignKey("PatternId")]
        public virtual List<PatternQuestion> PatternQuestions { get; set; }
    }
}
