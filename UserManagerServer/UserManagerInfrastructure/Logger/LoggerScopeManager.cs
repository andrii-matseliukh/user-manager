using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagerInfrastructure.Logger
{
    public static class LoggerScopeManager
    {
        public static Dictionary<string, string> GetUserScope(HttpContext context)
        {
            return new Dictionary<string, string>
            {
                ["UserName"] = context.User?.Identity.Name ?? "unknow-user"
            };
        }
    }
}
