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

        [DataMember]
        public long Time { get; set; }

        [DataMember]
        [Required]
        public QuestionComplexity Complexity { get; set; }

        [DataMember]
        [Required]
        public bool Active { get; set; }

        [ForeignKey("QuestionId")]
        public virtual List<Answer> Answers { get; set; }

        [DataMember]
        [Required]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [ForeignKey("QuestionId")]
        public virtual List<PatternQuestion> PatternQuestions { get; set; }
    }
}
