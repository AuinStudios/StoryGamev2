using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;

public sealed class ItemPickUp : MonoBehaviour
{


    [Header("Inv And Scriptableobject")]
    [SerializeField]
    private ItemsScriptableobject Item;
    [SerializeField]
    private invmanager inv;
    [Header("Transforms")]
    [SerializeField]
    private Transform UiPopUpHover;
    [SerializeField]
    private Transform Player;
    // [SerializeField]
    // private Transform ItemPrefab;
    [SerializeField]
    private Transform WeaponHolder;
    [Header("UiTexts")]
    [SerializeField]
    private TextMeshProUGUI UiText;
    [Header("DisableScript  And MakeBoxColliderTrigger")]
    [SerializeField]
    private BoxCollider boxcollider;
    [SerializeField]
    private ItemPickUp DisableScript;
    //[SerializeField]
    // private Sway SwayActive;
    private float LerpTime = 0;
    [HideInInspector]
    public bool IsDisableOrNot;

    private Vector3 UiPickUpNormalSize = new Vector3(1, 1, 1);
    private Vector3 UiPickUpHideSize = new Vector3(0, 0, 0);
    #region
    //private void OnMouseEnter()
    //{
    //    if (Vector3.Distance(transform.position, Player.position) <= 5)
    //    {
    //        Text.text = Item.ItemName;
    //        IsDisableOrNot = true;
    //        StartCoroutine(OnHover());
    //    }
    //
    //}
    #endregion
    public void MOUSEHover()
    {
        if (Vector3.Distance(transform.position, Player.position) >= 5 && IsDisableOrNot == true)
        {
            IsDisableOrNot = false;
            StartCoroutine(OffHover());
        }
        else if (Vector3.Distance(transform.position, Player.position) <= 5 && IsDisableOrNot == false)
        {
            UiText.text = Item.ItemName;
            IsDisableOrNot = true;
            StartCoroutine(OnHover());
        }
        if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(transform.position, Player.position) <= 5)
        {
            // inv.items.Add(Item.IsActive);
            inv.getpickupscriptonce = true;
            inv.pickup = null;

            for (int i = 0; i < WeaponHolder.childCount; i++)
            {
                if (WeaponHolder.GetChild(i).childCount > 0)
                {
                    WeaponHolder.GetChild(i).GetComponentInChildren<Sway>().enabled = false;
                }

                WeaponHolder.GetChild(i).gameObject.SetActive(false);

            }

            for (int i = 0; i < WeaponHolder.childCount; i++)
            {
                if (WeaponHolder.GetChild(i).childCount == 0)
                {
                    WeaponHolder.GetChild(i).gameObject.SetActive(true);
                    transform.position = WeaponHolder.position;
                    transform.rotation = WeaponHolder.rotation;
                    transform.parent = WeaponHolder.GetChild(i);
                    break;
                }

            }

            boxcollider.isTrigger = true;
            Destroy(DisableScript);
            //SwayActive.enabled = true;

            UiPopUpHover.localScale = UiPickUpHideSize;
            for (int i = 0; i < inv.slots.Length; i++)
            {
                if (inv.slots[i].enabled == false)
                {

                    inv.slots[i].enabled = true;
                    inv.slots[i].sprite = Item.ItemIcon;
                    break;
                }

            }
        }
    }
     public void mousexit()
     {
         IsDisableOrNot = false;
         StartCoroutine(OffHover());
     }

    private IEnumerator OffHover()
    {
        LerpTime = 0;
        while (LerpTime < 0.3f)
        {
            LerpTime += 0.6f * Time.deltaTime;
            UiPopUpHover.localScale = Vector3.Lerp(UiPopUpHover.localScale, UiPickUpHideSize, LerpTime / 1);
            if (IsDisableOrNot == true)
            {
                break;
            }
            yield return new WaitForFixedUpdate();
        }

    }
    private IEnumerator OnHover()
    {
        LerpTime = 0;
        while (LerpTime < 0.3f)
        {
            LerpTime += 0.6f * Time.deltaTime;
            UiPopUpHover.localScale = Vector3.Lerp(UiPopUpHover.localScale, UiPickUpNormalSize, LerpTime / 1);
            if (IsDisableOrNot == false)
            {
                break;
            }
            yield return new WaitForFixedUpdate();
        }

    }
}
