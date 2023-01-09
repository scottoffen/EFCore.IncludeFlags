# <img src="includeflags.jpg" width=25px> EFCore.IncludeFlags

EFCore.IncludeFlags is a small library that allows you to use enumerations with the [`Flags`](https://learn.microsoft.com/en-us/dotnet/api/system.flagsattribute?view=net-7.0) attribute to specify which related data you want to include with your entity when querying the database with EntityFramework.

## Installation

EFCore.IncludeFlags is available on [NuGet.org](https://www.nuget.org/packages/EFCore.IncludeFlags/).

## Usage

Define an enum class with the `Flags` attribute. Each enumeration constant must meet the following criteria:

1. Integer value must greater than 0
2. Integer values must be unique
3. Integer value must either be a power of two OR have the `CompositeFlag` or `IncludeAll` attribute.
4. Enumeration constants must match the name of a property in the related entity OR have the `RelatedEntity` attribute with a value that matches the name of a property in the related entity.
5. Enumeration constants must resolve to unique values

Use the `IQueryable<T>.IncludeFlag()` extensions method to pass in flags for the entity properties you want to include.

### Custom Attributes

#### Using `IncludeAll` Attribute

If an enumeration constant is `All`, it will automatically be recognized to mean include all the values in the enumeration (excluding composite values).

If you want to use a different enumeration constant to mean all, apply the `IncludeAll` attribute to it. From that point, All will no longer have any speciall meaning, so if you happen to have a related entity property named `All`, you're good to go.

If you have multiple enumeration constants with the `IncludeAll` attribute, an exception of type `UnableToDetermineAllException` will be thrown.

It is not required to have an enumeration constant that means All.

#### Using `CompositeFlag` Attribute

The `CompositeFlag` attribute is used on enumeration constants that do not have a value that is a power of 2, and are intended to be used to create an enumerated constant for commonly used flag combinations that will not be caught by the enumeration valiation process.

#### Using `RelatedEntity` Attribute

In cases where you do not want to use the name of the entity property as the name of the enumeration constant, you can choose another value for the enumeration constant and add the `RelatedEntity` attribute to redirect to the appropriate entity property. It is recommended to use the `nameof(Entity.Property)` format so that the relationship can be updated in the event that the entity class is refactored.

### More Examples

See the [Sample application](./src/Samples/) for specific usage examples.

## Support

- Engage in our [community discussions](https://github.com/scottoffen/efcore-includeflags/discussions) for Q&A, ideas, and show and tell!

- **Issues created to ask "how to" questions will be closed.**

## Contributing

We welcome contributions from the community! In order to ensure the best experience for everyone, before creating an issue or submitting a pull request, please see the [contributing guidelines](CONTRIBUTING.md) and the [code of conduct](CODE_OF_CONDUCT.md). Failure to adhere to these guidelines can result in significant delays in getting your contributions included in the project.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/scottoffen/efcore-includeflags/releases).

## License

EFCore.IncludeFlags is licensed under the [MIT](https://choosealicense.com/licenses/mit/) license.