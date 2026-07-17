using UnityEngine;

namespace SEHOON.UI
{
    public enum EDialogueBoxAlignment
    {
        Left,
        Right
    }

    [System.Serializable]
    public class DialogueLine
    {
        [SerializeField] private string _name;
        [SerializeField, TextArea] private string _dialogue;
        [SerializeField] private EDialogueBoxAlignment _alignment = EDialogueBoxAlignment.Left;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Dialogue
        {
            get => _dialogue;
            set => _dialogue = value;
        }

        public EDialogueBoxAlignment Alignment
        {
            get => _alignment;
            set => _alignment = value;
        }
    }
}
