using UnityEngine;

[CreateAssetMenu(fileName = "New player stats", menuName = "Player Stats")]
public class SO_PlayerStats : ScriptableObject
{
    public Stat health;
    public Stat stamina;
    public Stat hunger;
    public Stat thirst;
    public float bodyTemperature;
    public int armor;
}
