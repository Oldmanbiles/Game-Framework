using UnityEngine;

namespace GameFramework
{
    public class DontDestroyMe : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}