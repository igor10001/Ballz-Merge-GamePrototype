using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockCountSpawnChances", menuName = "ScriptableObjects/BlockCountSpawnChances", order = 2)]
public class BlockCountSpawnChances : ScriptableObject
{
    [Serializable]
    public class MoveRangeBlockCounts
    {
        public int minMoves;
        public int maxMoves;

        [Serializable]
        public class BlockCountChance
        {
            public int blockCount;
            public float probability;
        }

        public List<BlockCountChance> blockCountChances;
    }

    public List<MoveRangeBlockCounts> blockCountRules;
}