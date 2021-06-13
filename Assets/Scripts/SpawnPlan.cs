using System;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "ScriptableObjects/SpawnPlan")]
    public class SpawnPlan : ScriptableObject
    {
        public SpawnPlanItem[] Items;
    }

    [Serializable]
    public struct SpawnPlanItem
    {
        public float TimestampDelta;
        public int Count;
        public GameObject[] Prefabs;
    }
}