namespace EFCore.IncludeFlags.Attributes;

[AttributeUsage(AttributeTargets.Field, Inherited = false)]
public sealed class RelatedEntityAttribute : System.Attribute
{
    public RelatedEntityAttribute(string value) => Value = value;

    public string Value { get; set; }
}
