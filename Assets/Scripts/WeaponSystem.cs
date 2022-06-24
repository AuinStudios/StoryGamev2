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
	private float cooldown = 0;
	// Update is called once per frame
	private void Update()
	{
		// cooldown
		cooldown = cooldown > 0 ? cooldown -= Time.deltaTime : cooldown = 0;


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
			TimeUntllCharge = 0;
			ChargeDamage = 0;
			if(TimeUntllCharge > 0.1f)
            {

            }
            else
            {
				cooldown = 1.3f;
				WeaponAnim.SetTrigger("Swing");
            }
        }
	}
}
