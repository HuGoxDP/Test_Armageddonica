using System.Collections.Generic;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Entities.Base;
using _Project.Scripts.Architecture.Enums;
using _Project.Scripts.Architecture.Grid.Core;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core
{
    public class ArmyStatUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void CalculateStrength(GridSystem gridSystem)
        {
            float armyStrength = 0;
            foreach (var cell in gridSystem.Cells)
            {
                var entity = cell.OccupiedEntity;
                if (entity == null) continue;
                armyStrength += entity.RecalculateStrength();
            }
            _text.text =   $"Army strength: {armyStrength:F2}";
        }
    }
}