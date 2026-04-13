using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.DataAccess.JsonDtos
{
    public class ShopJsonDto
    {
        public int Id { get; set; }
        public int ScenarioId { get; set; }
        public string? Name { get; set; }
        public int PotionPrice { get; set; }
        public int KeyPrice { get; set; }
    }
}
