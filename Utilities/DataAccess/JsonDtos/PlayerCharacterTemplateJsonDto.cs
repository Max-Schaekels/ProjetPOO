using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.DataAccess.JsonDtos
{
    public class PlayerCharacterTemplateJsonDto
    {
        public int Id { get; set; }
        public int ScenarioId { get; set; }
        public string? Name { get; set; }
        public int MaxHp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Agility { get; set; }
        public int StartingExperience { get; set; }
        public int StartingLevel { get; set; }
    }
}
