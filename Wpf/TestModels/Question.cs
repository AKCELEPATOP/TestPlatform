using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    [DataContract]
    public class Question
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [MaxLength(1000)]
        public string Text { get; set; }

        [ForeignKey("QuestionId")]
        public virtual List<Answer> Answers { get; set; }

        [DataMember]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
