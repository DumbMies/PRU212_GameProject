using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VISUALNOVEL;

public class SaveAndLoadMenu : MenuPage
{
    public static SaveAndLoadMenu Instance { get; private set; }

    public const int MAX_FILES = 60;
    //private string savePath;

    private int currentPage = 1;
    private bool loadedFilesForFirstTime = false;

    public enum MenuFuction { save, load }
    public MenuFuction menuFuction = MenuFuction.save;

    public SaveLoadSlot[] saveSlots;
    public int slotsPerPage => saveSlots.Length;

    public Texture emptyFileImage;

    private void Awake()
    {
        Instance = this;
        //savePath = FilePaths.gameSaves;
    }

    public override void Open()
    {
        base.Open();

        if (!loadedFilesForFirstTime)
        {
            PopulateSaveSlotsForPage(currentPage);
        }
    }

    public void PopulateSaveSlotsForPage(int pageNumber)
    {
        currentPage = pageNumber;
        int startingFile = ((currentPage - 1) * slotsPerPage) + 1;
        int endingFile = startingFile + slotsPerPage - 1;

        for (int i = 0; i < slotsPerPage; i++)
        {
            int fileNum = startingFile + i;
            SaveLoadSlot slot = saveSlots[i];

            if (fileNum <= MAX_FILES)
            {
                slot.root.SetActive(true);
                string filePath = $"{FilePaths.gameSaves}{fileNum}{VNGameSave.FILE_TYPE}";
                slot.fileNumber = fileNum;
                slot.filePath = filePath;
                slot.PopulateDetails(menuFuction);
            }
            else
            {
                slot.root.SetActive(false);
            }

        }
    }
}
