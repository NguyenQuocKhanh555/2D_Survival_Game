using UnityEngine;

public class CraftMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject _craftItemTypeMenu;

    public void OnClick()
    {
        _craftItemTypeMenu.SetActive(!_craftItemTypeMenu.activeSelf);
    }
}
