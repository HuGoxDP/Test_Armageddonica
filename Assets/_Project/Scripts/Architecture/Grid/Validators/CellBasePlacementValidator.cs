using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Core.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Architecture.Grid.Validators
{
    public class CellAvailableValidator : BasePlacementValidator
    {
        protected override bool ValidateSpellCard(SpellCardData cardData, IGridCell cell)
        {
            if (cell.OccupiedEntity == null) return false;
            if (!ValidateTargetType(cardData, cell)) return false;

            foreach (var statType in cardData.RequiredStats)
            {
                if (!cell.OccupiedEntity.HasStat(statType))
                {
                    return false;
                }
            }

            return true;
        }

        protected override bool ValidateEntityCard(EntityCardData cardData, IGridCell cell)
        {
            return cell.OccupiedEntity == null;
        }
    }
}