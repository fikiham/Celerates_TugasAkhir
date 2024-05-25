using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantInteractable : Interactable
{
    Seed seed;
    private void Start()
    {
        seed = GetComponent<Seed>();
    }

    private void Update()
    {
        if (seed.isReadyToHarvest)
        {
            promptMessage = "Panen Tanaman";
        }
        else
        {
            promptMessage = "Tanaman Sedang Tumbuh";
        }

    }
    protected override void Interact()
    {
        if (seed.isReadyToHarvest)
        {
            seed.Harvest();
        }else if(seed.siram){
            seed.Siram();
        }
    }

}
