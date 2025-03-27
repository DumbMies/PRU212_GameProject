using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : MonoBehaviour
{
    public enum PageType { SaveAndLoad, Config, Help }
    public PageType pageType;

    public bool allowToUse = true;

    private const string OPEN = "Open";
    private const string CLOSE = "Close";
    [SerializeField] public Animator anim;

    public virtual void Open()
    {
        if (allowToUse)
        {
            anim.SetTrigger(OPEN);
        }
        else
        {
            return;
        }
    }

    public virtual void Close(bool cloaseAllMenus = false)
    {
        anim.SetTrigger(CLOSE);

        if (cloaseAllMenus)
        {
            VNMenuManager.instance.CloseRoot();
        }
    }
}
