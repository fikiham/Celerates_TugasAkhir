using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageController : MonoBehaviour
{
    public static VillageController Instance;

    Transform player;
    [SerializeField] Transform playerSpawnSpot;


    public Transform KakRenTransform;
    public Transform TanamanCabaiTransform;
    public Transform TengahKota;
    public Transform SumberBandit;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        player.position = playerSpawnSpot.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEventSystem.Instance.DoneDialogue_3 && !GameEventSystem.Instance.DoneDialogue_4)
        {
            Player_Direction.Instance.Target = KakRenTransform;
        }
        if (!GameEventSystem.Instance.DoneDialogue_6 && GameEventSystem.Instance.DoneDialogue_5)
        {
            Player_Direction.Instance.Target = TengahKota;
        }
    }
}
