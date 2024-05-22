using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestController : MonoBehaviour
{
    public static ForestController Instance;


    Transform player;
    [SerializeField] Transform playerSpawnSpot;


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

        player = GameObject.FindGameObjectWithTag("Player").transform;
        player.position = playerSpawnSpot.position;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
