using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GameFramework
{
    public abstract class Architecture<T> : IArchitecture where T: Architecture<T>, new()
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
            
            Debug.Log("Validating Architecture");

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
            
            Debug.Log($"Attempting to register Mono {component.GetType()}");
            
            _components.Add(component);
            _iocContainer.Register<TMono>(component);
            
            Debug.Log($"Initialising {component.GetType()}");
            
            component.GetOrAddComponent<UnRegisterOnDestroy>().AddTarget(_architecture, component);
            
            Debug.Log($"Successfully Added Mono {component.GetType()}");
        }

        public void RemoveMono<TMono>(TMono component) where TMono : MonoSystem
        {
            Debug.Log($"Attempting to remove Mono {component.GetType()}");
            _iocContainer.UnRegister(component);
            Debug.Log($"Successfully Removed Mono {component.GetType()}");
        }

        public void AddSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            _systems ??= new HashSet<ISystem>();
            Debug.Log($"Attempting to register System {typeof(TSystem)}");
            
            system.SetArchitecture(this);
            _iocContainer.Register<TSystem>(system);
            _systems.Add(system);
            
            Debug.Log($"Initialising System {typeof(TSystem)}");
            
            system.Init();
            
            Debug.Log($"Successfully registered System {typeof(TSystem)}");
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