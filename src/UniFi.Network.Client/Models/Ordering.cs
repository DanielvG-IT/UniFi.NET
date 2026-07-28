using System.Text.Json.Serialization;

namespace UniFi.Network.Client.Models;

public sealed class OrderedFirewallPolicyIds
{
    [JsonPropertyName("beforeSystemDefined")]
    public required IReadOnlyList<Guid> BeforeSystemDefined { get; init; }

    [JsonPropertyName("afterSystemDefined")]
    public required IReadOnlyList<Guid> AfterSystemDefined { get; init; }
}

public sealed class FirewallPolicyOrdering
{
    [JsonPropertyName("orderedFirewallPolicyIds")]
    public required OrderedFirewallPolicyIds OrderedFirewallPolicyIds { get; init; }
}

public sealed class AclRuleOrdering
{
    [JsonPropertyName("orderedAclRuleIds")]
    public required IReadOnlyList<Guid> OrderedAclRuleIds { get; init; }
}
