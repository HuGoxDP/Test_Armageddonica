using System.Collections.Generic;
using _Project.Scripts.Architecture.Core.Dependency_Injection;
using UnityEngine;
using UnityEngine.VFX;

namespace _Project.Scripts.Architecture.VFXManager
{
    public class VFXManager : MonoBehaviour
    {
        public List<VFXPool> vfxPools;
        private readonly Dictionary<string, VFXPool> _poolDictionary = new Dictionary<string, VFXPool>();

        private void Awake()
        {
            ServiceLocator.Register(this);
            InitializePools();
        }

        private void InitializePools()
        {
            foreach (VFXPool pool in vfxPools)
            {
                for (int i = 0; i < pool.poolSize; i++)
                {
                    GameObject obj = Instantiate(pool.prefab, transform, true);
                    obj.SetActive(false);
                    pool.availableObjects.Enqueue(obj);

                    var returnToPool = obj.AddComponent<VFXReturnToPool>();
                    returnToPool.Initialize(pool.effectName);
                }

                _poolDictionary.Add(pool.effectName, pool);
            }
        }

        public GameObject PlayEffect(string effectName, Vector3 position, Quaternion rotation, float duration = 0,
            bool isInfinite = false)
        {
            if (!_poolDictionary.ContainsKey(effectName))
            {
                Debug.LogWarning($"Effect with name {effectName} not found in pool");
                return null;
            }

            VFXPool pool = _poolDictionary[effectName];

            if (pool.availableObjects.Count == 0)
            {
                Debug.LogWarning($"Empty pool {pool.effectName}. Cannot play effect.");
                return null;
            }

            GameObject effect = pool.availableObjects.Dequeue();
            effect.transform.position = position;
            effect.transform.rotation = rotation;
            effect.SetActive(true);


            VisualEffect vfx = effect.GetComponent<VisualEffect>();
            if (vfx != null)
            {
                if (isInfinite)
                {
                    pool.activeInfiniteEffects.Add(effect);
                }

                vfx.Play();

                if (duration <= 0 && !isInfinite)
                {
                    duration = 2f;
                }
            }


            if (!isInfinite && duration > 0)
            {
                StartCoroutine(ReturnToPoolAfterDelay(effect, effectName, duration));
            }

            return effect;
        }

        public void StopEffect(GameObject effect, string effectName)
        {
            if (!_poolDictionary.ContainsKey(effectName))
            {
                Debug.LogWarning($"Effect with name {effectName} not found in pool");
                return;
            }

            VFXPool pool = _poolDictionary[effectName];

            if (pool.activeInfiniteEffects.Contains(effect))
            {
                pool.activeInfiniteEffects.Remove(effect);
            }

            ReturnEffectToPool(effect, effectName);
        }

        public void StopAllEffectsOfType(string effectName)
        {
            if (!_poolDictionary.ContainsKey(effectName))
            {
                Debug.LogWarning($"Effect with name {effectName} not found in pool");
                return;
            }

            VFXPool pool = _poolDictionary[effectName];

            List<GameObject> effectsToStop = new List<GameObject>(pool.activeInfiniteEffects);

            foreach (GameObject effect in effectsToStop)
            {
                StopEffect(effect, effectName);
            }
        }

        private System.Collections.IEnumerator ReturnToPoolAfterDelay(GameObject effect, string effectName, float delay)
        {
            yield return new WaitForSeconds(delay);

            ReturnEffectToPool(effect, effectName);
        }

        public void ReturnEffectToPool(GameObject effect, string effectName)
        {
            if (!_poolDictionary.ContainsKey(effectName))
            {
                Debug.LogWarning($"Effect with name {effectName} not found in pool");
                return;
            }

            VFXPool pool = _poolDictionary[effectName];


            VisualEffect vfx = effect.GetComponent<VisualEffect>();
            if (vfx != null)
            {
                vfx.Stop();
            }


            effect.SetActive(false);
            effect.transform.SetParent(transform);
            _poolDictionary[effectName].availableObjects.Enqueue(effect);
        }
    }
}