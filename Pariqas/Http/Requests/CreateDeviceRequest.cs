using Pariqas.Models.Devices;

namespace Pariqas.Http.Requests;

public record CreateDeviceRequest(string Name, DeviceType Type);