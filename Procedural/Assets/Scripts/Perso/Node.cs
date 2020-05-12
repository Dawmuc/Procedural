using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
	public Vector2Int position;
	public List<ExitEnum> exits;
	public int difficulty;

	public Node()
	{
		exits = new List<ExitEnum>();
	}
}