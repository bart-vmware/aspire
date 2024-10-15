// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Aspire.Hosting.ApplicationModel;

/// <summary>
/// Represents a network resource.
/// </summary>
/// <param name="name">The name of the resource.</param>
public class NetworkResource(string name) : Resource(name)
{
}
