using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;


namespace EFCore.IncludeFlags.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]

public class InvalidRelatedEntityException : Exception
{
    private const string InvalidRelatedEntityMessage = "Related entity value {0} from enumeration constant {1}.{2} does not exist as a property on entity {3}";

    /// <summary>
    /// The type of the enumeration that generated the execption.
    /// </summary>
    /// <returns></returns>
    public Type EnumType { get; set; } = typeof(object);

    /// <summary>
    /// The type of the entity that generated the execption.
    /// </summary>
    /// <returns></returns>
    public Type EntityType { get; set; } = typeof(object);

    /// <summary>
    /// The enumeration constant that generated the exception.
    /// </summary>
    /// <value></value>
    public string Field { get; set; } = string.Empty;

    /// <summary>
    /// The enumeration constant related entity name
    /// </summary>
    /// <value></value>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// All values of the enumeration must resolve to a property on the entity. This exception will include a message indicating which enumeration constant resolves to a value that does not exist as a property of the entity.
    /// </summary>
    /// <param name="enumType"></param>
    /// <param name="entityType"></param>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public InvalidRelatedEntityException(Type enumType, Type entityType, string field, string value) : base(string.Format(InvalidRelatedEntityMessage, value, enumType.FullName, field, entityType.FullName))
    {
        EnumType = enumType;
        EntityType = entityType;
        Field = field;
        Value = value;
    }

    private InvalidRelatedEntityException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
