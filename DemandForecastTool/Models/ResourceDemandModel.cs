using DemandForecastTool.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemandForecastTool.Models
{
    public class ResourceDemandModel
    {
        [Required(ErrorMessage = "Please select a valid environment.")]
        [Range(1, 1000000,ErrorMessage = "Please select a valid environment.")]
        public int EnvironmentId { get; set; }

        [Required(ErrorMessage = "Please select a valid datacentre.")]
        [Range(1, 1000000, ErrorMessage = "Please select a valid datacentre.")]
        public int DataCentreId { get; set; }

        [Required(ErrorMessage = "Please select a valid resource type.")]
        [Range(1, 1000000, ErrorMessage = "Please select a valid resource type.")]
        public int ResourceTypeId { get; set; }

        public DateTime RequestedDate { get; set; }
        public int CTS { get; set; }
        public int EIS { get; set; }
        public int GMB { get; set; }
        public int GMIT { get; set; }
        public int RISK { get; set; }
    }

    public class SelectItemModel
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class ReadResourceDemandModel
    {
        public int Id { get; set; }
        public string Environment { get; set; }
        public string DataCentre { get; set; }
        public string ResourceType { get; set; }
        public string RequestedDate { get; set; }
        public int CTS { get; set; }
        public int EIS { get; set; }
        public int GMB { get; set; }
        public int GMIT { get; set; }
        public int RISK { get; set; }
    }

}
