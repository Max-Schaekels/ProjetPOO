using ProjetPOO.Model.Story.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.DataAccess.JsonDtos
{
    public class SceneJsonDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public SceneType Type { get; set; }
        public int ScenarioId { get; set; }
        public string? PictureFileName { get; set; }
        public int? ShopId { get; set; }
        public int? EnemyId { get; set; }
        public int? FleeTargetSceneId { get; set; }
        public int? DefeatTargetSceneId { get; set; }
        public int? VictoryTargetSceneId { get; set; }
    }
}
