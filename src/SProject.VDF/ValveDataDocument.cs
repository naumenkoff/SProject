using System.Diagnostics.CodeAnalysis;
using SProject.VDF.Collections;

namespace SProject.VDF;

public sealed class ValveDataDocument
{
    public required ValveDataCollection<ValveDataSection> Sections { get; init; }
    public required ValveDataCollection<ValveDataProperty> Properties { get; init; }
    public required ValveDataSection? PrimarySection { get; init; }

    [MemberNotNullWhen(true, nameof(PrimarySection))]
    public bool HasSections => PrimarySection is not null && !Sections.IsEmpty;

    [MemberNotNullWhen(true, nameof(PrimarySection))]
    public bool HasProperties => PrimarySection is not null && !Properties.IsEmpty;
}