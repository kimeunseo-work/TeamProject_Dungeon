using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Game Data/Player Data")]
public class PlayerData : ScriptableObject
{
    public string Name;
    public int Hp;
    public int Atk;
}
