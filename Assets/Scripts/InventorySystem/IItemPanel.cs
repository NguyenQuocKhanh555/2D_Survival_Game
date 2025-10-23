using System.Collections.Generic;
using UnityEngine;

public interface IItemPanel
{
    public void Init();
    public void SetSourcePanel();
    public void SetIndex();
    public void Show();
    public void Clear();
    public void OnClick(int id);
}
