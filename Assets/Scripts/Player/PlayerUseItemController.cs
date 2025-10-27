using UnityEngine;

public class PlayerUseItemController : MonoBehaviour
{
    [SerializeField] private PlayerEquipmentController _equipmentController;

    public void UseTool(Animator animator)
    {
        if (_equipmentController.currentToolData == null) return;
        animator.SetTrigger("action");
    }

    public void UseWeapon(Animator animator)
    {
        if (_equipmentController.currentToolData == null) return;
        animator.SetTrigger("attack");
    }

    public void UseFishingRod(Animator animator)
    {
        if (_equipmentController.currentToolData == null) return;
        animator.SetTrigger("fish");
    }

    public void UseConsumableItem(Animator animator)
    {
        
    }
}
