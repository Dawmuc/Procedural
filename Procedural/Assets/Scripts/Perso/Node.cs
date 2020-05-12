using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
	public ExitEnum[] exits;
	public int difficulty;

	public Node(RoomScriptable rs)
	{
		exits = rs.exits;
		difficulty = rs.difficulty;
	}
}