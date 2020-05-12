using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{
	public bool hasDoor { get; set; }

	public Connection(bool _hasDoor = false)
	{
		hasDoor = _hasDoor;
	}
}