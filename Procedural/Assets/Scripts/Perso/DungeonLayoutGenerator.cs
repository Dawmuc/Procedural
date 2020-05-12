using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public enum ExitEnum
{
	Up = 0,
	Down,
	Left,
	Right
}

public class DungeonLayoutGenerator : MonoBehaviour
{
	[SerializeField] private string scriptableDirectoryPath = "Assets/Scripts/Perso/ScriptableRoom";
	[SerializeField] private int nbOfRooms = 5;
	private ExitEnum[] possibleExits;
	private GameObject[] possiblesRooms;


	List<Node> nodes;
	List<Connection> connections;

	private void Awake()
	{
		// retrieve all possible rooms
		//string[] fileNames = Directory.GetFiles(scriptableDirectoryPath).Where(path => !path.EndsWith(".meta")).ToArray();
		//possiblesRooms = new GameObject[fileNames.Length];
		//for (int i = 0; i < fileNames.Length; i++) { possiblesRooms[i] = (GameObject)AssetDatabase.LoadAssetAtPath(fileNames[i], typeof(GameObject)); }

		possibleExits = System.Enum.GetValues(typeof(ExitEnum)).Cast<ExitEnum>().ToArray();

		nodes = InitListOfRoom();
		connections = new List<Connection>();

		nodes = GenerateListOfNode(nodes, nbOfRooms);

		Debug.Log("Fini");
	}

	private List<Node> InitListOfRoom()
	{
		Node originNode = new Node();
		originNode.difficulty = 0;
		originNode.position = Vector2Int.zero;
		originNode.exits.Add(possibleExits[Random.Range(0, possibleExits.Length)]);

		return new List<Node>() { originNode };
	}

	private List<Node> GenerateListOfNode(List<Node> nodes, int nbOfRooms)
	{
		List<Node> nl = new List<Node>(nodes);

		for (int i = 0; i < nbOfRooms - 1; i++)
		{
			List<ExitEnum> pathToRemove = new List<ExitEnum>();
			Vector2Int pos = Vector2Int.zero;

			if (nl[i].exits.Contains(ExitEnum.Up))
			{
				pathToRemove.Add(ExitEnum.Down);
				pos = new Vector2Int(nl[i].position.x, nl[i].position.y + 1);
			}
			if (nl[i].exits.Contains(ExitEnum.Down))
			{
				pathToRemove.Add(ExitEnum.Up);
				pos = new Vector2Int(nl[i].position.x, nl[i].position.y - 1);
			}
			if (nl[i].exits.Contains(ExitEnum.Left))
			{
				pathToRemove.Add(ExitEnum.Right);
				pos = new Vector2Int(nl[i].position.x - 1, nl[i].position.y);
			}
			if (nl[i].exits.Contains(ExitEnum.Right))
			{
				pathToRemove.Add(ExitEnum.Left);
				pos = new Vector2Int(nl[i].position.x + 1, nl[i].position.y);
			}

			ExitEnum[] pe = possibleExits.Except(pathToRemove).ToArray();

			Node n = new Node();
			n.position = pos;
			n.difficulty = 0;
			n.exits.Add(pe[Random.Range(0, pe.Length)]);

			nl.Add(n);

			connections.Add(new Connection(new Node[] { nl[i], nl[i + 1] }));
		}

		return nl;
	}
}