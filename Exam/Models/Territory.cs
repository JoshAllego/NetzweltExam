﻿using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Exam.Models
{ 

    public class Root
    {
        public Territory[] data { get; set; }
    }
    public class Territory
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? parent { get; set; }
    }

    public class SubTerritory
    {
        public string? Name { get; set; }
        public List<string> SubTerritories { get; set; } = new List<string>();  
    }

    public class MainTerritory
    {
        public string? Name { get; set; }
        public List<SubTerritory> SubTerritories { get; set; } = new List<SubTerritory>();
    }
    
}
