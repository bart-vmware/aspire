// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
var builder = DistributedApplication.CreateBuilder(args);

var vnet = builder.AddNetwork("vnet")
                  .AddSubnet("subnet");

// Testing secret parameters
var param = builder.AddParameter("secretparam", "fakeSecret", secret: true);

// Testing volumes
var redis = builder.AddRedis("cache")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithNetwork(vnet);

// Testing secret outputs
var cosmosDb = builder.AddAzureCosmosDB("account")
                      .WithNetwork(vnet)
                      .RunAsEmulator(c => c.WithLifetime(ContainerLifetime.Persistent))
                      .AddDatabase("db");

// Testing a connection string
var blobs = builder.AddAzureStorage("storage")
                   .RunAsEmulator(c => c.WithLifetime(ContainerLifetime.Persistent))
                   .AddBlobs("blobs");

builder.AddProject<Projects.AzureVNetPlayground_ApiService>("api")
       .WithExternalHttpEndpoints()
       .WithReference(blobs)
       .WithReference(redis)
       .WithReference(cosmosDb)
       .WithEnvironment("VALUE", param)
       .PublishAsAzureContainerApp((module, app) =>
       {
           // Scale to 0
           app.Template.Value!.Scale.Value!.MinReplicas = 0;
       })
       .WithNetwork(vnet);

#if !SKIP_DASHBOARD_REFERENCE
// This project is only added in playground projects to support development/debugging
// of the dashboard. It is not required in end developer code. Comment out this code
// or build with `/p:SkipDashboardReference=true`, to test end developer
// dashboard launch experience, Refer to Directory.Build.props for the path to
// the dashboard binary (defaults to the Aspire.Dashboard bin output in the
// artifacts dir).
builder.AddProject<Projects.Aspire_Dashboard>(KnownResourceNames.AspireDashboard);
#endif

builder.Build().Run();

