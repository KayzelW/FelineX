var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("cache")
    .WithRedisInsight()
    .WithRedisCommander()
    .WithDataVolume(isReadOnly: false)
    .WithPersistence(TimeSpan.FromMinutes(5), 5);

// var apiService = builder.AddProject<Projects.Aspire_ApiService>("apiservice");
var username = builder.AddParameter("DbUsername");
var password = builder.AddParameter("DbUserPassword");

var postrges = builder
    .AddPostgres("postrges", userName: username, password: password)
    .WithDataVolume(isReadOnly: false);
var db = postrges
    .AddDatabase("FelineX");

builder.AddProject<Projects.Web>("web")
    // .WithReplicas(2)
    // .WithExternalHttpEndpoints()
    .WithReference(redis).WaitFor(redis)
    .WithReference(db).WaitFor(db);

builder.Build().Run();