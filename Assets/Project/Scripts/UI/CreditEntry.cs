using System.Collections.Generic;
using UnityEngine;

namespace SEHOON.UI
{
    [System.Serializable]
    public class CreditEntry
    {
        [SerializeField] private string _part;
        [SerializeField] private List<string> _names = new List<string>();

        public string Part
        {
            get => _part;
            set => _part = value;
        }

        public List<string> Names
        {
            get => _names;
            set => _names = value;
        }
    }
}
