using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Action : MonoBehaviour
{
    #region KEYBINDINGS
    KeyCode normalInput = KeyCode.Mouse0;
    KeyCode specialInput = KeyCode.Mouse1;
    KeyCode actionInput = KeyCode.F;

    KeyCode quickSlot1 = KeyCode.Q;
    KeyCode quickSlot2 = KeyCode.E;
    #endregion

    #region COMBAT
    [Header("COMBAT")]
    public bool combatMode = false;
    bool canAttack = true;
    [SerializeField] GameObject normalAttackHitArea;
    [SerializeField] float normalAttackCD = .5f;
    [SerializeField] GameObject specialAttackHitArea;
    [SerializeField] Image specialAttackUI;
    [SerializeField] float specialAttackCD = 1;
    #endregion

    #region QUICK_SLOTS
    [Header("QUICK SLOTS")]
    [SerializeField] Image[] quickSlotsUI = new Image[2];
    [SerializeField] float quickSlotCD = 2f;
    float[] quickSlotsTimer = new float[2];
    bool[] canQuickSlots = new bool[2];
    #endregion

    #region INTERACTS
    [Header("INTERACTS")]
    [SerializeField] bool drawInteractCircle;
    [SerializeField] TMP_Text promptText;
    [SerializeField] LayerMask interactablesLayer;
    [SerializeField] float interactsRadius = 2f;
    bool canInteract = false;
    Interactable interactable;
    #endregion


    void Update()
    {
        #region INPUTS_ACTION
        // Main Action (Attacking)
        if (Input.GetKeyDown(normalInput))
        {
            if (combatMode && canAttack)
            {
                Attack();
            }
        }

        // Secondary Action (Sepcial Attacking)
        if (Input.GetKeyDown(specialInput))
        {
            if (combatMode && canAttack)
            {
                SpecialAttack();
                StartCoroutine(HandleUICD(specialAttackUI, specialAttackCD));
            }
        }

        // Quick slots
        HandleQuickSLotUI(0);
        if (Input.GetKeyDown(quickSlot1) && canQuickSlots[0])
        {
            // quick slot 2
            if (Player_Inventory.Instance.quickSlots[0] != null)
            {
                Player_Inventory.Instance.UseQuickSlot(1);
                StartCoroutine(HandleUICD(quickSlotsUI[0], quickSlotCD));
            }
        }
        HandleQuickSLotUI(1);
        if (Input.GetKeyDown(quickSlot2) && canQuickSlots[1])
        {
            // quick slot 2
            if (Player_Inventory.Instance.quickSlots[1] != null)
            {
                Player_Inventory.Instance.UseQuickSlot(2);
                StartCoroutine(HandleUICD(quickSlotsUI[1], quickSlotCD));
            }
        }

        // Interact action (for interacting with environment)
        canInteract = CheckInteractables();
        // jadi logikanya cek dulu di sekitar ada yang bisa di interact gak
        if (Input.GetKeyDown(actionInput))
        { 
            // terus kalo udah dicek, baru bisa pencet interact
            if (canInteract)
            {
                interactable.BaseInteract();
            }
        }
        #endregion
    }

    // Handling Quick slot cooldown after using
    void HandleQuickSLotUI(int i)
    {
        if (!canQuickSlots[i])
        {
            quickSlotsTimer[i] += Time.deltaTime;
            quickSlotsUI[i].fillAmount = quickSlotsTimer[i] / quickSlotCD;
            if (quickSlotsTimer[i] > quickSlotCD)
            {
                quickSlotsTimer[i] = 0;
                canQuickSlots[i] = true;
            }
        }
    }

    // Handling the spiral animation for UIs
    IEnumerator HandleUICD(Image image, float cd)
    {
        float startTime = Time.time;
        while (Time.time < startTime + cd)
        {
            image.fillAmount = Time.time - startTime / cd;
            yield return null;
        }
        image.fillAmount = 1;
    }

    // Helper function for checking interactables nearby
    bool CheckInteractables()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, interactsRadius, Vector2.up, 0, interactablesLayer); // yang ngurus layer ada di sini
        // jadi ini cek kalo ada interactable, bisa interact, kalo gaada ya gabisa
        if (hit.transform != null)
        {
            interactable = hit.transform.GetComponent<Interactable>();
            promptText.text = interactable.promptMessage;
            return true;
        }
        else
        {
            promptText.text = string.Empty;
            return false;
        }
    }


    #region COMBAT_ACTIONS
    void Attack()
    {
        canAttack = false;
        StartCoroutine(Attacking());
    }

    IEnumerator Attacking()
    {
        normalAttackHitArea.SetActive(true);
        yield return new WaitForSeconds(normalAttackCD);
        normalAttackHitArea.SetActive(false);
        canAttack = true;
    }

    void SpecialAttack()
    {
        canAttack = false;
        StartCoroutine(SpecialAttacking());
    }

    IEnumerator SpecialAttacking()
    {
        specialAttackHitArea.SetActive(true);
        yield return new WaitForSeconds(specialAttackCD);
        specialAttackHitArea.SetActive(false);
        canAttack = true;
    }
    #endregion

    #region DEBUG
    private void OnDrawGizmos()
    {
        if (drawInteractCircle)
        {
            Gizmos.DrawWireSphere(transform.position, interactsRadius);
        }
    }
    #endregion
}
