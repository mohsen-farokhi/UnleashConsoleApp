using Unleash;

string apiToken = "default:development.f2189c4b8b7e2539cfabe513b3738c0ed66d34b210e1f0e1e05da63d";

var unleash = new DefaultUnleash(new UnleashSettings
{
    AppName = "my-app",
    InstanceTag = "development",
    UnleashApi = new Uri("http://localhost:4242/api/"),
    CustomHttpHeaders = new Dictionary<string, string>
    {
        { "Authorization", apiToken }
    },
});

Console.WriteLine("Unleash client started ✅");
Console.WriteLine("Checking feature flags...");

string currentUserId = "123";

while (true)
{
    bool isEnabled = unleash.IsEnabled("my-feature", context: new UnleashContext
    {
        UserId = currentUserId,
    });

    Console.WriteLine($"Feature 'my-feature' enabled? {isEnabled}");

    var variant = unleash.GetVariant("my-feature", context: new UnleashContext
    {
        UserId = currentUserId,
    });

    Console.WriteLine($"Variant for user {currentUserId}: {variant.Name}");
    Console.WriteLine($"Payload: {variant.Payload?.Value}");

    Thread.Sleep(5000);
}