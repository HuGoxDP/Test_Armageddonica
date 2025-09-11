using _Project.Scripts.Architecture.Cards.Data;
using UnityEngine;

namespace _Project.Scripts.Architecture.Spell
{
    public interface ISpellFactory
    {
        Base.Spell CreateSpell(SpellCardData cardData, Transform cellTransform);
    }
}