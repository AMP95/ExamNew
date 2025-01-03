﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Sub
{
    public class RoutePoint
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public LoadingType Loading { get; set; }
        public string Phones { get; set; }
    }
}
