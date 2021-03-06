﻿namespace CoPiloto.Models
{
    using System;
    using Newtonsoft.Json;

    public partial class AiportCharts
    {
        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("charts")]
        public Charts Charts { get; set; }
    }

    public partial class Charts
    {
        [JsonProperty("General")]
        public Approach[] General { get; set; }

        [JsonProperty("SID")]
        public Approach[] Sid { get; set; }

        [JsonProperty("STAR")]
        public Approach[] Star { get; set; }

        [JsonProperty("Approach")]
        public Approach[] Approach { get; set; }
    }

    public partial class Approach
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("chartname")]
        public string Chartname { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("proxy")]
        public string Proxy { get; set; }
    }

    public partial class Info
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("elevation")]
        public long Elevation { get; set; }
    }
}
