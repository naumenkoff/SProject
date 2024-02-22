using System.Diagnostics.CodeAnalysis;

namespace SProject.VDF;

public sealed class VDocument
{
    public required VCollection<VSection> AllContainers { get; init; }
    public required VCollection<VValue> AllObjects { get; init; }

    public required VSection? Root { get; init; }

    [MemberNotNullWhen(false, nameof(Root))]
    public bool Empty => Root is null || AllContainers.IsEmpty;

    [MemberNotNullWhen(true, nameof(Root))]
    public bool HasValues => Root is not null && AllObjects.IsNotEmpty;
}