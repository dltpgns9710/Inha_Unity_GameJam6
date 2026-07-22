using System;
using System.Collections.Generic;
using UnityEngine;

namespace SEHOON.UI
{
    public enum EGuideTrigger
    {
        Move,
        Run,
        Jump,
        Interact,
        EquipFlashlight,
        ToggleFlashlight,
        ReturnCamera,
        RequestDetectAnomaly,
        RequestWait,
    }

    [Serializable]
    public class KeyGuideEntry
    {
        public EGuideTrigger Trigger;
        public GameObject Target;
    }

    public class KeyGuideView : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private List<KeyGuideEntry> _entries;
        #endregion

        #region Public Methods
        public void Show(EGuideTrigger trigger) => SetEntryActive(trigger, true);

        public void Hide(EGuideTrigger trigger) => SetEntryActive(trigger, false);
        #endregion

        #region Private Methods
        private void SetEntryActive(EGuideTrigger trigger, bool active)
        {
            if (_entries == null) return;

            foreach (KeyGuideEntry entry in _entries)
            {
                if (entry.Trigger == trigger && entry.Target != null)
                {
                    entry.Target.SetActive(active);
                }
            }
        }
        #endregion
    }
}
