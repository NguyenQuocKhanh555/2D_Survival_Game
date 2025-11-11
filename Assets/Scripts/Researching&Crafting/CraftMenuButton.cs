using UnityEngine;

public class CraftMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject _craftItemTypeMenu;
    [SerializeField] private GameObject _craftRecipeMenu;
    [SerializeField] private GameObject _craftInfosUI;

    public void OnClick()
    {
        _craftItemTypeMenu.SetActive(!_craftItemTypeMenu.activeSelf);
        _craftRecipeMenu.SetActive(false);
        _craftInfosUI.SetActive(false);
    }
}
