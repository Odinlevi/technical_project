using UnityEngine;

namespace Features.Utility
{
    public abstract class Singleton<T>: MonoBehaviour where T : Component
    {
        [SerializeField] private bool _dontDestroyOnLoad;

        private static T _instance;

        public static T Instance => _instance;

        public virtual void Awake()
        {
            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
            
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;

            Debug.Log("??");
        }
    }
}