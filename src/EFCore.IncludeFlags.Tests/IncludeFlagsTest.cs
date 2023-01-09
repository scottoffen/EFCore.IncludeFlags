using EFCore.IncludeFlags.Exceptions;
using Shouldly;

namespace EFCore.IncludeFlags.Tests;

public class IncludeFlagsTests
{
    private IQueryable<TestModel> queryable;
    private readonly TestDbContext _dbContext = new TestDbContext();

    public IncludeFlagsTests()
    {
        queryable = (
            from o in _dbContext.TestModels
            select o
        );
    }

    [Fact]
    public void ReturnsQueryableOnSuccess()
    {
        queryable = queryable.IncludeFlags(TestFlags.Apple);
        queryable.Expression.ToString().ShouldEndWith(".Include(\"Apple\")");
    }

    [Fact]
    public void ReturnsQueryableOnSuccessWhenFlagsAreEmpty()
    {
        TestFlags flags = (TestFlags) 0;
        queryable = queryable.IncludeFlags(flags);
        queryable.Expression.ToString().ShouldEndWith(".Select(o => o)");
    }

    [Fact]
    public void ThrowExceptionWhenEnumIsMissingFlagsAttribute()
    {
        var ex = Should.Throw<MissingFlagsAttributeException>(() =>
        {
            var flags = MissingFlagsAttributeEnum.Apple;
            queryable.IncludeFlags(flags);
        });
    }

    [Fact]
    public void ThrowExceptionWhenEnumHasNonUniqueIntegerValues()
    {
        var ex = Should.Throw<InvalidEnumerationConstantException>(() =>
        {
            var flags = NonUniqueIntegerValuesEnum.Apple;
            queryable.IncludeFlags(flags);
        });

        ex.Value.ShouldBe(2);
        ex.EnumType.ShouldBe(typeof(NonUniqueIntegerValuesEnum));
    }

    [Fact]
    public void ThrowExceptionWhenEnumHasIntegerValuesThatAreNotPowersOfTwo()
    {
        var ex = Should.Throw<InvalidEnumerationConstantException>(() =>
        {
            var flags = NotPowersOfTwoEnum.Apple;
            queryable.IncludeFlags(flags);
        });

        ex.Value.ShouldBe((int)NotPowersOfTwoEnum.Orange);
        ex.Field.ShouldBe(NotPowersOfTwoEnum.Orange.ToString());
        ex.EnumType.ShouldBe(typeof(NotPowersOfTwoEnum));
    }

    [Fact]
    public void ThrowExceptionWhenEnumHasIntegerValuesThatAreZero()
    {
        var ex = Should.Throw<InvalidEnumerationConstantException>(() =>
        {
            var flags = HasZeroValueEnum.Apple;
            queryable.IncludeFlags(flags);
        });

        ex.Value.ShouldBe((int)HasZeroValueEnum.FruitSalad);
        ex.Field.ShouldBe(HasZeroValueEnum.FruitSalad.ToString());
        ex.EnumType.ShouldBe(typeof(HasZeroValueEnum));
    }

    [Fact]
    public void ThrowExceptionWhenEnumHasIntegerValuesThatAreNegative()
    {
        var ex = Should.Throw<InvalidEnumerationConstantException>(() =>
        {
            var flags = HasNegativeValueEnum.Apple;
            queryable.IncludeFlags(flags);
        });

        ex.Value.ShouldBe((int)HasNegativeValueEnum.FruitSalad);
        ex.Field.ShouldBe(HasNegativeValueEnum.FruitSalad.ToString());
        ex.EnumType.ShouldBe(typeof(HasNegativeValueEnum));
    }

    [Fact]
    public void ThrowExceptionWhenEnumHasNonUniqueMapValues()
    {
        var ex = Should.Throw<DuplicateMappedRelatedEntitiesException>(() =>
        {
            var flags = NonUniqueMapValuesEnum.Apple;
            queryable.IncludeFlags(flags);
        });

        ex.DuplicateValue.ShouldBe(nameof(TestModel.Orange));
        ex.EnumType.ShouldBe(typeof(NonUniqueMapValuesEnum));
    }

    [Fact]
    public void ThrowExceptionWhenEnumHasMultipleAllValues()
    {
        var ex = Should.Throw<UnableToDetermineAllException>(() =>
        {
            var flags = MultipleAllValuesEnum.Apple;
            queryable.IncludeFlags(flags);
        });

        ex.Fields.Length.ShouldBe(2);
        ex.Fields[0].ShouldBe(MultipleAllValuesEnum.All.ToString());
        ex.Fields[1].ShouldBe(MultipleAllValuesEnum.FruitSalad.ToString());
        ex.EnumType.ShouldBe(typeof(MultipleAllValuesEnum));
    }

    [Fact]
    public void ThrowExceptionWhenEnumValueIsNotValidPropertyOfEntity()
    {
        var ex = Should.Throw<InvalidRelatedEntityException>(() =>
        {
            var flags = InvalidRelatedEntityEnum.Apple;
            queryable.IncludeFlags(flags);
        });

        ex.Field.ShouldBe(InvalidRelatedEntityEnum.FruitSalad.ToString());
        ex.Value.ShouldBe(InvalidRelatedEntityEnum.FruitSalad.ToString());
        ex.EnumType.ShouldBe(typeof(InvalidRelatedEntityEnum));
        ex.EntityType.ShouldBe(typeof(TestModel));
    }
}
