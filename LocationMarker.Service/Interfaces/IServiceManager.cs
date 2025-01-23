namespace LocationMarker.Service.Interfaces
{
    public interface IServiceManager
    {
        ILocationService Location {  get; }
        IToolServiceV1 ToolV1 { get; }
    }
}
