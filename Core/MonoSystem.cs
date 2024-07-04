using System;
using UnityEngine;

namespace GameFramework
{
    public abstract class MonoSystem : MonoBehaviour, ICanGetSystem
    {
        private IArchitecture _architecture;
        
        protected void Register(IArchitecture architecture)
        {
            _architecture = architecture;
            architecture.AddMono(this);
        }

        protected virtual void Awake()
        {
            if (TargetArchitecture == null) return;
            Register(TargetArchitecture);
            
            Initialise();
        }

        protected TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            var found = _architecture.GetSystem<TSystem>();
            if (found == null)
                throw new ArgumentNullException($"No system registered of type {typeof(TSystem)}");
            return found;
        }

        protected TMono GetMono<TMono>() where TMono : MonoSystem
        {
            var found = _architecture.GetMono<TMono>();
            if (found == null)
                throw new ArgumentNullException($"No system registered of type {typeof(TMono)}");
            return found;
        }

        public IArchitecture GetArchitecture()
        {
            return TargetArchitecture;
        }

        public void Init()
        {
            Initialise();
        }

        protected abstract void Initialise();

        protected virtual IArchitecture TargetArchitecture => null;
    }
}