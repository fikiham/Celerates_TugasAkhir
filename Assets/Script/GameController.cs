using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] GameObject[] persistentUI;

    bool canPause = true;
    public bool gamePaused;
    [SerializeField] GameObject pauseUI;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (canPause && Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
            ShowPersistentUI(false);
        }
        pauseUI.SetActive(gamePaused);

    }

    public void ShowPersistentUI(bool show)
    {
        canPause = show;
        foreach (GameObject ui in persistentUI)
        {
            ui.SetActive(show);
        }
    }

    public void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
        ShowPersistentUI(true);
    }

    public void SaveGame()
    {
        SaveSystem.SaveData();
    }

    public void LoadGame()
    {
        GameData data = SaveSystem.LoadData();

        // Load player inventory data
        Player_Inventory.Instance.itemList = new();
        foreach (GameData.SimpleItem item in data.PlayerInventory_ItemNameAndCount)
        {
            Player_Inventory.Instance.AddItem(ItemPool.Instance.GetItem(item.itemName, item.stackCount, item.level));
        }

        // Load storage items to each storage container
        foreach (KeyValuePair<int, List<GameData.SimpleItem>> ele in data.Storages_ItemNameAndCount)
        {
            StorageSystem.Instance.storages[ele.Key].Items = new();
            List<Item> storage = StorageSystem.Instance.storages[ele.Key].Items;
            foreach (GameData.SimpleItem item in ele.Value)
            {
                storage.Add(ItemPool.Instance.GetItem(item.itemName, item.stackCount, item.level));
            }
        }
    }


}
