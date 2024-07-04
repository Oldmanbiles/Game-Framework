using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GameFramework
{
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {

        private HashSet<ISystem> _systems;
        private HashSet<MonoSystem> _components;
        private IOCContainer _iocContainer;

        private static T _architecture;
        public static IArchitecture Interface
        {
            get
            {
                if (_architecture == null)
                    ValidateArchitecture();

                return _architecture;
            }
        }

        public static void ValidateArchitecture()
        {
            if (_architecture != null) return;

            Logger.Log<Architecture<T>>("Validating Architecture");

            _architecture = new T
            {
                _iocContainer = new IOCContainer()
            };

            _architecture.Init();
        }

        protected abstract void Init();


        public void AddMono<TMono>(TMono component) where TMono : MonoSystem
        {
            _components ??= new HashSet<MonoSystem>();

            Logger.Log<Architecture<T>>($"Attempting to register Mono {component.GetType()}");

            _components.Add(component);
            _iocContainer.Register<TMono>(component);

            Logger.Log<Architecture<T>>($"Initialising {component.GetType()}");

            component.GetOrAddComponent<UnRegisterOnDestroy>().AddTarget(_architecture, component);

            Logger.Log<Architecture<T>>($"Successfully Added Mono {component.GetType()}");
        }

        public void RemoveMono<TMono>(TMono component) where TMono : MonoSystem
        {
            Logger.Log<Architecture<T>>($"Attempting to remove Mono {component.GetType()}");
            _iocContainer.UnRegister(component);
            Logger.Log<Architecture<T>>($"Successfully Removed Mono {component.GetType()}");
        }

        public void AddSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            _systems ??= new HashSet<ISystem>();
            Logger.Log<Architecture<T>>($"Attempting to register System {typeof(TSystem)}");

            system.SetArchitecture(this);
            _iocContainer.Register<TSystem>(system);
            _systems.Add(system);

            Logger.Log<Architecture<T>>($"Initialising System {typeof(TSystem)}");

            system.Init();

            Logger.Log<Architecture<T>>($"Successfully registered System {typeof(TSystem)}");
        }

        public TMono GetMono<TMono>() where TMono : MonoSystem
        {
            return _iocContainer.Get<TMono>();
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return _iocContainer.Get<TSystem>();
        }

        public List<TSystem> GetSystemsOfType<TSystem>() where TSystem : class
        {
            return _iocContainer.GetAll<TSystem>();
        }
    }
}