using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.DataAccess.JsonDtos
{
    public class JsonCollectionDto<T>
    {
        [JsonProperty("$values")]
        public List<T>? Values { get; set; }
    }
}
