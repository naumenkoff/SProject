using System.Diagnostics.CodeAnalysis;

namespace SProject.Vdf.Abstractions;

public class VdfNode
{
    public required VdfCollection<VdfContainer> AllContainers { get; init; }
    public required VdfCollection<VdfValue> AllObjects { get; init; }

    public required VdfContainer? Root { get; init; }

    [MemberNotNullWhen(false, nameof(Root))]
    public bool Empty => Root is null || AllContainers.IsEmpty;

    [MemberNotNullWhen(true, nameof(Root))]
    public bool HasValues => Root is not null && AllObjects.IsNotEmpty;
}