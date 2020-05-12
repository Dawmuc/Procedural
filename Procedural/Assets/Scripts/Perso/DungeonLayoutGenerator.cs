using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;

public enum ExitEnum
{
	Up,
	Down,
	Left,
	Right
}

public class DungeonLayoutGenerator : MonoBehaviour
{
	[SerializeField] private string scriptableDirectoryPath = "Assets/Scripts/Perso/ScriptableRoom";
	[SerializeField] private int layoutSize = 11;
	private RoomClass[] possiblesRooms;
	private RoomClass[,] rooms;

	private void Awake()
	{
		// retrieve all possible rooms
		string[] fileNames = Directory.GetFiles(scriptableDirectoryPath).Where(path => !path.EndsWith(".meta")).ToArray();
		possiblesRooms = new RoomClass[fileNames.Length];
		for (int i = 0; i < fileNames.Length; i++) { possiblesRooms[i] = new RoomClass((RoomScriptable)AssetDatabase.LoadAssetAtPath(fileNames[i], typeof(RoomScriptable))); }

		// 
		if (layoutSize % 2 == 0) layoutSize++;
		rooms = new RoomClass[layoutSize, layoutSize];
	}
}

public class RoomClass
{
	public ExitEnum[] exits;
	public int difficulty;

	public RoomClass(RoomScriptable rs)
	{
		exits = rs.exits;
		difficulty = rs.difficulty;
	}
}