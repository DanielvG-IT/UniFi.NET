using System.Text.Json.Serialization;

namespace UniFi.Network.Client.Models;

[JsonConverter(typeof(JsonStringEnumConverter<DeviceState>))]
public enum DeviceState
{
    [JsonStringEnumMemberName("ONLINE")]
    Online,

    [JsonStringEnumMemberName("OFFLINE")]
    Offline,

    [JsonStringEnumMemberName("PENDING_ADOPTION")]
    PendingAdoption,

    [JsonStringEnumMemberName("UPDATING")]
    Updating,

    [JsonStringEnumMemberName("GETTING_READY")]
    GettingReady,

    [JsonStringEnumMemberName("ADOPTING")]
    Adopting,

    [JsonStringEnumMemberName("DELETING")]
    Deleting,

    [JsonStringEnumMemberName("CONNECTION_INTERRUPTED")]
    ConnectionInterrupted,

    [JsonStringEnumMemberName("ISOLATED")]
    Isolated,

    [JsonStringEnumMemberName("U5G_INCORRECT_TOPOLOGY")]
    U5gIncorrectTopology,
}
