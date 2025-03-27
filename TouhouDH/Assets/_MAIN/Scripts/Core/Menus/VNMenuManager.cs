using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VNMenuManager : MonoBehaviour
{
    public static VNMenuManager instance;

    private MenuPage activePage = null;
    public bool isOpen = false;

    [SerializeField] private CanvasGroup root;
    [SerializeField] private MenuPage[] pages;
    [SerializeField] private CanvasGroup panelRoot;

    private CanvasGroupController rootCG;

    private UIConfirmationMenu uiChoiceMenu => UIConfirmationMenu.instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rootCG = new CanvasGroupController(this, root);
    }

    private MenuPage GetPage(MenuPage.PageType pageType)
    {
        return pages.FirstOrDefault(pages => pages.pageType == pageType);
    }

    public void OpenSavePage()
    {
        var page = GetPage(MenuPage.PageType.SaveAndLoad);
        var slm = page.anim.GetComponentInParent<SaveAndLoadMenu>();
        slm.menuFuction = SaveAndLoadMenu.MenuFuction.save;
        OpenPage(page);
    }

    public void OpenLoadPage()
    {
        var page = GetPage(MenuPage.PageType.SaveAndLoad);
        var slm = page.anim.GetComponentInParent<SaveAndLoadMenu>();
        slm.menuFuction = SaveAndLoadMenu.MenuFuction.load;
        OpenPage(page);
    }

    public void OpenConfigPage()
    {
        var page = GetPage(MenuPage.PageType.Config);
        OpenPage(page);
    }

    public void OpenHelpPage()
    {
        var page = GetPage(MenuPage.PageType.Help);
        OpenPage(page);
    }

    private void OpenPage(MenuPage page)
    {
        if (page == null)
        {
            return;
        }
        if (activePage != null && activePage != page)
        {
            activePage.Close();
        }
        page.Open();
        activePage = page;

        if (!isOpen)
        {
            OpenRoot();
        }
    }

    public void OpenRoot()
    {
        rootCG.Show();
        rootCG.SetInteractableState(true);
        isOpen = true;
    }

    public void CloseRoot()
    {
        rootCG.Hide();
        rootCG.SetInteractableState(false);
        isOpen = false;
    }

    public void DisablePanelRoot()
    {
        panelRoot.alpha = 0;
        panelRoot.interactable = false;
    }

    public void EnablePanelRoot()
    {
        panelRoot.alpha = 1;
        panelRoot.interactable = true;
    }


    public void Click_Home()
    {
        uiChoiceMenu.Show("Tiến độ chưa được save sẽ bị mất khi quay về Home. Bạn chắc chắn muốn quay về?", new UIConfirmationMenu.ConfirmationButton("Có", () => GoHome()), new UIConfirmationMenu.ConfirmationButton("Không", null));
    }

    public void GoHome()
    {
        VN_Configuration.activeConfig.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene(MainMenu.MAIN_MENU_SCENE);
    }    

    public void Click_Quit()
    {
        uiChoiceMenu.Show("Tiến độ chưa được save sẽ bị mất khi thoát game. Bạn chắc chắn muốn thoát?", new UIConfirmationMenu.ConfirmationButton("Có", () => Application.Quit()), new UIConfirmationMenu.ConfirmationButton("Không", null));
    }
}
