using System.Collections;
using TMPro;
using UnityEngine;

namespace TAEWOOK.Helper.Feedback
{
    public class BubbleUI : MonoBehaviour
    {
        [SerializeField] private GameObject _bubblePanel;
        [SerializeField] private TextMeshProUGUI _hintText;
        [SerializeField] private AnimationClip _bubbleAnimation;
        [SerializeField, Min(0f)] private float _displayDuration = 3f;

        private Coroutine _playCoroutine;

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

            if (_playCoroutine != null)
            {
                StopCoroutine(_playCoroutine);
            }

            _hintText.text = hintText;
            _bubblePanel.SetActive(true);
            _playCoroutine = StartCoroutine(PlayBubbleAnimation());
        }

        public void Hide()
        {
            if (_playCoroutine != null)
            {
                StopCoroutine(_playCoroutine);
                _playCoroutine = null;
            }

            _bubblePanel.SetActive(false);
        }
        #endregion

        #region Private Methods
        private IEnumerator PlayBubbleAnimation()
        {
            if (_bubbleAnimation == null)
            {
                yield return new WaitForSeconds(_displayDuration);
            }
            else
            {
                GameObject animationRoot = transform.parent != null
                    ? transform.parent.gameObject
                    : gameObject;
                float elapsedTime = 0f;

                while (elapsedTime < _bubbleAnimation.length)
                {
                    _bubbleAnimation.SampleAnimation(animationRoot, elapsedTime);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                _bubbleAnimation.SampleAnimation(animationRoot, _bubbleAnimation.length);
            }

            _bubblePanel.SetActive(false);
            _playCoroutine = null;
        }
        #endregion
    }
}
