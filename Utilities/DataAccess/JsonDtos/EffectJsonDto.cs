using ProjetPOO.Model.Story.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.DataAccess.JsonDtos
{
    public class EffectJsonDto
    {
        public int Id { get; set; }
        public int ChoiceId { get; set; }
        public EffectType Type { get; set; }
        public int? Amount { get; set; }
        public string? FlagKey { get; set; }
    }

}
