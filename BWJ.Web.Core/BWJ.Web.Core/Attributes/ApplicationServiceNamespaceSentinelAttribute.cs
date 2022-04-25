using BWJ.Web.Core.Sentinels;
using System;

namespace BWJ.Web.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ApplicationServiceNamespaceSentinelAttribute : ApplicationServiceAttribute
    {
        public ApplicationServiceNamespaceSentinelAttribute(ApplicationServiceLifetime serviceType) :
            base(serviceType, true)
        { }
    }
}
