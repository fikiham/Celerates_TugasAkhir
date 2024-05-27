using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestController : MonoBehaviour
{
    public static ForestController Instance;


    Transform player;
    [SerializeField] Transform playerSpawnSpot;

    public GameObject FirstSpawner;
    bool firstSpawnerDefeated;
    [SerializeField] Dialogues danauKetenanganDialogue;
    public Transform DanauKetenangan;
    public Transform VillagePortal;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!GameEventSystem.Instance.DoneFirstNarration)
        {
            DialogueSystem.Instance.StartFirstDialogue();
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            player.position = playerSpawnSpot.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!firstSpawnerDefeated)
        {
            FirstSpawner.GetComponent<Enemy_Spawner>().CanSpawn = false;
            if (FirstSpawner.GetComponent<Enemy_Spawner>().enemies.Count == 0)
            {
                firstSpawnerDefeated = true;
                DialogueSystem.Instance.StartDialogue(danauKetenanganDialogue);
            }
        }
    }
}
