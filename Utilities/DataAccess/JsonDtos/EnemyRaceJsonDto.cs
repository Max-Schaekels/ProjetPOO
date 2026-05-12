using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.DataAccess.JsonDtos
{
    public class EnemyRaceJsonDto
    {
        public int Id { get; set; }
        public int ScenarioId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
