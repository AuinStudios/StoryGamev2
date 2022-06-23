// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this WeaponSystem do?
/// </summary>
public sealed class WeaponSystem : MonoBehaviour
{
	[SerializeField]
	private Animator WeaponAnim;
	[Header("WeaponSwapValues")]
	[HideInInspector]
	public ItemsScriptableobject[] Items;
	[HideInInspector]
	public int Index = 0;
	private float ChargeDamage;
	private float TimeUntllCharge;

	// Update is called once per frame
	private void Update()
	{
	 if (Input.GetMouseButton(0) && Items[Index] != null && Items[Index].CanAttack == true )
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
			
			
			//TimeUntllCharge = TimeUntllCharge < Items[Index].MaxDamage / 100 ? TimeUntllCharge += Time.deltaTime * Items[Index].MaxDamage / 50 : 
		//	ChargeDamage = ChargeDamage < Items[Index].MaxDamage ? ChargeDamage += Items[Index].MaxDamage / 100 :;
		}
		else if (Input.GetMouseButtonUp(0) && Items[Index] != null && Items[Index].CanAttack == true)
        {
			TimeUntllCharge = 0;
			ChargeDamage = 0;
			if(TimeUntllCharge > 0.1f)
            {

            }
            else
            {
				WeaponAnim.SetBool("Swing", true);
				//Debug.Log(Items[Index].NormalDamage);
            }
        }
	}
}