using System;
using System.Collections.Generic;
using System.Text;

namespace FootballStatsApi.Domain.Sql
{
    public static class SubscriptionSql
    {
        public static string GetByName = @"
        SELECT 	
	        s.id,
	        s.internal_name as internalname,
	        s.display_name as displayname,
	        s.hourly_call_limit as hourlycalllimit,
	        s.cost,
	        s.is_active as isactive,
	        s.is_internal as isinternal,
	        sf.id,
	        sf.display_name as displayname
        FROM
	        ""public"".""subscriptions"" s
        LEFT JOIN 
	        ""public"".""subscription_features"" sf ON sf.subscription_id = s.id
        WHERE
            internal_name = @Name AND
            s.is_internal = false;";

        public static string GetById = @"
        SELECT 	
	        s.id,
	        s.internal_name as internalname,
	        s.display_name as displayname,
	        s.hourly_call_limit as hourlycalllimit,
	        s.cost,
	        s.is_active as isactive,
	        s.is_internal as isinternal,
	        sf.id,
	        sf.display_name as displayname
        FROM
	        ""public"".""subscriptions"" s
        LEFT JOIN 
	        ""public"".""subscription_features"" sf ON sf.subscription_id = s.id
        WHERE
            s.id = @Id AND
            s.is_internal = false;";

        public static string GetAll = @"
        SELECT 	
	        s.id,
	        s.internal_name as internalname,
	        s.display_name as displayname,
	        s.hourly_call_limit as hourlycalllimit,
	        s.cost,
	        s.is_active as isactive,
	        s.is_internal as isinternal,
	        sf.id,
	        sf.display_name as displayname
        FROM
	        ""public"".""subscriptions"" s
        LEFT JOIN 
	        ""public"".""subscription_features"" sf ON sf.subscription_id = s.id
        WHERE
            s.is_internal = false
        ORDER BY
            s.id;";

        public static string ChangeUsersSubscription = @"
        UPDATE
            ""identity"".""users""
        SET
            subscription_id = @SubscriptionId
        WHERE
            id = @UserId;";
    }
}