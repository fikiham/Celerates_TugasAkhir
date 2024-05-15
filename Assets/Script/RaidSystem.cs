using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RaidSystem : MonoBehaviour
{
    [SerializeField] Transform spawners;

    [SerializeField] bool raidStart = false;
    [SerializeField] bool raiding = false;
    [SerializeField] bool winStatus = false;

    [SerializeField] float raidTimeLimit = 5 * 60;
    float raidTimer;

    [SerializeField] float gracePeriod = 5;
    float periodTimer;

    [SerializeField] int totalWave;
    [SerializeField] int currentWave;
    [SerializeField] int enemiesNumber;
    [SerializeField] int enemiesNumbers;

    [Header("UI STUFF")]
    [SerializeField] GameObject raidUI;
    [SerializeField] TMP_Text raidTimerText;
    [SerializeField] Image raidProgress;
    [SerializeField] GameObject raidEndUI;
    [SerializeField] TMP_Text raidEndText;

    // Update is called once per frame
    void Update()
    {
        if (raidStart)
        {
            if (raiding)
            {
                raidTimer += Time.deltaTime;
                raidTimerText.text = (raidTimer / 60).ToString("F0") + ":" + (int)(raidTimer % 60);
                if (raidTimer > raidTimeLimit)
                    RaidEnd(false);

                // Always check enemy number
                enemiesNumber = GetCurrentNumber();
                raidProgress.fillAmount = (float)enemiesNumber / enemiesNumbers;

                // If enemy all died, go to next wave or end raid
                if (enemiesNumber <= 0)
                {
                    // If there are no next wave, raid finished
                    if (currentWave >= totalWave)
                        RaidEnd(true);
                    else
                        NextWave();
                }
            }
            else
            {
                if (winStatus)
                {
                    // Win
                    raidEndText.color = Color.green;
                    raidEndText.text = "good enough i guess";
                }
                else
                {
                    // Lost
                    raidEndText.color = Color.red;
                    raidEndText.text = "dumb b****";
                }
            }
        }
    }

    [ContextMenu("Start Raid")]
    void StartRaidFromMenu()
    {
        StartRaid(1);
    }

    void StartRaid(int totalWave)
    {
        raidStart = true;
        raiding = true;
        raidUI.SetActive(true);
        SetNumberTotal(enemiesNumber);
        enemiesNumbers = enemiesNumber;
        StartCoroutine(EnableSpawners());
        currentWave = 1;
        this.totalWave = totalWave;
    }

    void NextWave()
    {
        // Wait a sec before next wave
        periodTimer += Time.deltaTime;
        if (periodTimer > gracePeriod)
        {
            periodTimer = 0;
            currentWave++;
            enemiesNumbers = totalWave * 5;
            SetNumberTotal(enemiesNumbers);
            StartCoroutine(EnableSpawners());
        }
    }

    void RaidEnd(bool win)
    {
        raiding = false;
        winStatus = win;
        raidEndUI.SetActive(true);
    }

    IEnumerator EnableSpawners()
    {
        foreach (Transform spawner in spawners)
        {
            spawner.GetComponent<Enemy_Spawner>().CanSpawn = true;
        }
        yield return new WaitForSeconds(1f);
        foreach (Transform spawner in spawners)
        {
            spawner.GetComponent<Enemy_Spawner>().CanSpawn = false;
        }
    }

    int GetCurrentNumber()
    {
        int total = 0;
        foreach (Transform spawner in spawners)
        {
            total += spawner.GetComponent<Enemy_Spawner>().enemies.Count;
        }
        return total;
    }

    void SetNumberTotal(int numbers)
    {
        int number = numbers / spawners.childCount;
        foreach (Transform spawner in spawners)
        {
            spawner.GetComponent<Enemy_Spawner>().spawnCount = number;
        }
    }
}
