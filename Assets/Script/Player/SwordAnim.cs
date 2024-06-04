using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAnim : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] SpriteRenderer sr;

    public void StartFX()
    {
        StopAllCoroutines();
        StartCoroutine(Slashing());
    }

    IEnumerator Slashing()
    {
        sr.enabled = true;
        int currentFrame = 0;
        while (currentFrame < sprites.Length)
        {
            sr.sprite = sprites[currentFrame];
            currentFrame++;
            yield return new WaitForSeconds(.1f);
        }
        sr.enabled = false;
    }
}
