using EFCore.IncludeFlags.Attributes;

namespace EFCore.IncludeFlags.Tests
{
    public enum MissingFlagsAttributeEnum
    {
        Apple,
        Banana,
        Orange
    }

    [Flags]
    public enum NonUniqueIntegerValuesEnum
    {
        Apple = 1,
        Banana = 2,
        Orange = 2
    }

    [Flags]
    public enum NotPowersOfTwoEnum
    {
        Apple = 1,
        Banana = 2,
        [CompositeFlag]
        FruitSalad = 3,
        Orange = 7,
    }

    [Flags]
    public enum HasZeroValueEnum
    {
        Apple = 1,
        Banana = 2,
        [CompositeFlag]
        FruitSalad = 0,
        Orange = 4,
    }

    [Flags]
    public enum HasNegativeValueEnum
    {
        Apple = 1,
        Banana = 2,
        [CompositeFlag]
        FruitSalad = -2,
        Orange = 4,
    }

    [Flags]
    public enum MultipleAllValuesEnum
    {
        [IncludeAll]
        All = 1 << 0,
        Apple = 1 << 1,
        Banana = 1 << 2,
        Orange = 1 << 3,
        [IncludeAll]
        FruitSalad = 1 << 4,
    }

    [Flags]
    public enum NonUniqueMapValuesEnum
    {
        Apple = 1 << 0,
        Banana = 1 << 2,
        Orange = 1 << 3,
        [RelatedEntity(nameof(TestModel.Orange))]
        FruitSalad = 1 << 4,
    }

    [Flags]
    public enum InvalidRelatedEntityEnum
    {
        Apple = 1 << 0,
        Banana = 1 << 2,
        Orange = 1 << 3,
        FruitSalad = 1 << 4,
    }
}
