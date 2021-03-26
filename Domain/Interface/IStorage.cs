namespace Domain.Interface
{
    public interface IStorage
    {
        IReceptionComponent Reception { get; }
        ISettingsComponent Settings { get; }
    }
}
