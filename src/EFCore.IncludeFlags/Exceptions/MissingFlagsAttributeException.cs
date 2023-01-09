using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace EFCore.IncludeFlags.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class MissingFlagsAttributeException : Exception
{
    private const string MissingFlagsAttributeMessage = "The target enumeration {0} does not have the required FlagsAttribute.";

    /// <summary>
    /// The type of the enumeration that generated the execption.
    /// </summary>
    /// <returns></returns>
    public Type EnumType { get; } = typeof(object);

    /// <summary>
    /// The specified enumeration is expected to have the FlagsAttribute in order for bitwise operations to be performed.
    /// </summary>
    /// <param name="enumType">The enumeration type</param>
    /// <returns></returns>
    public MissingFlagsAttributeException(Type enumType) : base(string.Format(MissingFlagsAttributeMessage, enumType.FullName))
    {
        EnumType = enumType;
    }

    private MissingFlagsAttributeException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
