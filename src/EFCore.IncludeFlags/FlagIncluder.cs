using System.Collections.Concurrent;
using System.Reflection;
using EFCore.IncludeFlags.Attributes;
using EFCore.IncludeFlags.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.IncludeFlags;

public static class FlagIncluder<TEntity, TEnum> where TEntity : class where TEnum : Enum
{
    private static readonly Type _enumType;
    private static readonly Type _compositeFlagType = typeof(CompositeFlagAttribute);
    private static readonly Type _includeAllType = typeof(IncludeAllAttribute);
    private static readonly Type _relatedEntityType = typeof(RelatedEntityAttribute);

    private static ConcurrentDictionary<TEnum, string> _enumMap;
    private static string _allLabel = "All";
    private static bool _hasAllValue = false;

#nullable enable
    private static TEnum? _allValue;
#nullable disable

    static FlagIncluder()
    {
        _enumType = typeof(TEnum);

        EnsureEnumHasFlagAttribute();
        EnsureEnumIntegersAreUnique();
        EnsureNonCompositeValuesArePowersOfTwo();
        DiscoverNewAllValueLabel();

        ScanAndCacheEnum();

        EnsureEnumMapValuesAreUnique();
        EnsureEnumMapValuesArePropertiesOfEntity();
    }

#nullable enable
    internal static IQueryable<TEntity> AddIncludes(IQueryable<TEntity> queryable, TEnum flags)
    {
#pragma warning disable CS8604
        if (_hasAllValue && flags.HasFlag(_allValue))
#pragma warning restore CS8604
        {
            foreach (var (key, value) in _enumMap)
            {
                queryable = queryable.Include(value);
            }
        }
        else
        {
            foreach (var (key, value) in _enumMap)
            {
                if (flags.HasFlag(key))
                {
                    queryable = queryable.Include(value);
                }
            }
        }

        return queryable;
    }
#nullable disable

    private static void DiscoverNewAllValueLabel()
    {
        var counter = 0;
        foreach (var constant in Enum.GetValues(_enumType).Cast<TEnum>())
        {
            var flag = constant.ToString();
            var member = _enumType.GetMember(flag).Single();
            if (!member.GetCustomAttributes(_includeAllType, false).Any()) continue;

            counter++;
            if (counter > 1)
                throw new UnableToDetermineAllException(_enumType, new string[2]{ _allLabel, flag });

            _allLabel = flag;
        }
    }

    private static void EnsureEnumHasFlagAttribute()
    {
        if (!_enumType.IsDefined(typeof(FlagsAttribute), false))
        {
            throw new MissingFlagsAttributeException(_enumType);
        }
    }

    private static void EnsureEnumIntegersAreUnique()
    {
        var uniqueValues = new HashSet<int>();
        foreach (var flag in Enum.GetValues(_enumType).Cast<TEnum>())
        {
            var value = Convert.ToInt32(flag);
            if (!uniqueValues.Add(value)) throw new InvalidEnumerationConstantException(_enumType, value);
        }
    }

    private static void EnsureNonCompositeValuesArePowersOfTwo()
    {
        foreach (var constant in Enum.GetValues(_enumType).Cast<TEnum>())
        {
            var flag = constant.ToString();
            var value = Convert.ToInt32(constant);
            var member = _enumType.GetMember(flag).Single();

            // Nothing can be zero or negative numbers.
            if (value <= 0)
                throw new InvalidEnumerationConstantException(_enumType, flag, value);

            // Skip members that have the composite type flag on them
            if (member.GetCustomAttributes(_compositeFlagType, false).Any()) continue;

            // Skip members that have the include all flag on them
            if (member.GetCustomAttributes(_includeAllType, false).Any()) continue;

            if ((value & (value - 1)) != 0)
                throw new InvalidEnumerationConstantException(_enumType, flag, value);
        }
    }

    private static void EnsureEnumMapValuesAreUnique()
    {
        var uniqueValues = new HashSet<string>();
        foreach (var (key, value) in _enumMap)
        {
            if (!uniqueValues.Add(value)) throw new DuplicateMappedRelatedEntitiesException(_enumType, value);
        }
    }

    private static void EnsureEnumMapValuesArePropertiesOfEntity()
    {
        var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(p => p.Name);
        foreach (var (key, value) in _enumMap)
        {
            if (!properties.Contains(value))
            {
                throw new InvalidRelatedEntityException(_enumType, typeof(TEntity), key.ToString(), value);
            }
        }
    }

    private static void ScanAndCacheEnum()
    {
        _enumMap = new ConcurrentDictionary<TEnum, string>();

        foreach (var constant in Enum.GetValues(_enumType).Cast<TEnum>())
        {
            var flag = constant.ToString();
            var value = Convert.ToInt32(constant);

            // 1. If this flag represents all, do not add it to the cache, set it as the _allValue
            if (string.Equals(flag, _allLabel, StringComparison.CurrentCultureIgnoreCase))
            {
                _allValue = constant;
                _hasAllValue = true;
                continue;
            }

            // 2. If this flag is a composite flag, do not add it to the cache
            var member = _enumType.GetMember(flag).Single();
            if (member.GetCustomAttributes(_compositeFlagType, false).Any()) continue;

            // 3. If this flag has the RelatedEntity attribute with a value, use that value instead
            var related = member.GetCustomAttributes(_relatedEntityType, false).Cast<RelatedEntityAttribute>();
            if (related.Any() && !string.IsNullOrWhiteSpace(related.First().Value)) flag = related.First().Value;

            // 4. Add enum constant and related entity string to cache map
            _enumMap.TryAdd(constant, flag);
        }
    }
}
