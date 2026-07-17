using System;
using UnityEngine;
using USingleton;

namespace SEHOON.GameSystem
{
    public enum EStoryType
    {
        Start,
        Ending
    }

    public class DataManager : Singleton<DataManager>
    {
        [SerializeField] private int _goalFloor = 8;
        [SerializeField] private Texture2D _normalCursor;

        private int _floor = 1;
        private bool _isAnomalyApply = false;
        private EStoryType _storyType = EStoryType.Start;

        public Action<bool> ActiveGameUI;
        
        public int Floor => _floor;
        public int GoalFloor => _goalFloor;

        private void Start()
        {
            if (_normalCursor != null)
            {
                Cursor.SetCursor(_normalCursor, Vector2.zero, CursorMode.Auto);
            }
        }

        public bool IsAnomalyApply
        {
            get => _isAnomalyApply;
            set => _isAnomalyApply = value;
        }
        
        public EStoryType StoryType
        {
            get => _storyType;
            set => _storyType = value;
        }

        public void Init()
        {
            _storyType = EStoryType.Start;
            _floor = 1;
            _isAnomalyApply = false;
        }
        
        public void SelectCorrectDoor()
        {
            ++_floor;
        }
        
        public void SelectIncorrectDoor()
        {
            _floor = 1;
        }
    }
}
