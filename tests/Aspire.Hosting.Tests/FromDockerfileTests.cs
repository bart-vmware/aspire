// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Aspire.Hosting.Utils;
using Xunit;

namespace Aspire.Hosting.Tests;

public class FromDockerfileTests
{
    [Fact]
    public void FromDockerfileWithEmptyContextPathThrows()
    {
        using var builder = TestDistributedApplicationBuilder.Create();

        var ex = Assert.Throws<ArgumentException>(() =>
        {
            builder.AddContainer("mycontainer", "myimage")
                   .FromDockerfile(string.Empty);
        });

        Assert.Equal("contextPath", ex.ParamName);
    }

    [Fact]
    public void FromDockerfileWithContextPathThatDoesNotExistThrowsDirectoryNotFoundException()
    {
        using var builder = TestDistributedApplicationBuilder.Create();

        var ex = Assert.Throws<DirectoryNotFoundException>(() =>
        {
            builder.AddContainer("mycontainer", "myimage")
                   .FromDockerfile("a/path/to/nowhere");
        });
    }

    [Fact]
    public void FromDockerfileWithValidContextPathAndEmptyDockerfilePathThrows()
    {
        using var builder = TestDistributedApplicationBuilder.Create();
        var tempContextPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempContextPath);

        var ex = Assert.Throws<FileNotFoundException>(() =>
        {
            builder.AddContainer("mycontainer", "myimage")
                   .FromDockerfile(tempContextPath, string.Empty);
        });
    }

    [Fact]
    public void FromDockerfileWithValidContextPathAndInvalidDockerfilePathThrows()
    {
        using var builder = TestDistributedApplicationBuilder.Create();
        var tempContextPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempContextPath);

        var ex = Assert.Throws<FileNotFoundException>(() =>
        {
            builder.AddContainer("mycontainer", "myimage")
                   .FromDockerfile(tempContextPath, "Notarealdockerfile");
        });
    }

    [Fact]
    public async Task FromDockerfileWithValidContextPathValidDockerfileWithImplicitDefaultNameSucceeds()
    {
        using var builder = TestDistributedApplicationBuilder.Create();
        var tempContextPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempContextPath);
        var tempDockerfilePath = Path.Combine(tempContextPath, "Dockerfile");
        await File.WriteAllTextAsync(tempDockerfilePath, HelloWorldDockerfile);

        var container = builder.AddContainer("mycontainer", "myimage")
                               .FromDockerfile(tempContextPath);

        var annotation = Assert.Single(container.Resource.Annotations.OfType<DockerfileBuildAnnotation>());
        Assert.Equal(tempContextPath, annotation.ContextPath);
        Assert.Equal(tempDockerfilePath, annotation.DockerfilePath);
    }

    [Fact]
    public async Task FromDockerfileWithValidContextPathValidDockerfileWithExplicitDefaultNameSucceeds()
    {
        using var builder = TestDistributedApplicationBuilder.Create();
        var tempContextPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempContextPath);
        var tempDockerfilePath = Path.Combine(tempContextPath, "Dockerfile");
        await File.WriteAllTextAsync(tempDockerfilePath, HelloWorldDockerfile);

        var container = builder.AddContainer("mycontainer", "myimage")
                               .FromDockerfile(tempContextPath, "Dockerfile");

        var annotation = Assert.Single(container.Resource.Annotations.OfType<DockerfileBuildAnnotation>());
        Assert.Equal(tempContextPath, annotation.ContextPath);
        Assert.Equal(tempDockerfilePath, annotation.DockerfilePath);
    }

    [Fact]
    public async Task FromDockerfileWithValidContextPathValidDockerfileWithExplicitNonDefaultNameSucceeds()
    {
        using var builder = TestDistributedApplicationBuilder.Create();
        var tempContextPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempContextPath);
        var tempDockerfilePath = Path.Combine(tempContextPath, "Otherdockerfile");
        await File.WriteAllTextAsync(tempDockerfilePath, HelloWorldDockerfile);

        var container = builder.AddContainer("mycontainer", "myimage")
                               .FromDockerfile(tempContextPath, "Otherdockerfile");

        var annotation = Assert.Single(container.Resource.Annotations.OfType<DockerfileBuildAnnotation>());
        Assert.Equal(tempContextPath, annotation.ContextPath);
        Assert.Equal(tempDockerfilePath, annotation.DockerfilePath);
    }

    [Fact]
    public async Task FromDockerfileWithValidContextPathValidDockerfileWithExplicitAbsoluteDefaultNameSucceeds()
    {
        using var builder = TestDistributedApplicationBuilder.Create();
        var tempContextPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempContextPath);
        var tempDockerfilePath = Path.Combine(tempContextPath, "Dockerfile");
        await File.WriteAllTextAsync(tempDockerfilePath, HelloWorldDockerfile);

        var container = builder.AddContainer("mycontainer", "myimage")
                               .FromDockerfile(tempContextPath, tempDockerfilePath);

        var annotation = Assert.Single(container.Resource.Annotations.OfType<DockerfileBuildAnnotation>());
        Assert.Equal(tempContextPath, annotation.ContextPath);
        Assert.Equal(tempDockerfilePath, annotation.DockerfilePath);
    }

    private const string HelloWorldDockerfile = """
        FROM mcr.microsoft.com/hello-world
        RUN echo 'Hello, world!'
        """;
}
