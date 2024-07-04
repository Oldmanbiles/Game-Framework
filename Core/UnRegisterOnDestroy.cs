using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class UnRegisterOnDestroy :  MonoBehaviour
    {
        private Dictionary<IArchitecture, List<MonoSystem>> _unregisterTargets;

        public void AddTarget(IArchitecture targetArchitecture, MonoSystem targetMono)
        {
            _unregisterTargets ??= new Dictionary<IArchitecture, List<MonoSystem>>();

            if (_unregisterTargets.ContainsKey(targetArchitecture))
            {
                _unregisterTargets[targetArchitecture].Add(targetMono);
            }
            else
            {
                _unregisterTargets.Add(targetArchitecture,new List<MonoSystem>(){targetMono});
            }
        }
        
        private void OnDestroy()
        {
            foreach (var targets in _unregisterTargets)
            {
                targets.Value.ForEach(x => x.GetArchitecture()?.RemoveMono(x));
            }
        }
    }
}