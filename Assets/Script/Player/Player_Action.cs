using System.Collections;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class Player_Action : MonoBehaviour
{
    public static Player_Action Instance;

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
    public bool canAttack = true;
    [SerializeField] GameObject normalAttackHitArea;
    [SerializeField] GameObject specialAttackHitArea;

    float damageMult = 1;
    #endregion

    #region QUICK_SLOTS
    [Header("QUICK SLOTS")]
    [SerializeField] float quickSlotCD = 2f;
    float[] quickSlotsTimer = new float[2];
    bool[] canQuickSlots = new bool[2];
    #endregion

    #region INTERACTS
    [Header("INTERACTS")]
    [SerializeField] bool drawInteractCircle;
    [SerializeField] LayerMask interactablesLayer;
    [SerializeField] float interactsRadius = 2f;
    bool canInteract = false;
    Interactable interactable;
    #endregion


    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (GameController.Instance.enablePlayerInput)
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
                    StartCoroutine(HandleUICD(PlayerUI.Instance.specialAttackUI, Player_Inventory.Instance.equippedWeapon.SpecialAttackCD));
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
                    StartCoroutine(HandleUICD(PlayerUI.Instance.quickSlotsUI_HUD[0], quickSlotCD));
                }
            }
            HandleQuickSLotUI(1);
            if (Input.GetKeyDown(quickSlot2) && canQuickSlots[1])
            {
                // quick slot 2
                if (Player_Inventory.Instance.quickSlots[1] != null)
                {
                    Player_Inventory.Instance.UseQuickSlot(2);
                    StartCoroutine(HandleUICD(PlayerUI.Instance.quickSlotsUI_HUD[1], quickSlotCD));
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
    }

    #region UI_HELPER
    // Handling Quick slot cooldown after using
    void HandleQuickSLotUI(int i)
    {
        if (!canQuickSlots[i])
        {
            quickSlotsTimer[i] += Time.deltaTime;
            PlayerUI.Instance.quickSlotsUI_HUD[i].fillAmount = quickSlotsTimer[i] / quickSlotCD;
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
            image.fillAmount = (Time.time - startTime) / cd;
            yield return null;
        }
        image.fillAmount = 1;
    }
    #endregion

    // Helper function for checking interactables nearby
    bool CheckInteractables()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, interactsRadius, Vector2.up, 0, interactablesLayer); // yang ngurus layer ada di sini
        // jadi ini cek kalo ada interactable, bisa interact, kalo gaada ya gabisa
        if (hit.transform != null)
        {
            interactable = hit.transform.GetComponent<Interactable>();
            PlayerUI.Instance.promptText.text = "Press F to "+interactable.promptMessage;
            return true;
        }
        else
        {
            PlayerUI.Instance.promptText.text = string.Empty;
            return false;
        }
    }


    #region COMBAT_ACTIONS
    public void ActivateHitbox(int damage, float area, float howLong = .5f, bool AOE = false)
    {
        if (!AOE)
        {
            Transform theTransform = normalAttackHitArea.transform;
            theTransform.name = damage.ToString();
            theTransform.localPosition = new(area / 2, theTransform.localPosition.y, theTransform.localPosition.z);
            theTransform.localScale = new(area, theTransform.localScale.y, theTransform.localScale.z);
            StartCoroutine(activatingHitbox(normalAttackHitArea, howLong));
        }
        else
        {
            // Adding constant so the area isn't too small
            area += 1;
            Transform theTransform = specialAttackHitArea.transform;
            theTransform.name = damage.ToString();
            theTransform.localScale = new(area, area, theTransform.localScale.z);
            StartCoroutine(activatingHitbox(specialAttackHitArea, howLong));
        }
    }

    IEnumerator activatingHitbox(GameObject theHitbox, float howLong)
    {
        theHitbox.SetActive(true);
        yield return new WaitForSeconds(howLong);
        theHitbox.SetActive(false);
        canAttack = true;
    }

    public void Attack()
    {
        Item itemToAttack = Player_Inventory.Instance.equippedWeapon;
        if (itemToAttack.itemName == "Empty")
            return;

        if (itemToAttack.type == ItemType.Melee_Combat)
        {
            print("melee normal attacking");
            ActivateHitbox(itemToAttack.Damage, itemToAttack.AreaOfEffect);
            StartCoroutine(ActivateAttack(.5f));
        }
        else if (itemToAttack.itemName == "Batu")
        {
            print("throwing rock");
            // throw rock
            StartCoroutine(ShootProjectile(itemToAttack.RangedWeapon_ProjectilePrefab, itemToAttack.Damage));
            // check if rock depleted after use then remove as equipped then remove from inventory
            if (Player_Inventory.Instance.equippedWeapon.stackCount == 1)
            {
                Player_Inventory.Instance.EquipItem(ItemPool.Instance.GetItem("Empty"), 1);
            }

            // minus rock count
            Player_Inventory.Instance.RemoveItem(ItemPool.Instance.GetItem("Batu"));

            StartCoroutine(ActivateAttack(.5f));
        }
        else if (itemToAttack.type == ItemType.Ranged_Combat)
        {
            // Check for arrow first
            if (Player_Inventory.Instance.itemList.Exists(x => x.itemName == "Anak Panah"))
            {
                print("shooting arrow");
                // Shoot arrow if possible
                StartCoroutine(ShootProjectile(itemToAttack.RangedWeapon_ProjectilePrefab, itemToAttack.Damage));
                // minus arrow count
                Player_Inventory.Instance.RemoveItem(ItemPool.Instance.GetItem("Anak Panah"));
            }
            else
            {
                print("no arrow bish");
            }
            StartCoroutine(ActivateAttack(1));
        }
    }

    public void SpecialAttack()
    {
        Item itemToAttack = Player_Inventory.Instance.equippedWeapon;
        if (itemToAttack.itemName == "Empty")
            return;

        if (itemToAttack.itemName == "Penyiram Tanaman")
        {
            print("watering plants");
            // Water nearby plants
        }
        else if (itemToAttack.itemName == "Pedang Ren")
        {
            print("buffing");
            // Buff
            StartCoroutine(StartBuff_PedangRen(30));
            StartCoroutine(ActivateAttack(1));
        }
        else if (itemToAttack.type == ItemType.Melee_Combat)
        {
            print("special attacking with a stick");
            ActivateHitbox(itemToAttack.Damage * 4, itemToAttack.AreaOfEffect, 1, true);
            StartCoroutine(ActivateAttack(1));
        }
        else if (itemToAttack.itemName == "Batu")
        {
            print("rock no special attack");
        }
        else if (itemToAttack.type == ItemType.Ranged_Combat)
        {
            print("bow special attack");
            for (int i = 0; i < 5; i++)
            {
                // Check for arrow first
                if (Player_Inventory.Instance.itemList.Exists(x => x.itemName == "Anak Panah"))
                {
                    print("shooting arrow");
                    // Shoot arrow if possible
                    StartCoroutine(ShootProjectile(itemToAttack.RangedWeapon_ProjectilePrefab, itemToAttack.Damage, i * .1f));
                    // minus arrow count
                    Player_Inventory.Instance.RemoveItem(ItemPool.Instance.GetItem("Anak Panah"));
                }
                else
                {
                    print("no arrow bish");
                }
            }
            StartCoroutine(ActivateAttack(1));
        }
    }

    IEnumerator ActivateAttack(float dur)
    {
        canAttack = false;
        yield return new WaitForSeconds(dur);
        canAttack = true;
    }

    #region WEAPON_SPECIFIC
    IEnumerator StartBuff_PedangRen(float dur)
    {
        damageMult *= 2;
        yield return new WaitForSeconds(dur);
        damageMult /= 2;
    }

    IEnumerator ShootProjectile(GameObject prefab, int damage, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        // Check where to aim using mouse
        Vector2 aimPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Shoot prefab to that general area
        Vector2 rotation = aimPos - (Vector2)transform.position;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        GameObject projectile = Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, rot));
        projectile.name = damage.ToString();
        //GameObject projectile = ObjectPooler.Instance.SpawnFromPool("Bullet", transform.position, Quaternion.Euler(0, 0, rot));
        //projectile.GetComponent<BulletLogic>().SetBullet(bulletSpd, bulletdamage);
        projectile.GetComponent<Rigidbody2D>().AddForce(rotation.normalized * 10, ForceMode2D.Impulse);
        yield return null;
    }
    #endregion

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
