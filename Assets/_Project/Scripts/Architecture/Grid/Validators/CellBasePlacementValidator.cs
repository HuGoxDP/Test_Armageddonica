using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Grid.Core;
using UnityEngine;

namespace _Project.Scripts.Architecture.Grid.Validators
{
    public class CellAvailableValidator : BasePlacementValidator
    {
        public override bool Validate(BaseCardData cardData, IGridCell cell)
        {
            return !cell.IsOccupied;
        }
    }
}