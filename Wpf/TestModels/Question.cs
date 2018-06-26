using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    public class Question
    {
        public int Id { get; set; }

        [MaxLength(1000)]
        public string Text { get; set; }

        [ForeignKey("QuestionId")]
        public virtual List<Answer> Answers { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
