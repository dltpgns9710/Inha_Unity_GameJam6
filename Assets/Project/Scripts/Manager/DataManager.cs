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

        private int _floor = 1;
        private bool _isAnomalyApply = false;
        private EStoryType _storyType = EStoryType.Start;

        public int Floor => _floor;

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
            Debug.Log($"Correct door selected. Current floor: {_floor}");
        }
        
        public void SelectIncorrectDoor()
        {
            _floor = 0;
            Debug.Log($"Incorrect door selected. Current floor: {_floor}");
        }
    }
}
