// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Aspire.Hosting.ApplicationModel;

namespace Aspire.Hosting;

/// <summary>
/// Provides extension methods for adding and configuring network resources to the <see cref="IDistributedApplicationBuilder"/> application model.
/// </summary>
public static class NetworkResourceBuilderExtensions
{
    /// <summary>
    /// Adds a network resource to the application model.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <param name="name">The name of the network resource.</param>
    /// <returns>A resource builder for the network.</returns>
    public static IResourceBuilder<NetworkResource> AddNetwork(this IDistributedApplicationBuilder builder, [ResourceName]string name)
    {
        var network = new NetworkResource(name);
        return builder.AddResource(network);
    }

    /// <summary>
    /// Defines a subnet within a network.
    /// </summary>
    /// <param name="builder">The resource builder for the parent network.</param>
    /// <param name="name">The name for the subnet.</param>
    /// <returns>A resource builder for the subnet.</returns>
    public static IResourceBuilder<SubnetworkResource> AddSubnet(this IResourceBuilder<NetworkResource> builder, [ResourceName]string name)
    {
        var subnet = new SubnetworkResource(name, builder.Resource);
        return builder.ApplicationBuilder.AddResource(subnet);
    }

    /// <summary>
    /// Connects a resource to a network.
    /// </summary>
    /// <typeparam name="T">The type of the resoure builder.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="subnet">The subnet resource builder.</param>
    /// <returns>A resource builder</returns>
    public static IResourceBuilder<T> WithNetwork<T>(this IResourceBuilder<T> builder, IResourceBuilder<SubnetworkResource> subnet) where T: IResourceWithEndpoints
    {
        return builder.WithAnnotation(new SubnetworkAnnotation(subnet.Resource));
    }
}
