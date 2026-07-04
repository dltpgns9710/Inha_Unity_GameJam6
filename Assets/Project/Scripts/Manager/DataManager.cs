using UnityEngine;
using USingleton;

namespace SEHOON.GameSystem
{
    public class DataManager : Singleton<DataManager>
    {

        [SerializeField] private int _goalFloor = 8;
        
        private int _floor = 1;
        
        public int Floor => _floor;

        public void SelectCorrectDoor()
        {
            ++_floor;
        }
        
        public void SelectIncorrectDoor()
        {
            _floor = 0;
        }
    }
}
