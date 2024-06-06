using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameObject persistent;
    public static GameController Instance;

    public static bool NewGame = true;
    public static int LatestMap = 1;
    Vector2 latestPlayerPos;
    public static int QuestItemCount = 0;
    public static bool CanFinishStory = false;

    public string playerName;
    public bool enablePlayerInput;

    public Transform player;

    [SerializeField] GameObject[] persistentUI;
    [SerializeField] GameObject playeyDiedUI;

    [HideInInspector] public bool canPause = true;
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
        InitializePlayer();

        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayCurrentSceneBGM();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {


        PlayCurrentSceneBGM();
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found in the scene!");
            return;
        }

        if (PlayerPrefs.GetInt("HaveSaved") == 99)
        {
            player.position = latestPlayerPos;
        }
    }

    private void PlayCurrentSceneBGM()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        switch (currentSceneName)
        {
            case "Village":
                SoundManager.Instance.PlayBGM("VillageBGM");
                //  SoundManager.Instance.Stop("BGMDanau");

                break;
            case "Forest":
                SoundManager.Instance.PlayBGM("ForestBGM");
                break;
            default:
                SoundManager.Instance.PlayBGM("DefaultBGM");
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                ResumeGame();
                ShowPersistentUI(true);
                pauseUI.SetActive(false);
            }
        }

        // Update enablePlayerInput logic
        if (Player_Action.Instance != null)
        {
            enablePlayerInput = Player_Action.Instance.canAttack && !gamePaused;
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

    public void PauseWithUI()
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
        NewGame = false;
    }

    [ContextMenu("Load Game")]
    public void LoadGame(GameData theData = null)
    {
        Debug.Log("Loading Game");
        GameData data = theData ?? SaveSystem.LoadData();
        Player_Inventory inventory = Player_Inventory.Instance;

        playerName = data.playerName;

        LatestMap = data.LatestMap;
        latestPlayerPos = new(data.playerPos[0], data.playerPos[1]);

        inventory.itemList = new();
        foreach (GameData.SimpleItem item in data.PlayerInventory_ItemNameAndCount)
        {
            inventory.AddItem(ItemPool.Instance.GetItem(item.itemName, item.stackCount, item.level));
        }

        inventory.EquipItem(inventory.FindItemInInventory(data.PlayerInventory_ActiveItemAndCount[0].itemName), 0);
        inventory.EquipItem(inventory.FindItemInInventory(data.PlayerInventory_ActiveItemAndCount[1].itemName), 1);
        inventory.AddQuickSlot(inventory.FindItemInInventory(data.PlayerInventory_ActiveItemAndCount[2].itemName), 0);
        inventory.AddQuickSlot(inventory.FindItemInInventory(data.PlayerInventory_ActiveItemAndCount[3].itemName), 1);

        // Load storage items to each storage container
        if (StorageSystem.Instance != null)
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
        GameEventSystem.Instance.DoneDialogue_TamashiiGiveName = data.gameEvent_DoneDialogue_1;
        GameEventSystem.Instance.DoneDialogue_DanauPertamaKeDesa = data.gameEvent_DoneDialogue_2;
        GameEventSystem.Instance.DoneDialogue_FirstDesaWarga = data.gameEvent_DoneDialogue_3;
        GameEventSystem.Instance.DoneDialogue_FirstKakRen = data.gameEvent_DoneDialogue_4;
        GameEventSystem.Instance.DoneDialogue_FirstBandit = data.gameEvent_DoneDialogue_5;
        GameEventSystem.Instance.DoneDialogue_FirstBanditDone = data.gameEvent_DoneDialogue_6;
        GameEventSystem.Instance.DoneDialogue_FinshDialogue = data.gameEvent_DoneDialogue_7;
    }

    public void GoToMainMenu()
    {
        SaveGame();
        LoadingScreenUI.Instance.LoadScene(0);
        Destroy(transform.root.gameObject);
    }

    public void PlayerDied()
    {
        PauseGame();
        playeyDiedUI.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
