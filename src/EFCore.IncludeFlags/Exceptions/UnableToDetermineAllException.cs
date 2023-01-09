using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace EFCore.IncludeFlags.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class UnableToDetermineAllException : Exception
{
    private const string UnableToDetermineAllMessage = "Unable to resolve All on {0} between values ({1})";

    /// <summary>
    /// The type of the enumeration that generated the execption.
    /// </summary>
    /// <returns></returns>
    public Type EnumType { get; } = typeof(object);

    /// <summary>
    /// The enumeration constants that generated the exception.
    /// </summary>
    /// <value></value>
    public string[] Fields { get; set; } = new string[0];

    public UnableToDetermineAllException(Type enumType, string[] fields) : base(string.Format(UnableToDetermineAllMessage, enumType.FullName, string.Join(", ", fields)))
    {
        EnumType = enumType;
        Fields = fields;
    }

    private UnableToDetermineAllException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
