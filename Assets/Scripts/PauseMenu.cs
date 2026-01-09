using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void OnYes()
    {
        Application.Quit();
    }

    public void OnNo()
    {
        gameObject.SetActive(false);
    }
}
