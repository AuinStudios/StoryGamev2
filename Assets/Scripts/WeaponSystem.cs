// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this WeaponSystem do?
/// </summary>
public sealed class WeaponSystem : MonoBehaviour
{
    #region
    // [Header("Recoil")]
    // private Vector3 pos1;
    // private Vector3 pos2;
    // //[SerializeField]
    // //private float RecoilX = -3;
    // //[SerializeField]
    // //private float RecoilY = -3;
    // //[SerializeField]
    // //private float RecoilZ = -3;
    // [SerializeField]
    // private Vector2 lerptowardsrecoil;
    //
    // [SerializeField]
    // private float returnspeed = 6;
    // [SerializeField]
    // private float snapiness = 6;
    // // --------------------------------------------
    // [SerializeField]
    // private Transform head;
    #endregion
    [SerializeField]
    private AudioSource hitsound;
    [Header("Anim Stuff")]
    [SerializeField]
    private Animator WeaponAnim;
    [Header("WeaponSwapValues")]
    [HideInInspector]
    public ItemsScriptableobject[] Items;
    [HideInInspector]
    public int Index = 0;
    // float values;
    private float ChargeDamage;
    private float TimeUntllCharge;
    private float cooldown = 0.0f;
    private float SphereRadius = 0.3f;
    // Update is called once per frame
    private void Start()
    {

    }
    private void Update()
    {
        // cooldown ---------------------------------------------------------------------------------------
        cooldown = cooldown > 0 ? cooldown -= Time.deltaTime : cooldown = 0;
        // recoil ---------------------------------------------------------------------------------------

        // weaponsystem ---------------------------------------------------------------------------------------
        if (Input.GetMouseButton(0) && Items[Index] != null && Items[Index].CanAttack == true && cooldown <= 0)
        {
            if (TimeUntllCharge <= Items[Index].MaxDamage)
            {
                TimeUntllCharge += 0.35f * Time.deltaTime;

            }
            if (TimeUntllCharge > 0.1f)
            {
                ChargeDamage += Items[Index].MaxDamage / 2 * Time.deltaTime;
            }
            if(WeaponAnim.GetBool("chargeing") == false && TimeUntllCharge > 0.1f)
            {
                WeaponAnim.SetBool("chargeing", true);
            }
        }
        else if (Input.GetMouseButtonUp(0) && Items[Index] != null && Items[Index].CanAttack == true && cooldown <= 0)
        {
            //TimeUntllCharge = 0;
            //ChargeDamage = 0;
            if (TimeUntllCharge > 0.1f)
            {
                cooldown = 2.3f;
                WeaponAnim.SetBool("chargeing", false);
                //StartCoroutine(recoil());
                StartCoroutine(HitObject(0 , 30));
                TimeUntllCharge = 0;
            }
            else
            {
                TimeUntllCharge = 0;
                cooldown = 1.3f;
                WeaponAnim.SetTrigger("Swing");
                StartCoroutine(HitObject(0.3f , 15));
            }
        }
    }
    #region recoilstuff
    //private IEnumerator recoil()
    //{
    //    //pos1 = new Vector3(0, 0, 0);
    //    //down = Quaternion.Euler(10, -20, 0);
    //
    //    int i = 0;
    //    while (i < 30)
    //    {
    //        pos1 = Vector3.Lerp(pos1, Vector3.zero, returnspeed * Time.deltaTime);
    //        pos2 = Vector3.Slerp(pos2, pos1, snapiness * Time.fixedDeltaTime);
    //        head.localRotation = Quaternion.Euler(pos1);
    //        pos1 += new Vector3(lerptowardsrecoil.x, lerptowardsrecoil.y, 0);
    //        i++;
    //        yield return new WaitForFixedUpdate();
    //    }
    //    i = 0;
    //    while (i < 60)
    //    {
    //        pos1 = Vector3.Lerp(pos1, Vector3.zero, returnspeed * Time.deltaTime);
    //        pos2 = Vector3.Slerp(pos2, pos1, snapiness * Time.fixedDeltaTime);
    //        head.localRotation = Quaternion.Euler(pos1);
    //        pos1 += new Vector3(0, 0, 0);
    //
    //        i++;
    //        yield return new WaitForFixedUpdate();
    //    }
    //    head.localRotation = Quaternion.Euler(0, 0, 0);
    //}
    #endregion
    private IEnumerator HitObject(float waitboi ,float howlongaxe)
    {
        yield return new WaitForSeconds(waitboi);
        int i = 0;
        bool test = false;
        while( i < howlongaxe)
        {
            
          if(Physics.SphereCast(transform.position, SphereRadius, transform.up * 2, out RaycastHit hit , 10) && test == false)
          {
                Debug.Log(hit);
                  hitsound.Play();
                test = true;
          }
            i++;
            yield return new WaitForFixedUpdate();
        }
        
    }


    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.up * 2);
        Gizmos.DrawSphere(transform.position + transform.up  * 2 , SphereRadius);
    }
}


