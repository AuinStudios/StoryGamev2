// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this WeaponSystem do?
/// </summary>
public sealed class WeaponSystem : MonoBehaviour
{
	[Header("Recoil")]
	private Vector3 pos1;
	private Vector3 pos2;
	//[SerializeField]
	//private float RecoilX = -3;
	//[SerializeField]
	//private float RecoilY = -3;
	//[SerializeField]
	//private float RecoilZ = -3;
	[SerializeField]
    private Vector2 lerptowardsrecoil;

	[SerializeField]
	private float returnspeed = 6;
	[SerializeField]
	private float snapiness = 6;
	[SerializeField]
	private Transform head;
	[Header("Anim Stuff")]
	[SerializeField]
	private Animator WeaponAnim;
	[Header("WeaponSwapValues")]
	[HideInInspector]
	public ItemsScriptableobject[] Items;
	[HideInInspector]
	public int Index = 0;
	private float ChargeDamage;
	private float TimeUntllCharge;
	private float cooldown = 0;
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
		if (Input.GetMouseButton(0) && Items[Index] != null && Items[Index].CanAttack == true && cooldown <= 0 )
        {
			if(TimeUntllCharge <= Items[Index].MaxDamage)
            {
				TimeUntllCharge += 0.35f * Time.deltaTime ;
				
			}
			if (TimeUntllCharge > 0.1f)
            {
               ChargeDamage += Items[Index].MaxDamage / 2 * Time.deltaTime;
			   Debug.Log(ChargeDamage);
			}
		}
		else if (Input.GetMouseButtonUp(0) && Items[Index] != null && Items[Index].CanAttack == true && cooldown <= 0)
        {
			//TimeUntllCharge = 0;
			//ChargeDamage = 0;
			if(TimeUntllCharge > 0.1f)
            {
				StartCoroutine(recoil());
            }
            else
            {
				cooldown = 1.3f;
				WeaponAnim.SetTrigger("Swing");
            }
        }
	}

	private IEnumerator recoil()
    {
		//pos1 = new Vector3(0, 0, 0);
		//down = Quaternion.Euler(10, -20, 0);
		
		int i = 0;
		while(i  < 30)
        {
			pos1 = Vector3.Lerp(pos1, Vector3.zero, returnspeed * Time.deltaTime);
			pos2 = Vector3.Slerp(pos2, pos1, snapiness * Time.fixedDeltaTime);
			head.localRotation = Quaternion.Euler(pos1);
			pos1 += new Vector3(lerptowardsrecoil.x , lerptowardsrecoil.y , 0);
			i++;
			yield return new WaitForFixedUpdate();
        }
		i = 0;
		while (i < 60)
		{
			pos1 = Vector3.Lerp(pos1, Vector3.zero, returnspeed * Time.deltaTime);
			pos2 = Vector3.Slerp(pos2, pos1, snapiness * Time.fixedDeltaTime);
			head.localRotation = Quaternion.Euler(pos1);
			pos1 += new Vector3(0, 0, 0);

			i++;
			yield return new WaitForFixedUpdate();
		}
		head.localRotation = Quaternion.Euler(0, 0, 0);
	}
}


