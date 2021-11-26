using System;

namespace AWS.Lambda.Pipeline.Attributes.Binding
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class FromPathAttribute : Attribute
    {
        public FromPathAttribute(string property, bool isRequired = true)
        {
            PropertyName = property;
            IsRequired = isRequired;
        }

        public bool IsRequired { get; }

        public string PropertyName { get; }
    }
}
