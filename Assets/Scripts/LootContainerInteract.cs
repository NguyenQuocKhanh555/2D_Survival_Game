using UnityEngine;

public class LootContainerInteract : Interactable
{
    [SerializeField] private Sprite _openedForm;
    [SerializeField] private Sprite _closedForm;
    [SerializeField] private bool _isOpened = false;
    [SerializeField] private string _lootContainerName;

    private GameObject _containerUI;
    private LootContainerUI _lootContainerUI;
    [SerializeField] private SO_ItemContainer _itemContainer;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_itemContainer == null)
        {
            Init();
        }
    }

    private void Init()
    {
        _itemContainer = (SO_ItemContainer)ScriptableObject.CreateInstance(typeof(SO_ItemContainer));
        _itemContainer.Init(30);
    }

    public override void Interact(Player player)
    {
        if (!_isOpened)
        {
            Open(player);
        }
        else
        {
            Close();
        }
    }

    private void Open(Player player)
    {
        _isOpened = true;
        _spriteRenderer.sprite = _openedForm;

        if (_lootContainerName != "" && _containerUI == null)
        {
            player.GetComponent<PlayerUIInteractController>().itemConvertingUI.TryGetValue(
                _lootContainerName, out _containerUI);
        }

        if (_lootContainerUI == null)
        {
            _lootContainerUI = _containerUI.GetComponent<LootContainerUI>();            
        }

        _lootContainerUI.SetLootContainer(_itemContainer);
        _containerUI.SetActive(!_containerUI.activeSelf);
    }

    private void Close()
    {
        _isOpened = false;
        _spriteRenderer.sprite = _closedForm;

        _containerUI.SetActive(!_containerUI.activeSelf);
    }
}
