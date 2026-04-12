namespace WildTracker.Contracts.Common;

/// <summary>Hypermedia link — tells the client what action it can take next (Richardson Level 3).</summary>
public record Link(string Rel, string Href, string Method);
