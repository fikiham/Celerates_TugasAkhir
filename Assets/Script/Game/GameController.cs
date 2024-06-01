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
        if (NewGame)
        {
            GameData newGameData = new(true);
            newGameData.ResetGameData();
            LoadGame(newGameData);
        }
        else
        {
            LoadGame();
        }
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

    [ContextMenu("Save Game")]
    public void SaveGame()
    {
        Debug.Log("Saving Game");
        PlayerPrefs.SetInt("HaveSaved", 99);
        LatestMap = SceneManager.GetActiveScene().buildIndex;
        SaveSystem.SaveData();
    }

    [ContextMenu("Load Game")]
    public void LoadGame(GameData theData = null)
    {
        Debug.Log("Loading Game");
        GameData data;
        if (theData == null)
            data = SaveSystem.LoadData();
        else
            data = theData;
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
        if (StorageSystem.Instance.isActiveAndEnabled)
        {
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

        GameEventSystem.Instance.DoneFirstNarration = data.gameEvent_DoneFirstNarration;
        GameEventSystem.Instance.DoneDialogue_1 = data.gameEvent_DoneDialogue_1;
        GameEventSystem.Instance.DoneDialogue_2 = data.gameEvent_DoneDialogue_2;
        GameEventSystem.Instance.DoneDialogue_3 = data.gameEvent_DoneDialogue_3;
        GameEventSystem.Instance.DoneDialogue_4 = data.gameEvent_DoneDialogue_4;
        GameEventSystem.Instance.DoneDialogue_5 = data.gameEvent_DoneDialogue_5;
        GameEventSystem.Instance.DoneDialogue_6 = data.gameEvent_DoneDialogue_6;
        GameEventSystem.Instance.DoneDialogue_7 = data.gameEvent_DoneDialogue_7;
    }

    public void GoToMainMenu()
    {
        SaveGame();
        NewGame = false;
        LoadingScreenUI.Instance.LoadScene(0);
        Destroy(transform.root.gameObject);
        //SceneManager.LoadScene(0);
    }

    public void PlayerDied()
    {
        PauseGame();
    }


}
