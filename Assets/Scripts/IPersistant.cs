using UnityEngine;

public interface IPersistant
{
    public string Read();
    public void Load(string jsonString);
}
