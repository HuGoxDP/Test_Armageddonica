using System;
using _Project.Scripts.Architecture.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Architecture.UI
{
    public class CardUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameTextMeshPro;
        [SerializeField] private TextMeshProUGUI _descriptionTextMeshPro;
        [SerializeField] private Image _cardImage;
        [SerializeField] private Image _cardTypeIcon;

        public BaseCardData CardData { get; private set; }
        
        public void SetCardData(BaseCardData cardData)
        {
            if (cardData == null) throw new ArgumentNullException(nameof(cardData), "Card data cannot be null");
            
            CardData = cardData;
            
            _nameTextMeshPro.text = cardData.CardName;
            _descriptionTextMeshPro.text = cardData.Description;
            _cardTypeIcon.sprite = cardData.CardTypeIcon;
            _cardImage.sprite = cardData.CardImage;
        }
    }
}