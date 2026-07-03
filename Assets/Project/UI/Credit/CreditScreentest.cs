using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;

namespace SEHOON.UI
{
    [System.Serializable]
    public class CreditSectiontest
    {
        public LocalizedString  sectionTitle;
        public List<LocalizedString> names;
    }
    
    public class CreditScreentest : MonoBehaviour
    {
        
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private float _autoScrollSpeed = 20f;
        [SerializeField] private string _scrollViewName;
        [SerializeField] private string _contentViewName;
        [SerializeField] private string _closeButtonName;
        [SerializeField] private List<CreditSectiontest> _creditSections;
        
        private ScrollView _scrollView;
        private VisualElement _contentView;
        private bool _shouldAutoScroll = true;

        private readonly List<(LocalizedString loc, LocalizedString.ChangeHandler handler)> _bindings = new();

        void OnEnable()
        {
            VisualElement root = _uiDocument.rootVisualElement;
            
            _scrollView = root.Q<ScrollView>(_scrollViewName);
            _contentView = root.Q<VisualElement>(_contentViewName);
            
            BuildCreditsContent();
            
            Button closeButton = root.Q<Button>(_closeButtonName);
            closeButton.clicked += CloseCredits;

            root.schedule.Execute(AutoScrollTick).Every(16); 
        }
        private void OnDisable()
        {
            foreach (var (loc, handler) in _bindings)
                loc.StringChanged -= handler;
            _bindings.Clear();
        }
        private void Bind(LocalizedString loc, LocalizedString.ChangeHandler handler)
        {
            loc.StringChanged += handler;
            _bindings.Add((loc, handler));
        }
        
        private void BuildCreditsContent()
        {
            _contentView.Clear();

            foreach (CreditSectiontest section in _creditSections)
            {
                Label titleLabel = new Label();
                titleLabel.AddToClassList("CommonTitle");
                _contentView.Add(titleLabel);
                Bind(section.sectionTitle, v => titleLabel.text = v);
                
                foreach (LocalizedString nameEntry in section.names)
                {
                    Label nameLabel = new Label();
                    nameLabel.AddToClassList("CommonText");
                    _contentView.Add(nameLabel);
                    Bind(nameEntry, v => nameLabel.text = v);
                }
            }

            var spacer = new VisualElement { style = { height = 300 } };
            _contentView.Add(spacer);
        }

        private void AutoScrollTick()
        {
            if (!_shouldAutoScroll) return;

            float newY = _scrollView.scrollOffset.y + (_autoScrollSpeed * 0.016f);

            if (newY >= _scrollView.contentContainer.layout.height - _scrollView.contentViewport.layout.height)
            {
                _shouldAutoScroll = false;
                CloseCredits();
            }

            _scrollView.scrollOffset = new Vector2(0, newY);
        }
        
        private void CloseCredits()
        {
            gameObject.SetActive(false);
        }
    }
}
