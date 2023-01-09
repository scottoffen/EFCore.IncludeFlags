using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace EFCore.IncludeFlags.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class InvalidEnumerationConstantException : Exception
{
    private const string NotMultpleOfTwoErrorMessage = "Value {0} of flag {1}.{2} is not a power of 2. Are you missing the CompositeFlagAttribute?";
    private const string DuplicateIntegerValueErrorMessage = "Enum {0} has more than one constant with a value of {1}";

    /// <summary>
    /// The type of the enumeration that generated the execption.
    /// </summary>
    /// <returns></returns>
    public Type EnumType { get; private set; } = typeof(object);

    /// <summary>
    /// The enumeration constant that generated the exception.
    /// </summary>
    /// <value></value>
    public string Field { get; private set; } = string.Empty;

    /// <summary>
    /// The enumeration constant value that generated the exception.
    /// </summary>
    /// <value></value>
    public int Value { get; private set; }

    /// <summary>
    /// Enumeration constant integer values should be unique. This exception will include a message indicating that duplicate integer values were found in the enumeration.
    /// </summary>
    /// <param name="enumType">The enumeration type</param>
    /// <param name="enumValue">The duplicate integer value</param>
    /// <returns></returns>
    public InvalidEnumerationConstantException(Type enumType, int enumValue) : base(string.Format(DuplicateIntegerValueErrorMessage, enumType.Name, enumValue))
    {
        EnumType = enumType;
        Value = enumValue;
    }

    /// <summary>
    /// Enumeration constants should be defined as non-zero integers in powers of 2. This exception will include a message indicating the enumeration class and field that does not meet this condition, and suggest a possible fix.
    /// </summary>
    /// <param name="enumType">The enumeration type</param>
    /// <param name="enumField">The eumeration constant string</param>
    /// <param name="enumValue">The enumeration constant integer</param>
    /// <returns></returns>
    public InvalidEnumerationConstantException(Type enumType, string enumField, int enumValue) : base(string.Format(NotMultpleOfTwoErrorMessage, enumType.FullName, enumField, enumValue))
    {
        EnumType = enumType;
        Field = enumField;
        Value = enumValue;
    }

    private InvalidEnumerationConstantException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
