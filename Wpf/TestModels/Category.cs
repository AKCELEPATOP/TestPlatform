﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    public class Category
    {
        public int Id { get; set; }

        [StringLength(30, ErrorMessage = "Name length should be from 1 to 30", MinimumLength = 1)]
        public string Name { get; set; }

        [ForeignKey("CategoryId")]
        public virtual List<Question> Questions { get; set; }

        [ForeignKey("CategoryId")]
        public virtual List<PatternCategory> PatternCategories { get; set; }
    }
}
