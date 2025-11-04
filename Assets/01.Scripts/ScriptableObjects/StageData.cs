using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "GameData/StageData")]
public class StageData : ScriptableObject
{
    public enum StageType { Combat, Rest, Boss }
    public StageType stageType;

    [Header("Enemy Spawn Settings (Combat/Boss Only)")]
    public List<GameObject> monsterPrefabs;

    public int minMonsterCount = 1;
    public int maxMonsterCount = 3;
}