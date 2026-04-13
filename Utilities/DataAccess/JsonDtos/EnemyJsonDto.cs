using ProjetPOO.Model.Combat.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.DataAccess.JsonDtos
{
    public class EnemyJsonDto
    {
        public int Id { get; set; }
        public int ScenarioId { get; set; }
        public EnemyType Type { get; set; }
        public int RewardExperience { get; set; }
        public int RewardGoldMin { get; set; }
        public int RewardGoldMax { get; set; }
        public int PotionDropChance { get; set; }
        public int PotionAmountMin { get; set; }
        public int PotionAmountMax { get; set; }
        public int KeyDropChance { get; set; }
        public int KeyAmountMin { get; set; }
        public int KeyAmountMax { get; set; }
        public string? Name { get; set; }
        public int MaxHp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Agility { get; set; }
    }
}
