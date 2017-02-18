using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiNativeShared.API.Models
{
    public class HNQuestionsResult
    {
        public ObservableCollection<HNQuestion> questions { get; set; }
        public HNPagination pagination { get; set; }
        public HNQuestionFilters question_filters { get; set; }
    }

    public class HNQuestionFilters
    {
        public bool questions_not_answered_only { get; set; }
        public bool interesting_questions_only { get; set; }
        public bool questions_with_audios_only { get; set; }
    }
}
