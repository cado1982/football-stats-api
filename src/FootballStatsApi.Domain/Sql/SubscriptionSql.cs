using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Domain.Sql
{
    public static class SubscriptionSql
    {
        public static string GetByName = @"
        SELECT 
            id,
            internal_name as internalname,
            display_name as displayname,
            hourly_call_limit as hourlycalllimit,
            cost,
            is_active as isactive,
            is_internal as isinternal
        FROM 
            ""public"".""subscriptions"" ts
        WHERE
            internal_name = @Name;";

        public static string GetById = @"
        SELECT 
            id,
            internal_name as internalname,
            display_name as displayname,
            hourly_call_limit as hourlycalllimit,
            cost,
            is_active as isactive,
            is_internal as isinternal
        FROM 
            ""public"".""subscriptions"" ts
        WHERE
            id = @Id;";
    }
}