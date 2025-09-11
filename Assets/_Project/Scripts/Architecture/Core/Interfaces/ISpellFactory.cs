using _Project.Scripts.Architecture.Cards.Data;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.Interfaces
{
    public interface ISpellFactory
    {
        Spell.Base.Spell CreateSpell(SpellCardData cardData, Transform cellTransform);
    }
}