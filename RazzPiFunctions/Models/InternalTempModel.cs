using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazzPiFunctions.Models
{
    public class InternalTempModel : BaseModel
    {
        /// <value>The temperature.</value>
        [JsonProperty("temp")]
        public string Temp { get; set; }
    }
}
