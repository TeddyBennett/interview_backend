using System;

namespace Backend_Test.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class HideFieldsAttribute : Attribute
    {
        public string[] Fields { get; }

        public HideFieldsAttribute(params string[] fields)
        {
            Fields = fields;
        }
    }
}
