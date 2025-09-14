using _Project.Scripts.Architecture.Core.Dependency_Injection;
using UnityEngine;
using UnityEngine.VFX;

namespace _Project.Scripts.Architecture.VFXManager
{
    public class VFXReturnToPool : MonoBehaviour
    {
        private string _effectName;
        private VFXManager VFXManager => _vfxManager ??= ServiceLocator.Get<VFXManager>();
        private VFXManager _vfxManager;
        
        public void Initialize(string name)
        {
            _effectName = name;
        }

        void OnDisable()
        {
            VisualEffect vfx = GetComponent<VisualEffect>();
            if (vfx != null) vfx.Stop();
        }

        public void OnVFXFinished()
        {
            if (gameObject.activeInHierarchy)
            {
                VFXManager.ReturnEffectToPool(gameObject, _effectName);
            }
        }
    }
}