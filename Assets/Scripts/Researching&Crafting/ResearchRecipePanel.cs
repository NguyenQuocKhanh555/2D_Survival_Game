using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResearchRecipePanel : MonoBehaviour
{
    [SerializeField] private ResearchItemButton _researchItemButton;
    [SerializeField] RectTransform _recipeScrollViewContent;
    [SerializeField] private List<RecipePanel> _recipePanels;
    [SerializeField] private GameObject _recipePanelPrefab;

    public SO_CraftingRecipeContainer recipeContainer;

    private void OnEnable()
    {
        OnClickResearchButton();
    }

    public void OnClickResearchButton()
    {
        if (_researchItemButton.researchedItem.slots.Count <= 0) { return; }

        List<CraftingRecipeSlot> recipes = new List<CraftingRecipeSlot>();

        for (int i = 0; i < _researchItemButton.researchedItem.slots.Count; i++)
        {
            SO_Item materialItem = _researchItemButton.researchedItem.slots[i].item;

            recipes = recipes.Union(recipeContainer.FindRecipeWithResearchMaterial(materialItem)).ToList();
        }

        Show(recipes, _researchItemButton.researchedItem);
    }

    private void CreateNewRecipePanels(List<CraftingRecipeSlot> recipes)
    {
        if (recipes.Count <= _recipePanels.Count) return;
        int loop = recipes.Count - _recipePanels.Count;

        for (int i = 0; i < loop; i++)
        {
            GameObject obj = Instantiate(_recipePanelPrefab, this.transform);

            RecipePanel newRecipePanel = obj.GetComponent<RecipePanel>();
            _recipePanels.Add(newRecipePanel);
        }
    }

    private void Show(List<CraftingRecipeSlot> recipes, SO_ItemContainer researchedItems)
    {
        if (_recipePanels.Count <= 0) return;

        CreateNewRecipePanels(recipes);

        for (int i = 0; i < _recipePanels.Count; i++)
        {
            _recipePanels[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < recipes.Count; i++)
        {
            _recipePanels[i].Show(recipes[i], researchedItems);
        }

        _recipeScrollViewContent.sizeDelta = new Vector2(_recipeScrollViewContent.sizeDelta.x, 120 * recipes.Count);
    }
}
