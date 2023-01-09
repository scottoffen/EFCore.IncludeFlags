using EFCore.IncludeFlags.Attributes;

namespace EFCore.IncludeFlags.Tests
{
    public class TestModel
    {
        public string Id { get; set; } = string.Empty;

        public string AppleId { get; set; } = string.Empty;

        public string BananaId { get; set; } = string.Empty;

        public string OrangeId { get; set; } = string.Empty;

        public Apple Apple { get; set; } = null!;

        public virtual Banana Banana { get; set; } = null!;

        public virtual Orange Orange { get; set; } = null!;
    }

    public class Apple
    {
        public string Id { get; set; } = string.Empty;
    }

    public class Banana
    {
        public string Id { get; set; } = string.Empty;
    }

    public class Orange
    {
        public string Id { get; set; } = string.Empty;
    }

    [Flags]
    public enum TestFlags
    {
        Apple = 1 << 0,
        Banana = 1 << 2,

        [RelatedEntity(nameof(TestModel.Orange))]
        SweetOrange = 1 << 3,

        All = 1 << 4,

        [CompositeFlag]
        ApplesAndBanans = Apple | Banana,
    }
}
