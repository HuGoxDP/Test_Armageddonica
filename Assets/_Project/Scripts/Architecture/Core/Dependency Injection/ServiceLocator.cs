using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.Dependency_Injection
{
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator _instance;
        private readonly Dictionary<Type, object> _services = new();

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public static T Get<T>() where T : class
        {
            if (_instance._services.TryGetValue(typeof(T), out var service))
                return service as T;
            return null;
        }
    
        public static void Register<T>(T service) where T : class
        {
            Debug.Log($"Service {typeof(T).Name} registered.");
            _instance._services[typeof(T)] = service;
        }

    }
}