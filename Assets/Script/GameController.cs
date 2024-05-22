using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameObject persistent;
    public static GameController Instance;

    public static bool NewGame = true;
    public static int LatestMap = 1;
    Vector2 latestPlayerPos;

    public string playerName;
    public bool enablePlayerInput;

    public Transform player;

    [SerializeField] GameObject[] persistentUI;

    bool canPause = true;
    public bool gamePaused;
    [SerializeField] GameObject pauseUI;

    private void Awake()
    {
        if (persistent != null)
        {
            Destroy(transform.root.gameObject);
            return;
        }
        persistent = transform.root.gameObject;
        DontDestroyOnLoad(persistent);
        Instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (PlayerPrefs.GetInt("HaveSaved") == 99)
        {
            player.position = latestPlayerPos;
        }
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

        if (!Player_Action.Instance.canAttack || gamePaused)
        {
            enablePlayerInput = false;
        }
        else
        {
            enablePlayerInput = true;
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
        Debug.Log("Saving Game");
        PlayerPrefs.SetInt("HaveSaved", 99);
        SaveSystem.SaveData();
    }

    public void LoadGame()
    {
        Debug.Log("Loading Game");
        GameData data = SaveSystem.LoadData();
        Player_Inventory inventory = Player_Inventory.Instance;

        playerName = data.playerName;

        LatestMap = data.LatestMap;
        latestPlayerPos = new(data.playerPos[0], data.playerPos[1]);

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

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
        SaveGame();
    }

    public void GoToScene(int i)
    {
        SceneManager.LoadScene(i);
        SaveGame();
    }


}
