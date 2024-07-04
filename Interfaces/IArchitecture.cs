using System.Collections.Generic;

namespace GameFramework
{
    public interface IArchitecture
    {
        void AddMono<T>(T component) where T : MonoSystem;
        void RemoveMono<T>(T component) where T : MonoSystem;
        void AddSystem<T>(T system) where T : ISystem;
        
        T GetMono<T>() where T : MonoSystem;
        T GetSystem<T>() where T : class, ISystem;
        List<T> GetSystemsOfType<T>() where T : class;

    }
}