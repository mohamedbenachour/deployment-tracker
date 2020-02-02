namespace IntegrationTests.Helpers {
    public class FluentUrlBuilder {
        private string CurrentUrl { get; }

        private FluentUrlBuilder(string url) {
            CurrentUrl = url;
        }

        public override string ToString() => CurrentUrl;

        public static FluentUrlBuilder WithRoot(string root)
            => new FluentUrlBuilder($"/{root}/");

        public FluentUrlBuilder WithPart(object part)
            => new FluentUrlBuilder($"{CurrentUrl}{part.ToString()}/");
    }
}