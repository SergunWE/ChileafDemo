namespace ChileafBleXamarin.Interfaces
{
    public interface IBTDevice
    {
        string Name { get; }
        string Address { get; }
        object Native { get; }
    }
}
