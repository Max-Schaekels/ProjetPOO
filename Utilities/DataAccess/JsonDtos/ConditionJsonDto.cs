using ProjetPOO.Model.Story.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.DataAccess.JsonDtos
{
    public class ConditionJsonDto
    {
        public int Id { get; set; }
        public int ChoiceId { get; set; }
        public ConditionType Type { get; set; }
        public int MinValue { get; set; }
    }
}
