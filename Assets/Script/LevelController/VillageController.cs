using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageController : MonoBehaviour
{
    public static VillageController Instance;

    Transform player;
    [SerializeField] Transform playerSpawnSpot;

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
        
    }
}
