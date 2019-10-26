namespace Gestion.Services
{
    public interface ILogger
    {
        void Log(EventType e, string description);
    }
}
