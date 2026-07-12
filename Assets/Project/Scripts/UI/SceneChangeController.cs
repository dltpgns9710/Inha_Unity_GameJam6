using UnityEngine;
using UnityEngine.SceneManagement;

namespace SEHOON.UI
{
    public class SceneChangeController : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Scene")]
        [SerializeField] private string _sceneToLoad;
        #endregion

        #region Public Methods
        public void ChangeScene()
        {
            SceneManager.LoadScene(_sceneToLoad);
        }
        #endregion
    }
}
