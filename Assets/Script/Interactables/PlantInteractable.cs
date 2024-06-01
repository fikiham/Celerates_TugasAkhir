using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantInteractable : Interactable
{
    public Seed seed; // Changed to public to be accessible

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
        else if (seed.siram)
        {
            promptMessage = "Siram Tanaman";
        }
        else
        {
            promptMessage = "Tanaman Tumbuh";
        }
    }

    protected override void Interact()
    {
        if (seed.isReadyToHarvest)
        {
            seed.Harvest();
        }
    }
}
