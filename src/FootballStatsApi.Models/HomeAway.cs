using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Models
{
    public class HomeAway<T> where T : struct
    {
        public T Home { get; set; }
        public T Away { get; set; }
    }
}
