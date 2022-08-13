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
    [SerializeField]
    private WeaponSystem WeaponHolder;

    [Header("UiTexts")]
    [SerializeField]
    private TextMeshProUGUI HoverPickUpText;
    [HideInInspector]
    public bool IsDisableOrNot;

   // private Vector3 UiPickUpNormalSize = Vector3.one;
   // private Vector3 UiPickUpHideSize = Vector3.zero;
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
            OffHover();
        }
        else if (Vector3.Distance(transform.position, Player.position) <= 5 && IsDisableOrNot == false)
        {
            HoverPickUpText.text = Item.ItemName;
            IsDisableOrNot = true;
            OnHover();
        }
        else if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(transform.position, Player.position) <= 5)
        {
            ItemGet();
        }
    }


    private void ItemGet()
    {
        // inv.items.Add(Item.IsActive);\

        inv.getpickupscriptonce = true;
        inv.pickup = null;

        for (int i = 0; i < WeaponHolder.transform.childCount; i++)
        {
            if (WeaponHolder.transform.GetChild(i).childCount > 0)
            {
                inv.ItemHolderSway[i].enabled = false;
                // WeaponHolder.transform.GetChild(i).GetComponentInChildren<Sway>().enabled = false;
            }
            inv.ItemHolderSway[i].gameObject.SetActive(false);
            //  WeaponHolder.transform.GetChild(i).gameObject.SetActive(false);

        }

        for (int i = 0; i < WeaponHolder.transform.childCount; i++)
        {
            //if ( Item.CanAttack == false)
            //{
            //    // WeaponHolder.transform.GetChild(i).gameObject.SetActive(true);
            //    transform.GetChild(0).gameObject.SetActive(true);
            //    inv.ItemHolderSway[i].gameObject.SetActive(true);
            //    transform.GetChild(0).position = WeaponHolder.transform.position;
            //    transform.GetChild(0).rotation = WeaponHolder.transform.rotation;
            //    transform.GetChild(0).GetChild(0).parent = WeaponHolder.transform.GetChild(i);
            //
            //    // Gets the animator from the itemholder -------------------------------------------------------------------------------------------------------------------------------
            //    WeaponHolder.WeaponAnim = inv.ItemHolderAnimator[i];
            //    // somehow this fixes the animation from bugging out because the animator doesnt render that it has the child fast enough so we close it and open it again to refresh it
            //    WeaponHolder.WeaponAnim.gameObject.SetActive(false);
            //    WeaponHolder.WeaponAnim.gameObject.SetActive(true);
            //    //gets this item and puts it in the inv of items
            //    WeaponHolder.Items[i] = Item;
            //    WeaponHolder.Index = i;
            //    break;
            //}
            // else if (Item.CanAttack == true)
            // {
            if (inv.ItemHolderSway[i].transform.childCount > 0)
            {
              // for some reason if i leave it empty it just changes the slot to the next instead of glitching IDK HOW THIS WORKS
            }
            else
            {


                transform.GetChild(0).gameObject.SetActive(true);
                inv.ItemHolderSway[i].gameObject.SetActive(true);
                transform.GetChild(0).position = WeaponHolder.transform.position;
                transform.GetChild(0).rotation = WeaponHolder.transform.rotation;
                transform.GetChild(0).GetChild(0).parent = WeaponHolder.transform.GetChild(i);

                // Gets the animator from the itemholder -------------------------------------------------------------------------------------------------------------------------------
                WeaponHolder.WeaponAnim = inv.ItemHolderAnimator[i];
                // somehow this fixes the animation from bugging out because the animator doesnt render that it has the child fast enough so we close it and open it again to refresh it
                WeaponHolder.WeaponAnim.gameObject.SetActive(false);
                WeaponHolder.WeaponAnim.gameObject.SetActive(true);
                //gets this item and puts it in the inv of items
                WeaponHolder.Items[i] = Item;
                WeaponHolder.Index = i;
                break;
            }
          //  }
        }

        // Destroy(boxcollider);
        // Destroy(DisableScript);

        UiPopUpHover.localScale = Vector3.zero;
        for (int i = 0; i < inv.slots.Length; i++)
        {
            if (inv.slots[i].enabled == false)
            {

                inv.slots[i].enabled = true;
                inv.slots[i].sprite = Item.ItemIcon;
                break;
            }

        }

        Destroy(gameObject);
    }


    public void mousexit()
    {
        IsDisableOrNot = false;
        OffHover();
    }

    private void OffHover()
    {
        LeanTween.scale(UiPopUpHover.gameObject, Vector3.zero, 60.0f * Time.deltaTime).setEaseOutBounce();
        //LerpTime = 0;
        //while (LerpTime < 0.3f)
        //{
        //    LerpTime += 0.6f * Time.deltaTime;
        //    UiPopUpHover.localScale = Vector3.Lerp(UiPopUpHover.localScale, UiPickUpHideSize, LerpTime / 1);
        //    if (IsDisableOrNot == true)
        //    {
        //        break;
        //    }
        //    yield return new WaitForFixedUpdate();
        //}

    }
    private void OnHover()
    {
        LeanTween.scale(UiPopUpHover.gameObject, Vector3.one, 60.0f * Time.deltaTime).setEaseOutBounce();
        //LerpTime = 0;
        //while (LerpTime < 0.3f)
        //{
        //    LerpTime += 0.6f * Time.deltaTime;
        //    UiPopUpHover.localScale = Vector3.Lerp(UiPopUpHover.localScale, UiPickUpNormalSize, LerpTime / 1);
        //    if (IsDisableOrNot == false)
        //    {
        //        break;
        //    }
        //    yield return new WaitForFixedUpdate();
        //}

    }
}
