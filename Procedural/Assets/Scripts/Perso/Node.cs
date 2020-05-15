using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
	public Vector2Int position { get; set; }
	public List<ExitEnum> exits { get; set; }

	public Node() { exits = new List<ExitEnum>(); }
}