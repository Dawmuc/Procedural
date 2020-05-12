using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RoomScriptable : ScriptableObject
{
	public ExitEnum[] exits;
	public int difficulty;
}