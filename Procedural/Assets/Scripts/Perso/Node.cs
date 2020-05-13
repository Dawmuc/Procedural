using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
	public Vector2Int position { get; set; }
	public List<ExitEnum> exits { get; set; }
	public int difficulty { get; set; }

	public Node() { exits = new List<ExitEnum>(); }
}