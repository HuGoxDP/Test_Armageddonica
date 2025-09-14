using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Entities.Base;
using _Project.Scripts.Architecture.Enums;
using PrimeTween;
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
       
       [Header( "TooltipSettings" )]
       [SerializeField] private float _maxWidth = 200f;
       [SerializeField] private float _maxHeight = 800f;
       [SerializeField] private Vector2 _padding = new Vector2(10, 10);
       [SerializeField] private float _verticalSpacing = 5f;

       private Entity _entity;
       private CanvasGroup _canvasGroup;
       private Tween _tween;
       private bool _isEnable = true; 
       
       private RectTransform _tooltipRect;
       private Camera _camera;

       private void Awake()
       {
           ServiceLocator.Register(this);
           _canvasGroup = GetComponent<CanvasGroup>();
           _tooltipRect = GetComponent<RectTransform>();
           _camera = Camera.main;
       }
       
       private void Start()
       {
           HideTooltip();
       }
       
       private void OnDestroy()
       {
           _tween.Stop();

           if (_entity != null && _entity.CardData != null && _entity.CardData.Stats != null)
           {
               _entity.CardData.Stats.OnStatChanged -= OnStatChanged;
           }
       }
       
       private void Update()
       {
           if (_canvasGroup.alpha <= 0.1) return;
           var position = Input.mousePosition;

           RectTransformUtility.ScreenPointToWorldPointInRectangle(_tooltipRect, position + new Vector3(10, 0), _camera, out var worldPosition);
           transform.position = worldPosition;
       }

       public void HideTooltip()
       {
           if (_isEnable)
           {
               if (_entity != null && _entity.CardData != null && _entity.CardData.Stats != null)
               {
                   _entity.CardData.Stats.OnStatChanged -= OnStatChanged;
               }

               _entity = null;
               _titleText.text = string.Empty;
               _descriptionText.text = string.Empty;
               _statsText.text = string.Empty;

               _tween.Stop();
               _tween = Tween.Alpha(_canvasGroup, 0, 0.2f);
               _isEnable = false;
           }
       }

       public void ShowTooltip()
       {
           if (!_isEnable)
           {
               _tween.Stop();
               _tween = Tween.Alpha(_canvasGroup, 1, 0.2f);
               _isEnable = true;
           }
       }
       
       public void SetTooltip(Entity entity)
       {
           if (entity == null || entity.CardData == null || entity.CardData.Stats == null)
           {
               Debug.LogWarning("Attempting to set tooltip with null entity or missing data");
               HideTooltip();
               return;
           }
           
           if (_entity != null && _entity.CardData != null && _entity.CardData.Stats != null)
           {
               _entity.CardData.Stats.OnStatChanged -= OnStatChanged;
           }
           
           _entity = entity; 
           
           try
           {
               _entity.CardData.Stats.OnStatChanged += OnStatChanged;
           }
           catch (System.Exception e)
           {
               Debug.LogError($"Failed to subscribe to stat changes: {e.Message}");
               HideTooltip();
               return;
           }
           
           SetTitle(entity.CardData.CardName);
           SetDescription(entity.CardData.Description);
           SetStats(entity.StatsLabel);
           RecalculateRectTransformSize();
       }

       private void RecalculateRectTransformSize()
       {
           if (_titleText == null || _descriptionText == null || _statsText == null || _rectTransform == null)
           {
               Debug.LogWarning("UI elements are not assigned in StatsTooltipUI");
               return;
           }
           
           
           _titleText.ForceMeshUpdate();
           _descriptionText.ForceMeshUpdate();
           _statsText.ForceMeshUpdate();
           
           var titleSize = _titleText.GetPreferredValues();
           var descriptionSize = _descriptionText.GetPreferredValues();
           var statsSize = _statsText.GetPreferredValues();

           float titleWidth = titleSize.x;
           float titleHeight = titleSize.y;
            
           float descriptionWidth = descriptionSize.x;
           float descriptionHeight = descriptionSize.y;
            
           float statsWidth = statsSize.x;
           float statsHeight = statsSize.y;

           // Find the maximum width
           float maxWidth = Mathf.Max(titleWidth, descriptionWidth, statsWidth);
            
           // Calculate total height with spacing
           float totalHeight = titleHeight + _verticalSpacing + descriptionHeight + _verticalSpacing + statsHeight;
                               
           float width = maxWidth + _padding.x * 2;
           float height = totalHeight + _padding.y * 2;
           
           float maxWidthWithPadding = _maxWidth - _padding.x * 2;  
           float maxHeightWithPadding = _maxHeight - _padding.y * 2;
           
           width = Mathf.Min(width, maxWidthWithPadding);
           height = Mathf.Min(height, maxHeightWithPadding);
           
           _rectTransform.sizeDelta = new Vector2(width, height);
       }

       private void OnStatChanged(StatType type, float value)
       {
           if (_entity == null || _entity.CardData == null)
           {
               HideTooltip();
               return;
           }
           
           SetTitle(_entity.StatsLabel);
           RecalculateRectTransformSize();
       }

       private void SetTitle(string title)
       {
           if (_titleText != null) 
               _titleText.SetText(title);
       }

       private void SetDescription(string description)
       {
           if (_descriptionText != null) 
               _descriptionText.SetText(description);
       }

       private void SetStats(string stats)
       {
           if (_statsText != null)
               _statsText.SetText(stats);
       }
    }
    
    
    
}