namespace GameFramework
{
    public interface ISystem: ICanSetArchitecture
    {
        IArchitecture GetArchitecture();
        void Init();
    }
}