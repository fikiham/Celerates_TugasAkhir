using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public string playerName;

    [SerializeField] GameObject[] persistentUI;

    bool canPause = true;
    public bool gamePaused;
    [SerializeField] GameObject pauseUI;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DialogueSystem.Instance.StartFirstDialogue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canPause)
            {
                PauseWithUI();
            }
            else if (gamePaused)
            {
                ResumeGame();
                ShowPersistentUI(true);
                pauseUI.SetActive(false);
            }
        }

    }

    public void ShowPersistentUI(bool show)
    {
        canPause = show;
        foreach (GameObject ui in persistentUI)
        {
            ui.SetActive(show);
        }
    }

    void PauseWithUI()
    {
        PauseGame();
        ShowPersistentUI(false);
        pauseUI.SetActive(true);

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
        Player_Inventory inventory = Player_Inventory.Instance;

        playerName = data.playerName;


        // Load player inventory data
        inventory.itemList = new();
        foreach (GameData.SimpleItem item in data.PlayerInventory_ItemNameAndCount)
        {
            inventory.AddItem(ItemPool.Instance.GetItem(item.itemName, item.stackCount, item.level));
        }

        // Load Player Active Items
        inventory.EquipItem(inventory.FindItemInInventory(data.PlayerInventory_ActiveItemAndCount[0].itemName), 0);
        inventory.EquipItem(inventory.FindItemInInventory(data.PlayerInventory_ActiveItemAndCount[1].itemName), 1);
        inventory.AddQuickSlot(inventory.FindItemInInventory(data.PlayerInventory_ActiveItemAndCount[2].itemName), 0);
        inventory.AddQuickSlot(inventory.FindItemInInventory(data.PlayerInventory_ActiveItemAndCount[3].itemName), 1);

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

    public void QuitGame()
    {
        Application.Quit();
    }
}
