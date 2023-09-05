using System;

namespace MagicOnion.Server.Hubs
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MethodIdAttribute : Attribute
    {
        public readonly int MethodId;

        public MethodIdAttribute(int methodId)
        {
            MethodId = methodId;
        }
    }
}
