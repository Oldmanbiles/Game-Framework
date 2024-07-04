using System;

namespace GameFramework
{
    public abstract class ASystem : ISystem
    {
        private IArchitecture _architecture;

        public void SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
        }

        public IArchitecture GetArchitecture()
        {
            return _architecture;
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

        void ISystem.Init()
        {
            Initialise();
        }

        protected abstract void Initialise();
    }
}