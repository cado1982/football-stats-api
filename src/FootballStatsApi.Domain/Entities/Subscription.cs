﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Domain.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public string InternalName { get; set; }
        public string DisplayName { get; set; }
        public int HourlyCallLimit { get; set; }
        public int Cost { get; set; }
        public bool IsActive { get; set; }
        public bool IsInternal { get; set; }
        public List<SubscriptionFeature> Features { get; set; }
    }
}
