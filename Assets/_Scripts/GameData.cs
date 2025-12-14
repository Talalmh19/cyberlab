using UnityEngine;

[CreateAssetMenu(fileName = "Game Data", menuName = "Scriptable Object/Game Data", order = 1)]
public class GameData : ScriptableObject
{
    public float music;
    public float sound;

    public int level;
    public int points;

    public bool[] haveLevel = new bool[6];
    public bool[] playedLevel = new bool[6];
}
