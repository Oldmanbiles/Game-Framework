using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace GameFramework
{
    public static class DefinitionLibrary
    {
        private static List<CoreDefinition> _coreDefinitions;
        public static void LoadLibraryInstant()
        {
            _coreDefinitions = new List<CoreDefinition>();
            var task = Addressables.LoadAssetsAsync<CoreDefinition>("Definitions", so =>
            {
                _coreDefinitions.Add(so);
            }).WaitForCompletion();
            Addressables.Release(task);
        }

        public static async void LoadLibrary(Action onComplete = null)
        {
            Logger.Log("Loading Library Definitions");
            
            var task = Addressables.LoadAssetsAsync<CoreDefinition>("Definitions", so =>
            {
                _coreDefinitions.Add(so);
            });
            await task.Task;
            
            onComplete?.Invoke();
            
            Addressables.Release(task);
            
            Logger.Log("Finalising Library Definitions");
            
        }

        public static T GetRandomDefinition<T>() where T : CoreDefinition
        {
            if (_coreDefinitions.Count == 0) return default(T);
            return _coreDefinitions.Where(x => x is T).ElementAt(Random.Range(0, _coreDefinitions.Count)) as T;
        }

        public static List<T> GetAll<T>()
        {
            return _coreDefinitions.OfType<T>().ToList();
        }

        public static List<T> GetAll<T>(Func<T,bool> query)
        {
            return _coreDefinitions.OfType<T>().Where(query).ToList();
        }

        [CanBeNull]
        public static T GetDefinitionByName<T>(string name) where T : CoreDefinition
        {
            return _coreDefinitions.OfType<T>().FirstOrDefault(x => x.name == name);
        }

        private static string GetRandomLineFromTextFile(StreamReader reader)
        {
            string chosen = null;
            int numberSeen = 0;
            var rng = new System.Random();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (rng.Next(++numberSeen) == 0)
                {
                    chosen = line;
                }
            }
            return chosen;
        }
    }
}