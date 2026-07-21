using System.Collections;
using TMPro;
using UnityEngine;

namespace TAEWOOK.Helper.Feedback
{
    public class BubbleUI : MonoBehaviour
    {
        [SerializeField] private GameObject _bubblePanel;
        [SerializeField] private TextMeshProUGUI _hintText;
        [SerializeField, Min(0f)] private float _displayDuration = 3f;

        private Coroutine _hideCoroutine;

        private void Awake()
        {            
            Hide();
        }

        #region Public Methods
        public void Show(string hintText)
        {            
            if (string.IsNullOrWhiteSpace(hintText))
            {                
                Hide();
                return;
            }

            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
            }

            _hintText.text = hintText;
            _bubblePanel.SetActive(true);          
            _hideCoroutine = StartCoroutine(HideAfterDelay());
        }

        public void Hide()
        {
            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
                _hideCoroutine = null;
            }

            _bubblePanel.SetActive(false);
        }
        #endregion

        #region Private Methods
        private IEnumerator HideAfterDelay()
        {            
            yield return new WaitForSeconds(_displayDuration);

            _bubblePanel.SetActive(false);
            _hideCoroutine = null;            
        }
        #endregion
    }
}
