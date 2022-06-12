using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public sealed class invmanager : MonoBehaviour
{
    [Header("SlotChangeUi")]
    [SerializeField]
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField]
    EventSystem m_EventSystem;
    [Header("Transforms")]
    [SerializeField]
    private Transform invui;
    [SerializeField]
    private Transform OpenAndCloseInvUi;
    [SerializeField]
    [Header("WeaponSystem")]
    private WeaponSystem WeaponHolder;
    private ItemsScriptableobject tempobj;
    [Header("Player Stop Cam And Movement")]
    [SerializeField]
    private CharacterMovement Player;
    [SerializeField]
    private CameraController MainCam;
    private GameObject HoldTempSlot = null;
    private bool GetTempSlotOnce = false;

    [Header("GetTheSlotsOfItems and getsway")]
    public Image[] slots;
    [SerializeField]
    private Sway[] ItemHolderSway;
    [HideInInspector]
    public bool getpickupscriptonce = true;
    [HideInInspector]
    public ItemPickUp pickup;
    
    [Header("RaycastToPickUpObject")]
    [SerializeField]
    private LayerMask mask;
   // public List<bool> items;
    public void Start()
    {
       // for (int i = 0; i < invui.GetChild(1).childCount; i++)
       // {
       //     slots[i](invui.GetChild(1).GetChild(i).GetChild(0).GetComponent<Image>());
       // }
        invui.gameObject.SetActive(false);
       // WeaponHolder.GetComponentInChildren<Sway>().enabled = false;
        // array[i++] = value;
        // for(int i = 0; i < WeaponHolder.childCount; i++)
        // {
        //     SwayWeapon.Add(WeaponHolder.GetChild(i)
        // }
    }


    private void Update()
    {
        // Raycast -------------------------------------------------------
        if(Physics.Raycast(MainCam.transform.position , MainCam.transform.forward, out  RaycastHit hit, 5 , mask ) && invui.gameObject.activeSelf == false)
        {
     
            if( getpickupscriptonce == true && pickup != hit.transform.GetComponent<ItemPickUp>())
            {
             pickup =  hit.transform.GetComponent<ItemPickUp>();
                getpickupscriptonce = false;
            }
            if(pickup != null)
            {
              pickup.MOUSEHover();
            }
        }
        else if(pickup != null && pickup.IsDisableOrNot == true)
        {
            getpickupscriptonce = true;
          pickup.mousexit();
        }
        // Weapon Swap ------------------------------------------------------
        #region uglycode
        if (Input.GetKeyDown(KeyCode.Alpha1) && invui.gameObject.activeSelf == false )
        {
            for (int i = 0; i < WeaponHolder.transform.childCount; i++)
            {
                //WeaponHolder.GetChild(i).gameObject.SetActive(false);
                ItemHolderSway[i].gameObject.SetActive(false);
                ItemHolderSway[i].enabled = true;
            }
            ItemHolderSway[0].gameObject.SetActive(true);
           // WeaponHolder.GetChild(0).gameObject.SetActive(true);
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && invui.gameObject.activeSelf == false )
        {
            for (int i = 0; i < WeaponHolder.transform.childCount; i++)
            {
                //WeaponHolder.GetChild(i).gameObject.SetActive(false);
                ItemHolderSway[i].gameObject.SetActive(false);
                ItemHolderSway[i].enabled = true;
            }
            ItemHolderSway[1].gameObject.SetActive(true);
            //  WeaponHolder.GetChild(1).gameObject.SetActive(true);
            // WeaponHolder.GetComponentInChildren<Sway>().enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && invui.gameObject.activeSelf == false )
        {
            for (int i = 0; i < WeaponHolder.transform.childCount; i++)
            {
                //WeaponHolder.GetChild(i).gameObject.SetActive(false);
                ItemHolderSway[i].gameObject.SetActive(false);
                ItemHolderSway[i].enabled = true;
            }
            ItemHolderSway[2].gameObject.SetActive(true);
            //WeaponHolder.GetChild(2).gameObject.SetActive(true);
           // WeaponHolder.GetComponentInChildren<Sway>().enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && invui.gameObject.activeSelf == false )
        {
            for (int i = 0; i < WeaponHolder.transform.childCount; i++)
            {
                // WeaponHolder.GetChild(i).gameObject.SetActive(false);
                ItemHolderSway[i].gameObject.SetActive(false);
                ItemHolderSway[i].enabled = true;
            }
            ItemHolderSway[3].gameObject.SetActive(true);
            // WeaponHolder.GetChild(3).gameObject.SetActive(true);
            // WeaponHolder.GetComponentInChildren<Sway>().enabled = true;
        }
        #endregion
        // Open Inv --------------------------------------------------------
        #region OpenInv
        if (Input.GetKeyDown(KeyCode.Q))
        {

            if (invui.gameObject.activeSelf == true)
            {
                
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Player.enabled = true;
                MainCam.enabled = true;
                StartCoroutine(OpeninvOrClose());
                for(int i = 0; i < WeaponHolder.transform.childCount; i++)
                {
                    ItemHolderSway[i].enabled = true;
                }
                
               
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Player.enabled = false;
                MainCam.enabled = false;

                for (int i = 0; i < WeaponHolder.transform.childCount; i++)
                {
                    ItemHolderSway[i].enabled = false;
                }
                StartCoroutine(OpeninvOrClose());
            }

        }
    }
    private IEnumerator OpeninvOrClose()
    {
        float timelerp = 0;
        if (invui.gameObject.activeSelf == true)
        {
            while (timelerp <= 0.3f)
            {
                timelerp += Time.deltaTime * 0.7f;
                invui.position = Vector3.Lerp(invui.position, OpenAndCloseInvUi.GetChild(0).position, timelerp / 1);
                yield return new WaitForFixedUpdate();
            }
            
            invui.gameObject.SetActive(false);
        }
        else
        {
            invui.gameObject.SetActive(true);
            while (timelerp <= 0.3f)
            {
                timelerp += Time.deltaTime * 0.7f;
                invui.position = Vector3.Lerp(invui.position, OpenAndCloseInvUi.position, timelerp / 1);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    #endregion
    #region DragUi
    public void DragUi()
    {
        StartCoroutine(DragUiNumerator());
    }
    private IEnumerator DragUiNumerator()
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position

        m_Raycaster.Raycast(m_PointerEventData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        while (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse2))
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("itemSlot"))
                {
                    if (GetTempSlotOnce == false)
                    {
                       // for (int i = 0; i < slots.Length; i++)
                       // {
                       //     slots[i].tag = "Untagged";
                       // }
                        HoldTempSlot = result.gameObject;
                        //HoldTempWeapon = HoldTempSlot.transform.parent
                      //  result.gameObject.tag = "itemSlot";
                        HoldTempSlot.transform.position = result.gameObject.transform.position;
                    }
                    GetTempSlotOnce = true;
                    result.gameObject.transform.position = Input.mousePosition;
                    if (Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        break;
                    }
                }

            }
            yield return new WaitForFixedUpdate();
        }

    }
    #endregion
    #region StopDraging
    public void EndDragUi()
    {
        StopCoroutine(DragUiNumerator());
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("ItemSlotHolder") && WeaponHolder.transform.childCount > 1 && result.gameObject.transform.GetSiblingIndex() <= WeaponHolder.transform.childCount)
            {
                if(WeaponHolder.transform.GetChild(result.gameObject.transform.GetSiblingIndex()).childCount < 1)
                {
                    // change item in weaponholder slot
                    WeaponHolder.transform.GetChild(HoldTempSlot.transform.parent.GetSiblingIndex()).GetChild(0).SetParent(WeaponHolder.transform.GetChild(result.gameObject.transform.GetSiblingIndex()));
                    // Change arrayslots
                    tempobj =  WeaponHolder.Items[HoldTempSlot.transform.parent.GetSiblingIndex()];
                    WeaponHolder.Items[HoldTempSlot.transform.parent.GetSiblingIndex()] = WeaponHolder.Items[result.gameObject.transform.GetSiblingIndex()];
                    WeaponHolder.Items[result.gameObject.transform.GetSiblingIndex()] = tempobj;
                }
                else
                {
                    // change item in weaponholder slot
                    WeaponHolder.transform.GetChild(result.gameObject.transform.GetSiblingIndex()).GetChild(0).SetParent(WeaponHolder.transform.GetChild(HoldTempSlot.transform.parent.GetSiblingIndex()));

                    WeaponHolder.transform.GetChild(HoldTempSlot.transform.parent.GetSiblingIndex()).GetChild(0).SetParent(WeaponHolder.transform.GetChild(result.gameObject.transform.GetSiblingIndex()));
                    // Change arrayslots
                    tempobj = WeaponHolder.Items[HoldTempSlot.transform.parent.GetSiblingIndex()];
                    WeaponHolder.Items[HoldTempSlot.transform.parent.GetSiblingIndex()] = WeaponHolder.Items[result.gameObject.transform.GetSiblingIndex()];
                    WeaponHolder.Items[result.gameObject.transform.GetSiblingIndex()] = tempobj;
                }
                HoldTempSlot.transform.position = result.gameObject.transform.position;
                

                //WeaponHolder.GetChild(result.gameObject.transform.GetSiblingIndex()).SetSiblingIndex(HoldTempSlot.transform.parent.GetSiblingIndex());

                invui.GetChild(1).GetChild(result.gameObject.transform.GetSiblingIndex()).GetChild(0).transform.position = HoldTempSlot.transform.parent.position;
                invui.GetChild(1).GetChild(result.gameObject.transform.GetSiblingIndex()).GetChild(0).SetParent(HoldTempSlot.transform.parent);
                HoldTempSlot.transform.SetParent(invui.GetChild(1).GetChild(result.gameObject.transform.GetSiblingIndex()));


            }
            else
            {
                HoldTempSlot.transform.position = HoldTempSlot.transform.parent.position;
            }
        }
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].tag = "itemSlot";
        }
        GetTempSlotOnce = false;
        // HoldTempSlot.GetComponent<Image>().raycastTarget = true;
    }
    #endregion
}

