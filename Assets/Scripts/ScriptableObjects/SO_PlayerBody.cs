using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Body", menuName = "Player Body")]
public class SO_PlayerBody : ScriptableObject
{
    public PlayerBodyPart[] playerBodyParts;
}

[Serializable]
public class PlayerBodyPart
{
    public string bodyPartName;
    public SO_PlayerBodyPart playerPart;
}
