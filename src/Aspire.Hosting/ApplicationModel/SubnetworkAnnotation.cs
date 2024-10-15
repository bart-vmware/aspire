// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Aspire.Hosting.ApplicationModel;

/// <summary>
/// Associates a subnet resource with the containing resource.
/// </summary>
/// <param name="subnet">The subnet resource.</param>
public class SubnetworkAnnotation(SubnetworkResource subnet) : IResourceAnnotation
{
    /// <summary>
    /// The subnet resource.
    /// </summary>
    public SubnetworkResource Subnetwork => subnet;
}
