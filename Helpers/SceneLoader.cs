using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace GameFramework
{
    public class SceneLoader : ASystem
    {
        private AsyncOperationHandle<SceneInstance> _lastHandle;
        private List<AsyncOperationHandle<SceneInstance>> _handles;
        private Action _callback;
        protected override void Initialise()
        {
            _handles = new List<AsyncOperationHandle<SceneInstance>>();
        }

        public void LoadScene(string sceneName, LoadSceneMode mode, Action onComplete = null)
        {
            Logger.Log<SceneLoader>($"Loading scene {sceneName}");
            _callback = onComplete;
            _lastHandle = Addressables.LoadSceneAsync(sceneName, mode, false);
            _lastHandle.Completed += OnSceneLoadCompleted;
        }

        public void UnloadScene(string sceneName, Action onComplete = null)
        {
            Logger.Log<SceneLoader>($"Unload scene {sceneName}");
            
            _callback = onComplete;

            var handle = GetHandle(sceneName);

            if (!handle.IsValid())
            {
                Logger.LogError<SceneLoader>($"Failed to find valid handle to unload for scene {sceneName}, trying to unload manually");
                UnloadSceneManually(sceneName);
                return;
            }
            
            _lastHandle = Addressables.UnloadSceneAsync(handle, false);
            _lastHandle.Completed += OnSceneUnloadCompleted;
        }

        
        //todo: why? This might only be an editor thing so check.
        private void UnloadSceneManually(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName).completed += _ =>
            {
                _callback?.Invoke();
                Addressables.Release(_lastHandle);
            };
        }

        private void OnSceneLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
        {
            switch (obj.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    Logger.Log<SceneLoader>($"{obj.Result.Scene.name} load operation complete!");
                    
                    obj.Result.ActivateAsync().completed += _ =>
                    {
                        _callback?.Invoke();
                        _handles.Add(obj);
                        _callback = null;
                        _lastHandle.Completed -= OnSceneLoadCompleted;
                    };
                    break;
                case AsyncOperationStatus.Failed:
                    Logger.LogError<SceneLoader>($"Failed scene load operation");
                    throw obj.OperationException;
                case AsyncOperationStatus.None:
                default:
                    Logger.LogError<SceneLoader>("Something went wrong with scene load operation!");
                    throw obj.OperationException;
            }

        }
        
        private void OnSceneUnloadCompleted(AsyncOperationHandle<SceneInstance> obj)
        {
            switch (obj.Status)
            {
                case AsyncOperationStatus.Succeeded:
                    Logger.Log<SceneLoader>($"{obj.Result.Scene.name} unload operation complete!");
                    _handles.Remove(obj);
                    _callback?.Invoke();
                    break;
                case AsyncOperationStatus.Failed:
                    Logger.LogError<SceneLoader>($"Failed scene load operation");
                    throw obj.OperationException;
                case AsyncOperationStatus.None:
                default:
                    Logger.LogError<SceneLoader>("Something went wrong with scene load operation!");
                    throw obj.OperationException;
            }

            _callback = null;
            _lastHandle.Completed -= OnSceneUnloadCompleted;
            Addressables.Release(obj);
        }
        
        private AsyncOperationHandle<SceneInstance> GetHandle(string sceneName) =>
            _handles.FirstOrDefault(x => x.Result.Scene.name == sceneName);
    }
}