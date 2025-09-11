using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Entities.Base;
using _Project.Scripts.Architecture.Enums;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Architecture.Tooltip
{
    public class StatsTooltipUI : MonoBehaviour
    {
       [SerializeField] private TextMeshProUGUI _titleText;
       [SerializeField] private TextMeshProUGUI _descriptionText;
       [SerializeField] private TextMeshProUGUI _statsText;
       [SerializeField] private RectTransform _rectTransform;
    
       private Entity _entity;
       private CanvasGroup _canvasGroup;
       private void Awake()
       {
           _canvasGroup = GetComponent<CanvasGroup>();
       }
       private void Start()
       {
           ServiceLocator.Register(this);
           HideTooltip();
       }

       private void Update()
       {
           var position = Input.mousePosition;
           transform.position = position + new Vector3(10, 0);
       }

       public void HideTooltip()
       {
           _entity = null;
           _titleText.text = string.Empty;
           _descriptionText.text = string.Empty;
           _statsText.text = string.Empty;
           gameObject.SetActive(false);
       }

       public void ShowTooltip()
       {
           gameObject.SetActive(true);
       }
       
       public void SetTooltip(Entity entity)
       {
           if (_entity != null)
           {
               _entity.CardData.Stats.OnStatChanged -= OnStatChanged;
           }
           
           _entity = entity; 
           SetTitle(entity.CardData.CardName);
           SetDescription(entity.CardData.Description);
           SetStats(entity.StatsLabel);
           RecalculateRectTransformSize();
           
           _entity.CardData.Stats.OnStatChanged += OnStatChanged;
       }

       private void RecalculateRectTransformSize()
       {

           var sizeTitle = _titleText.GetRenderedValues(false);
           var sizeDescription = _descriptionText.GetRenderedValues(false);
           var sizeStats = _statsText.GetRenderedValues(false);
           var x = Mathf.Max(sizeTitle.x, sizeDescription.x, sizeStats.x);
           var y = (sizeTitle.y + sizeDescription.y + sizeStats.y);
           var width = x + 10;
           var height = y + 10;
           //_rectTransform.sizeDelta = new Vector2(width, height);
       }

       private void OnStatChanged(StatType type, float value)
       {
           SetTitle(_entity.StatsLabel);
           RecalculateRectTransformSize();
       }

       private void SetTitle(string title)
       {
           _titleText.SetText(title);
           _titleText.ForceMeshUpdate();
       }

       private void SetDescription(string description)
       {
           _descriptionText.SetText(description);
           _descriptionText.ForceMeshUpdate();
       }

       private void SetStats(string stats)
       {
           _statsText.SetText(stats);
           _statsText.ForceMeshUpdate();
       }
    }
    
    
    
}