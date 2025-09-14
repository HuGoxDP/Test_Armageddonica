using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Entities.Base;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private StatMultiplierConfig _multiplierConfig;

        private void Awake()
        {
            ServiceLocator.Register<StatMultiplierConfig>(_multiplierConfig);
        }
    }
}