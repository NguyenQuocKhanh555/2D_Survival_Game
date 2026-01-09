using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(SO_CraftingRecipeContainer))]
public class CraftingRecipeContainerEditorConfig : MonoBehaviour
{
    /*override public void OnInspectorGUI()
    {
        SO_CraftingRecipeContainer container = target as SO_CraftingRecipeContainer;
        if (GUILayout.Button("Set default"))
        {
            for (int i = 0; i < container.craftingRecipes.Count; i++)
            {
                container.craftingRecipes[i].isLearn = false;
            }
        }
        if (GUILayout.Button("Clear"))
        {
            for (int i = 0; i < container.craftingRecipes.Count; i++)
            {
                container.craftingRecipes[i].Clear();
            }
        }
        DrawDefaultInspector();
    }*/
}
