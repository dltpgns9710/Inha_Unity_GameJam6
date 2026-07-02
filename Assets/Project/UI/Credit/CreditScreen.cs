using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SEHOON.UI
{
    [System.Serializable]
    public class CreditSection
    {
        public string sectionTitle;
        public List<string> names;
    }
    
    public class CreditScreen : MonoBehaviour
    {
        
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private float _autoScrollSpeed = 20f;
        [SerializeField] private string _scrollViewName;
        [SerializeField] private string _contentViewName;
        [SerializeField] private string _closeButtonName;
        [SerializeField] private List<CreditSection> _creditSections;
        
        private ScrollView _scrollView;
        private VisualElement _contentView;
        private bool _shouldAutoScroll = true;

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
        
        private void BuildCreditsContent()
        {
            _contentView.Clear();

            foreach (CreditSection section in _creditSections)
            {
                Label titleLabel = new Label(section.sectionTitle);
                titleLabel.AddToClassList("CommonTitle");
                _contentView.Add(titleLabel);

                foreach (string name in section.names)
                {
                    Label nameLabel = new Label(name);
                    nameLabel.AddToClassList("CommonText");
                    _contentView.Add(nameLabel);
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
                //CloseCredits();
            }

            _scrollView.scrollOffset = new Vector2(0, newY);
        }
        
        private void CloseCredits()
        {
            gameObject.SetActive(false);
        }
    }
}
