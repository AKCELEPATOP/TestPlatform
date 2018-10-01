using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    [DataContract]
    public class Stat
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public DateTime DateCreate { get; set; }

        [DataMember]
        [Required]
        public int Right { get; set; }

        [Required]
        [DataMember]
        public int Total { get; set; }

        [DataMember]
        public int PatternId { get; set; }

        public virtual Pattern Pattern { get; set; }

        [DataMember]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
