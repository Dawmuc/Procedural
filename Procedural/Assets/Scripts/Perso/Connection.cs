using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection
{
	public bool hasDoor { get; set; }
	public Node[] linkedNodes;

	public Connection(Node[] _linkedNodes, bool _hasDoor = false)
	{
		hasDoor = _hasDoor;
		linkedNodes = _linkedNodes;
	}
}