using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantInteractable : Interactable
{
    // ya intinya inherit Interactable

    // terus bikin function

    // Misal aku bikin logika tanemanya
    float siramValue = 0;
    protected override void Interact()
    {
        // nah disini masukkin logika interaksinya

        if (siramValue >= 100)
        {
            // kalo siramValue lebih dari 100, masukkin taneman ke inventory
            print("panen bro");
        }
        else
        {
            // kalo belom siap panen, ya berarti siram
            siramValue += 30;
            print("Sirammmm");
        }
    }

}
