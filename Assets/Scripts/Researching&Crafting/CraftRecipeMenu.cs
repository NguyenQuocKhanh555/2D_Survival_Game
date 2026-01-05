using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftRecipeMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _menuName;
    [SerializeField] private List<CraftRecipeButton> _recipeButtons;
    [SerializeField] private GameObject _craftRecipeButtonPrefab;
    [SerializeField] private GameObject _craftRecipeMenuUI;
    [SerializeField] private RectTransform _recipeMenuContent;

    public SO_CraftingRecipeContainer _playerLearnedRecipes;
    public CraftingPanel craftingPanel;
 
    public void ShowRecipe(ItemTypes itemTypes)
    {
        _craftRecipeMenuUI.gameObject.SetActive(true);
        _menuName.text = itemTypes.ToString();

        List<SO_CraftingRecipe> recipes = _playerLearnedRecipes.GetAllRecipesWithItemType(itemTypes);

        CreateNewCraftRecipeButton(recipes);

        for (int i = 0; i < _recipeButtons.Count; i++)
        {
            _recipeButtons[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < recipes.Count; i++)
        {
            _recipeButtons[i].Show(recipes[i]);
        }

        _recipeMenuContent.sizeDelta = new Vector2(_recipeMenuContent.sizeDelta.x, 90 * (int)(recipes.Count / 4));
    }

    private void CreateNewCraftRecipeButton(List<SO_CraftingRecipe> recipes)
    {
        if (recipes.Count <= _recipeButtons.Count) { return; }

        int buttonsToCreate = recipes.Count - _recipeButtons.Count;

        for (int i = 0; i < buttonsToCreate; i++) 
        {
            GameObject obj = Instantiate(_craftRecipeButtonPrefab, this.transform);

            CraftRecipeButton craftRecipeButton = obj.GetComponent<CraftRecipeButton>();
            _recipeButtons.Add(craftRecipeButton);
        }
    }
}
