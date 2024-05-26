using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageController : MonoBehaviour
{
    public static VillageController Instance;

    Transform player;
    [SerializeField] Transform playerSpawnSpot;


    public Transform KakRenTransform;
    public Transform TengahKota;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //player.position = playerSpawnSpot.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEventSystem.Instance.DoneDialogue_1 && !GameEventSystem.Instance.DoneDialogue_2)
        {
            Player_Direction.Instance.Target = KakRenTransform;
        }
        if (!GameEventSystem.Instance.DoneDialogue_3 && GameEventSystem.Instance.DoneDialogue_2)
        {
            Player_Direction.Instance.Target = TengahKota;
        }
    }
}
