﻿using System;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnPlan")]
    public class SpawnPlan : ScriptableObject
    {
        public SpawnPlanItem[] Items;
    }

    [Serializable]
    public struct SpawnPlanItem
    {
        public float TimestampDelta;
        public int Count;
        public GameObject Prefab;
    }
}