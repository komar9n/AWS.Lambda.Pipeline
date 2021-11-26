using AWS.Lambda.Pipeline.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AWS.Lambda.Pipeline
{
    public static class Utils
    {
        public static IList<BaseRequestPipelineAttribute> GetAttributes<TPipelineAttribute>(Type callerType, string callerName)
        {
            var methodExecution = callerType.GetMethods().First(m => m.Name.Equals(callerName));

            return methodExecution
                .GetCustomAttributes(false)
                .Select(a => a is TPipelineAttribute ? (BaseRequestPipelineAttribute)a : null)
                .Where(a => a != null)
                .ToList();
        }
    }
}
