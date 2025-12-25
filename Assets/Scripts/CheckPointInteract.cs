using UnityEngine;

public class CheckPointInteract : Interactable
{
    private Animator _animator;
    private bool _isActive = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Interact(Player player)
    {
        if (_isActive) return;

        player.SetRespawnPoint(transform.position, this);
        _animator.SetBool("isActive", true);
        _isActive = true;
    }

    public void Deactivate()
    {
        _animator.SetBool("isActive", false);
        _isActive = false;
    }
}
