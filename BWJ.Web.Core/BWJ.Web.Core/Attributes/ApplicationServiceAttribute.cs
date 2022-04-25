using BWJ.Web.Core.Sentinels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BWJ.Web.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class ApplicationServiceAttribute : Attribute
    {
        public ApplicationServiceAttribute(ApplicationServiceLifetime serviceType)
        {
            Type = serviceType;
            IsNamespaceSentinel = false;
        }
        protected ApplicationServiceAttribute(ApplicationServiceLifetime serviceType, bool isNamespaceSentinel)
        {
            Type = serviceType;
            IsNamespaceSentinel = isNamespaceSentinel;
        }

        public ApplicationServiceLifetime Type { get; }
        public bool IsNamespaceSentinel { get; }
    }
}
