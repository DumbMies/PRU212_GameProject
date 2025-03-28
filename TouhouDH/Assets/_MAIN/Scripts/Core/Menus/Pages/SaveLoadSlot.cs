﻿using History;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VISUALNOVEL;

public class SaveLoadSlot : MonoBehaviour
{
    public GameObject root;
    public RawImage previewImage;
    public TextMeshProUGUI titleText;
    public Button deleteButton;
    public Button saveButton;
    public Button loadButton;

    [HideInInspector] public int fileNumber = 0;
    [HideInInspector] public string filePath = "";

    private UIConfirmationMenu uiChoiceMenu => UIConfirmationMenu.instance;

    public void PopulateDetails(SaveAndLoadMenu.MenuFuction function)
    {
        if (File.Exists(filePath))
        {
            VNGameSave file = VNGameSave.Load(filePath);
            PopulateDetailsFromFile(function, file);
        }
        else
        {
            PopulateDetailsFromFile(function, null);
        }
    }

    private void PopulateDetailsFromFile(SaveAndLoadMenu.MenuFuction function, VNGameSave file)
    {
        if (file == null)
        {
            titleText.text = $"{fileNumber}. Slot trống";
            deleteButton.gameObject.SetActive(false);
            loadButton.gameObject.SetActive(false);
            saveButton.gameObject.SetActive(function == SaveAndLoadMenu.MenuFuction.save);
            previewImage.texture = SaveAndLoadMenu.Instance.emptyFileImage;
        }
        else
        {
            titleText.text = $"{fileNumber}. {file.timestamp}";
            deleteButton.gameObject.SetActive(true);
            loadButton.gameObject.SetActive(function == SaveAndLoadMenu.MenuFuction.load);
            saveButton.gameObject.SetActive(function == SaveAndLoadMenu.MenuFuction.save);

            byte[] data = File.ReadAllBytes(file.screenshotPath);
            Texture2D screenshotPreview = new Texture2D(1, 1);
            ImageConversion.LoadImage(screenshotPreview, data);
            previewImage.texture = screenshotPreview;
        }
    }
    
    public void Delete()
    {
        uiChoiceMenu.Show(
            //Title
            "Bạn muốn xóa file này?",
            //Choice 1
            new UIConfirmationMenu.ConfirmationButton("Có", () =>
                {
                    uiChoiceMenu.Show(
                        "<u>File bị xóa sẽ không thể khôi phục lại.</u> Bạn chắc chắn muốn xóa?",
                        new UIConfirmationMenu.ConfirmationButton("Có", OnConfirmDelete),
                        new UIConfirmationMenu.ConfirmationButton("Không", null));
                },
                autoCloseOnClick: false
            ),
            //Choice 2
            new UIConfirmationMenu.ConfirmationButton("Không", null));
    }

    private void OnConfirmDelete()
    {
        File.Delete(filePath);
        PopulateDetails(SaveAndLoadMenu.Instance.menuFuction);
    }

    public void Load()
    {
        
        VNGameSave file = VNGameSave.Load(filePath, false);
        SaveAndLoadMenu.Instance.Close(cloaseAllMenus: true);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == MainMenu.MAIN_MENU_SCENE)
        {
            MainMenu.instance.LoadGame(file);
        }
        else
        {
            file.Activate();
        }
    }

    public void Save()
    {
        if (HistoryManager.instance.isViewingHistory)
        {
            UIConfirmationMenu.instance.Show("You cannot save while viewing history.", new UIConfirmationMenu.ConfirmationButton("Back", null));
            return;
        }

        var activeSave = VNGameSave.activeFile;
        activeSave.slotNumber = fileNumber;

        activeSave.Save();

        PopulateDetailsFromFile(SaveAndLoadMenu.Instance.menuFuction, activeSave);
    }
}
