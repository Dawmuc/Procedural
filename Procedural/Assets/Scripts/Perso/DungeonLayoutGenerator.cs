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
	private Node[] possiblesRooms;
	private Node[,] rooms;

	private void Awake()
	{
		// retrieve all possible rooms
		string[] fileNames = Directory.GetFiles(scriptableDirectoryPath).Where(path => !path.EndsWith(".meta")).ToArray();
		possiblesRooms = new Node[fileNames.Length];
		for (int i = 0; i < fileNames.Length; i++) { possiblesRooms[i] = new Node((RoomScriptable)AssetDatabase.LoadAssetAtPath(fileNames[i], typeof(RoomScriptable))); }

		// LayoutSize
		if (layoutSize % 2 == 0) layoutSize++;
		rooms = new Node[layoutSize, layoutSize];
	}
}