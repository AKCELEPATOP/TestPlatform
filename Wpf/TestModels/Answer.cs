﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    public class Answer
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public bool True { get; set; }

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
