using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    public class PatternCategory
    {
        public int Id { get; set; }

        public double Complex { get; set; }

        public double Middle { get; set; }

        public double Easy { get; set; }

        public int PatternId { get; set; }

        public int CategoryId { get; set; }

        public virtual Pattern Pattern  { get; set; }

        public virtual Category Category { get; set; }
    }
}
