using UnityEngine;


    [CreateAssetMenu(fileName = "GridObject", menuName = "GridScriptanbleObject", order = 0)]
    public class GridObject : ScriptableObject
    {
        public Transform prefab;
        public int health;
        public Vector2 gridPosition;
    }
