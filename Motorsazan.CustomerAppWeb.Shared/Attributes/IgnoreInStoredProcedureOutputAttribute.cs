using System;

namespace Motorsazan.CustomerAppWeb.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreInStoredProcedureOutputAttribute : Attribute
    {
    }
}