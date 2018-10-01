using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService.ViewModels
{
    public class PatternUsingModel
    {
        public string Name { get; set; }

        public List<QuestionViewModel> Questions { get; set; }

        public List<PatternCategoryViewModel> PatternCategories { get; set; }
    }
}
