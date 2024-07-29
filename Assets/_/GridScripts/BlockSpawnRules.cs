using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockSpawnRules", menuName = "ScriptableObjects/BlockSpawnRules", order = 1)]
public class BlockSpawnRules : ScriptableObject
{
    [Serializable]
    public class MoveRangeBlockNumbers
    {
        public int minMoves;
        public int maxMoves;
        public List<int> possibleBlockNumbers;
    }

    public List<MoveRangeBlockNumbers> blockNumberRules;

    [Serializable]
    public class BlockColorMapping
    {
        public int number;
        public Color color;
    }

    public List<BlockColorMapping> blockColorMappings;
}