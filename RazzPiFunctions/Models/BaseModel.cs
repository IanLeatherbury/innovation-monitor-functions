using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazzPiFunctions.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            Id = Guid.NewGuid().ToString();
        }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
