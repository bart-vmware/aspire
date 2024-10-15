// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Aspire.Hosting.ApplicationModel;

/// <summary>
/// Represents a subnet with a network.
/// </summary>
/// <param name="name">The name of the subnet.</param>
/// <param name="parent">The <see cref="NetworkResource"/> parent.</param>
public class SubnetworkResource(string name, NetworkResource parent) : Resource(name), IResourceWithParent<NetworkResource>
{
    /// <summary>
    /// The <see cref="NetworkResource"/> parent.
    /// </summary>
    public NetworkResource Parent => parent;
}
