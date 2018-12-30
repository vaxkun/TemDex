using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TypeController : MonoBehaviour {

	#region Singleton
	public static TypeController instance;
	private void Awake()
	{
		instance = this;
	}
	#endregion
	public List<TemType> GiveWeakType(TemInfo input)
	{
		List<TemType> r = new List<TemType>();
		byte length = (byte)input.Type.Length;
		for (int i = 0; i < length; i++)
		{
			switch (input.Type[i])
			{
				case TemType.Crystal:
					r.Add(TemType.Melee);
					r.Add(TemType.Fire);
					break;
				case TemType.Digital:
					r.Add(TemType.Water);
					r.Add(TemType.Electric);
					break;
				case TemType.Earth:
					r.Add(TemType.Melee);
					r.Add(TemType.Crystal);
					r.Add(TemType.Nature);
					r.Add(TemType.Water);
					break;
				case TemType.Electric:
					r.Add(TemType.Melee);
					r.Add(TemType.Earth);
					break;
				case TemType.Fire:
					r.Add(TemType.Water);
					r.Add(TemType.Earth);
					break;
				case TemType.Melee:
					r.Add(TemType.Toxic);
					r.Add(TemType.Mental);
					r.Add(TemType.Crystal);
					r.Add(TemType.Wind);
					break;
				case TemType.Mental:
					r.Add(TemType.Digital);
					break;
				case TemType.Nature:
					r.Add(TemType.Fire);
					r.Add(TemType.Wind);
					break;
				case TemType.Neutral:
					r.Add(TemType.Neutral);
					break;
				case TemType.Toxic:
					r.Add(TemType.Fire);
					r.Add(TemType.Wind);
					break;
				case TemType.Water:
					r.Add(TemType.Nature);
					r.Add(TemType.Electric);
					r.Add(TemType.Toxic);
					break;
				case TemType.Wind:
					r.Add(TemType.Electric);
					r.Add(TemType.Earth);
					break;
			}
		}
		return r;
	}
	public List<TemType> GiveAttack2x(TemInfo input)
	{
		List<TemType> r = new List<TemType>();
		byte length = (byte)input.Type.Length;
		for (int i = 0; i < length; i++)
		{
			switch (input.Type[i])
			{
				case TemType.Crystal:
					r.Add(TemType.Electric);
					r.Add(TemType.Mental);
					break;
				case TemType.Digital:
					r.Add(TemType.Mental);
					r.Add(TemType.Digital);
					r.Add(TemType.Melee);
					break;
				case TemType.Earth:
					r.Add(TemType.Fire);
					r.Add(TemType.Electric);
					r.Add(TemType.Crystal);
					break;
				case TemType.Electric:
					r.Add(TemType.Water);
					r.Add(TemType.Mental);
					r.Add(TemType.Wind);
					r.Add(TemType.Digital);
					break;
				case TemType.Fire:
					r.Add(TemType.Nature);
					r.Add(TemType.Crystal);
					break;
				case TemType.Melee:
					r.Add(TemType.Earth);
					r.Add(TemType.Crystal);
					break;
				case TemType.Mental:
					r.Add(TemType.Neutral);
					r.Add(TemType.Melee);
					break;
				case TemType.Nature:
					r.Add(TemType.Water);
					r.Add(TemType.Earth);
					break;
				case TemType.Neutral:
					break;
				case TemType.Toxic:
					r.Add(TemType.Water);
					r.Add(TemType.Nature);
					break;
				case TemType.Water:
					r.Add(TemType.Fire);
					r.Add(TemType.Earth);
					r.Add(TemType.Digital);
					break;
				case TemType.Wind:
					r.Add(TemType.Earth);
					r.Add(TemType.Toxic);
					break;
			}
		}
		return r;
	}
	public List<TemType> GiveAttack05x(TemInfo input)
	{
		List<TemType> r = new List<TemType>();
		byte length = (byte)input.Type.Length;
		for (int i = 0; i < length; i++)
		{
			switch (input.Type[i])
			{
				case TemType.Crystal:
					r.Add(TemType.Fire);
					r.Add(TemType.Earth);
					break;
				case TemType.Digital:
					break;
				case TemType.Earth:
					r.Add(TemType.Water);
					r.Add(TemType.Nature);
					r.Add(TemType.Wind);
					break;
				case TemType.Electric:
					r.Add(TemType.Nature);
					r.Add(TemType.Electric);
					r.Add(TemType.Earth);
					r.Add(TemType.Crystal);
					break;
				case TemType.Fire:
					r.Add(TemType.Fire);
					r.Add(TemType.Water);
					r.Add(TemType.Earth);
					break;
				case TemType.Melee:
					r.Add(TemType.Mental);
					r.Add(TemType.Melee);
					break;
				case TemType.Mental:
					r.Add(TemType.Crystal);
					break;
				case TemType.Nature:
					r.Add(TemType.Fire);
					r.Add(TemType.Nature);
					r.Add(TemType.Toxic);
					break;
				case TemType.Neutral:
					r.Add(TemType.Mental);
					break;
				case TemType.Toxic:
					r.Add(TemType.Earth);
					r.Add(TemType.Digital);
					r.Add(TemType.Crystal);
					r.Add(TemType.Toxic);
					break;
				case TemType.Water:
					r.Add(TemType.Water);
					r.Add(TemType.Nature);
					r.Add(TemType.Toxic);
					break;
				case TemType.Wind:
					r.Add(TemType.Electric);
					r.Add(TemType.Wind);
					break;
			}
		}
		return r;
	}
	public List<TemType> GiveDefense2x(TemInfo input)
	{
		List<TemType> r = new List<TemType>();
		byte length = (byte)input.Type.Length;
		for (int i = 0; i < length; i++)
		{
			switch (input.Type[i])
			{
				case TemType.Crystal:
					r.Add(TemType.Fire);
					r.Add(TemType.Earth);
					r.Add(TemType.Melee);
					break;
				case TemType.Digital:
					r.Add(TemType.Water);
					r.Add(TemType.Electric);
					r.Add(TemType.Digital);
					break;
				case TemType.Earth:
					r.Add(TemType.Water);
					r.Add(TemType.Nature);
					r.Add(TemType.Wind);
					r.Add(TemType.Melee);
					break;
				case TemType.Electric:
					r.Add(TemType.Earth);
					r.Add(TemType.Crystal);
					break;
				case TemType.Fire:
					r.Add(TemType.Water);
					r.Add(TemType.Earth);
					break;
				case TemType.Melee:
					r.Add(TemType.Mental);
					r.Add(TemType.Digital);
					break;
				case TemType.Mental:
					r.Add(TemType.Electric);
					r.Add(TemType.Digital);
					r.Add(TemType.Crystal);
					break;
				case TemType.Nature:
					r.Add(TemType.Fire);
					r.Add(TemType.Toxic);
					break;
				case TemType.Neutral:
					r.Add(TemType.Mental);
					break;
				case TemType.Toxic:
					r.Add(TemType.Wind);
					break;
				case TemType.Water:
					r.Add(TemType.Nature);
					r.Add(TemType.Electric);
					r.Add(TemType.Toxic);
					break;
				case TemType.Wind:
					r.Add(TemType.Electric);
					break;
			}
		}
		return r;
	}
	public List<TemType> GiveDefense05x(TemInfo input)
	{
		List<TemType> r = new List<TemType>();
		byte length = (byte)input.Type.Length;
		for (int i = 0; i < length; i++)
		{
			switch (input.Type[i])
			{
				case TemType.Crystal:
					r.Add(TemType.Electric);
					r.Add(TemType.Mental);
					r.Add(TemType.Toxic);
					break;
				case TemType.Digital:
					r.Add(TemType.Toxic);
					break;
				case TemType.Earth:
					r.Add(TemType.Fire);
					r.Add(TemType.Electric);
					r.Add(TemType.Crystal);
					r.Add(TemType.Toxic);
					break;
				case TemType.Electric:
					r.Add(TemType.Electric);
					r.Add(TemType.Wind);
					break;
				case TemType.Fire:
					r.Add(TemType.Fire);
					r.Add(TemType.Nature);
					r.Add(TemType.Crystal);
					break;
				case TemType.Melee:
					r.Add(TemType.Melee);
					break;
				case TemType.Mental:
					r.Add(TemType.Neutral);
					r.Add(TemType.Melee);
					break;
				case TemType.Nature:
					r.Add(TemType.Water);
					r.Add(TemType.Nature);
					r.Add(TemType.Electric);
					r.Add(TemType.Earth);
					break;
				case TemType.Neutral:
					break;
				case TemType.Toxic:
					r.Add(TemType.Water);
					r.Add(TemType.Nature);
					r.Add(TemType.Toxic);
					break;
				case TemType.Water:
					r.Add(TemType.Fire);
					r.Add(TemType.Water);
					r.Add(TemType.Earth);
					break;
				case TemType.Wind:
					r.Add(TemType.Earth);
					r.Add(TemType.Wind);
					break;
			}
		}
		return r;
	}
	public List<TemType> GiveStrongTemType(TemInfo input)
	{
		List<TemType> r = new List<TemType>();
		byte length = (byte)input.Type.Length;
		for (int i = 0; i < length; i++)
		{
			switch (input.Type[i])
			{
				case TemType.Crystal:
					r.Add(TemType.Melee);
					r.Add(TemType.Earth);
					break;
				case TemType.Digital:
					r.Add(TemType.Mental);
					break;
				case TemType.Earth:
					r.Add(TemType.Electric);
					r.Add(TemType.Fire);
					r.Add(TemType.Wind);
					break;
				case TemType.Electric:
					r.Add(TemType.Digital);
					r.Add(TemType.Wind);
					r.Add(TemType.Water);
					break;
				case TemType.Fire:
					r.Add(TemType.Nature);
					r.Add(TemType.Crystal);
					r.Add(TemType.Toxic);
					break;
				case TemType.Melee:
					r.Add(TemType.Electric);
					r.Add(TemType.Earth);
					r.Add(TemType.Crystal);
					break;
				case TemType.Mental:
					r.Add(TemType.Melee);
					break;
				case TemType.Nature:
					r.Add(TemType.Water);
					r.Add(TemType.Earth);
					break;
				case TemType.Neutral:
					r.Add(TemType.Neutral);
					break;
				case TemType.Toxic:
					r.Add(TemType.Water);
					r.Add(TemType.Melee);
					break;
				case TemType.Water:
					r.Add(TemType.Fire);
					r.Add(TemType.Digital);
					r.Add(TemType.Earth);
					break;
				case TemType.Wind:
					r.Add(TemType.Toxic);
					r.Add(TemType.Melee);
					r.Add(TemType.Nature);
					break;
			}
		}
		return r;
	}
	public TemType GetTemTypeFromNum(int n)
	{
		TemType r = TemType.Crystal;

		switch (n)
		{
			case 0:
				r = TemType.Crystal;
				break;
			case 1:
				r = TemType.Digital;
				break;
			case 2:
				r = TemType.Earth;
				break;
			case 3:
				r = TemType.Electric;
				break;
			case 4:
				r = TemType.Fire;
				break;
			case 5:
				r = TemType.Melee;
				break;
			case 6:
				r = TemType.Mental;
				break;
			case 7:
				r = TemType.Nature;
				break;
			case 8:
				r = TemType.Neutral;
				break;
			case 9:
				r = TemType.Toxic;
				break;
			case 10:
				r = TemType.Water;
				break;
			case 11:
				r = TemType.Wind;
				break;
		}

		return r;
	}
}
