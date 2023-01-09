using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace EFCore.IncludeFlags.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class DuplicateMappedRelatedEntitiesException : Exception
{
    private const string DuplicateFoundMessage = "{0} has multiple entries the resolve to the value {1}";

    /// <summary>
    /// The type of the enumeration that generated the execption.
    /// </summary>
    /// <returns></returns>
    public Type EnumType { get; private set; } = typeof(object);

    /// <summary>
    /// The related entity value that generated the exception.
    /// </summary>
    /// <value></value>
    public string DuplicateValue { get; private set; } = string.Empty;

    /// <summary>
    /// Enumeration constants should resolve to unique value. This exception will include a message indicating that not all enumeration constants resolved to unique values, and which value triggered this exception.
    /// </summary>
    /// <param name="enumType">The enumeration type</param>
    /// <param name="duplicateValue">The realted entity value</param>
    /// <returns></returns>
    public DuplicateMappedRelatedEntitiesException(Type enumType, string duplicateValue) : base(string.Format(DuplicateFoundMessage, enumType.FullName, duplicateValue))
    {
        EnumType = enumType;
        DuplicateValue = duplicateValue;
    }

    private DuplicateMappedRelatedEntitiesException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
