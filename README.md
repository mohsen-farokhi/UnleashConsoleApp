Comprehensive Guide to Using Unleash with .NET
1. Introduction
This document explains how to install and configure Unleash Feature Flag Server using Docker, connect it to a .NET application, create and test Feature Flags, implement percentage rollouts, and use variants. Goal: precise feature control and reducing the risk of changes in different environments.
________________________________________
2. Installing and Running Unleash with Docker
2.1. Docker Compose
A Docker Compose setup to quickly prepare the development environment:
services:
  unleash-db:
    image: postgres:14
    container_name: unleash-db
    restart: always
    environment:
      POSTGRES_USER: unleash_user
      POSTGRES_PASSWORD: unleash_password
      POSTGRES_DB: unleash
    volumes:
      - unleash_data:/var/lib/postgresql/data
    ports:
      - "5432:5432"

  unleash:
    image: unleashorg/unleash-server:latest
    container_name: unleash-server
    restart: always
    environment:
      DATABASE_HOST: unleash-db
      DATABASE_NAME: unleash
      DATABASE_USERNAME: unleash_user
      DATABASE_PASSWORD: unleash_password
      DATABASE_SSL: "false"
      LOG_LEVEL: info
      INIT_ADMIN_USER: admin
      INIT_ADMIN_EMAIL: admin@unleash.local
      INIT_ADMIN_PASSWORD: unleash4all
      BASE_URL: http://localhost:4242
    ports:
      - "4242:4242"
    depends_on:
      - unleash-db

volumes:
  unleash_data:
2.2. Accessing the Admin Panel
•	URL: http://localhost:4242/
✅ Advantage: Quick and easy feature testing in development.
________________________________________
3. Connecting a .NET Application to Unleash
3.1. Install NuGet Package
dotnet add package Unleash
3.2. Configure Connection
using Unleash;

var unleash = new DefaultUnleash(new UnleashSettings
{
    AppName = "my-app",
    InstanceTag = "development",
    UnleashApi = new Uri("http://localhost:4242/api/"),
    CustomHttpHeaders = new Dictionary<string, string>
    {
        { "Authorization", "*:development.YOUR_TOKEN_HERE" }
    },
});
✅ Advantage: The .NET application can receive feature flags in real-time.
________________________________________
4. Creating a Feature Flag
4.1. In the Admin Panel
1.	Click Create Feature
2.	Name: my-feature
3.	Enable the feature
4.	Add a Strategy (UserId, Gradual Rollout, etc.)
✅ Advantage: Control features without code changes or redeployment.
________________________________________
5. Activating a Feature for Specific Users
5.1. In the Admin Panel
•	Strategy: UserWithId
•	List of userIds: 123, 456
5.2. .NET Code
string currentUserId = "123";
bool isEnabled = unleash.IsEnabled("my-feature", ctx => ctx.UserId = currentUserId);
Console.WriteLine($"Feature active for user {currentUserId}: {isEnabled}");
✅ Advantage: Specific users always see the feature.
________________________________________
6. Percentage Rollout
6.1. In the Admin Panel
•	Strategy: Flexible Rollout
•	Percentage: 50%
•	Stickiness: userId
✅ Advantage: Reduce risk, gradual testing, easy A/B testing.
________________________________________
7. Using Variants
7.1. In the Admin Panel
•	Go to Feature → Variants → Add Variant

•	Example:
o	red-banner → { "color": "red" }
o	blue-banner → { "color": "blue" }
o	green-banner → { "color": "green" }
7.2. .NET Code
var variant = unleash.GetVariant("my-feature", ctx => ctx.UserId = currentUserId);
Console.WriteLine($"Variant for user {currentUserId}: {variant.Name}");
Console.WriteLine($"Payload: {variant.Payload?.Value}");
✅ Advantage: Personalized user experience, easy A/B testing, gradual rollout for different variants.
________________________________________
8. Common Issues
1.	Always seeing the same variant (due to Stickiness and UserId).

2.	Feature not active → possible reasons: Feature not created, wrong name, SDK not yet synced.
________________________________________
9. Step-by-Step Flow Diagram
+---------------------------+
| 1. Install Docker & Compose|
+---------------------------+
             |
             v
+---------------------------+
| 2. Start Unleash          |
| - docker-compose up -d     |
| - Access admin panel: 4242|
+---------------------------+
             |
             v
+---------------------------+
| 3. Create Feature Flag     |
| - Name: my-feature        |
| - Enable                  |
| - Select Strategy         |
+---------------------------+
             |
             v
+---------------------------+
| 4. Define Strategies       |
| - UserWithId               |
| - Flexible Rollout         |
| - Combine Rollout & UserId |
+---------------------------+
             |
             v
+---------------------------+
| 5. Define Variants         |
| - red-banner, blue-banner  |
| - Custom payload           |
| - Limit by % or UserId     |
+---------------------------+
             |
             v
+---------------------------+
| 6. Connect .NET to Unleash|
| - DefaultUnleash           |
| - ApiKey, AppName          |
+---------------------------+
             |
             v
+---------------------------+
| 7. Use in Application      |
| - IsEnabled (Feature)      |
| - GetVariant (Variant)     |
| - Combine UserId & Rollout |
+---------------------------+
             |
             v
+---------------------------+
| 8. Test and Collect Data   |
| - Rollout % and variants   |
| - View logs and behavior   |
+---------------------------+
             |
             v
+---------------------------+
| 9. Gradual Release & Analyze|
| - Increase rollout %        |
| - A/B Testing on variants   |
+---------------------------+
Overall Advantages
•	Precise feature control without redeployment
•	Reduced risk with gradual rollout
•	A/B testing and personalized user experience
•	Real data reporting and analysis
________________________________________

