using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.DataAccess.JsonDtos
{
    public class ChoiceJsonDto
    {
        public int Id { get; set; }
        public string? Label { get; set; }
        public int TargetSceneId { get; set; }
        public int SceneId { get; set; }
    }
}
