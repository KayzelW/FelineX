var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithRedisCommander()
    .WithDataVolume(isReadOnly: false)
    .WithPersistence(TimeSpan.FromMinutes(5), 5);

var username = builder.AddParameter("DbUsername");
var password = builder.AddParameter("DbUserPassword");
var postrges = builder
    .AddPostgres("postrges", userName: username, password: password)
    .WithDataVolume(isReadOnly: false);
var db = postrges
    .AddDatabase("FelineX");

var apiService = builder.AddProject<Projects.Aspire_ApiService>("apiservice")
    .WithReference(db).WaitFor(db);

builder.AddProject<Projects.Aspire_Web>("webfrontend")
    // .WithReplicas(2)
    .WithExternalHttpEndpoints()
    .WithReference(cache).WaitFor(cache)
    .WithReference(apiService).WaitFor(apiService);

builder.Build().Run();
