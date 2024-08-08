using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
	public float baseValue;
	public List<float> fixedModifiers = new List<float>();
	public List<float> persentedModifiers = new List<float>();

	public float GetValue() //Get Added All Modifier Value
	{
		float finalValue = baseValue;
		for (int i = 0; i < fixedModifiers.Count; i++)
		{
			finalValue += fixedModifiers[i];
		}
		for (int i = 0; i < persentedModifiers.Count; i++)
		{
			finalValue *= 1 + (persentedModifiers[i] * 0.01f);
		}
		return finalValue;
	}

	public float SetBaseValue(float changeValue) //Set None Buff Stat value
	{
		baseValue = changeValue;
		return baseValue;
	}

	public void AddModifier(float value, bool IsPersent = false) //Add Stat Modifier Values
	{
		if (value != 0)
		{
			if (IsPersent) { fixedModifiers.Add(value); }
			else { persentedModifiers.Add(value); }
			Debug.Log(GetValue());
		}
	}

	public void RemoveModifier(float value, bool IsPersent = false) //Modifiers Remoce to value in list
	{
		if (value != 0)
		{
			if (IsPersent) { fixedModifiers.Remove(value); }
			else { persentedModifiers.Remove(value); }
		}
	}
}
