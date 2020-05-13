using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEditor;

public enum ExitEnum
{
	Up = 0,
	Down,
	Left,
	Right
}

public class DungeonLayoutGenerator : MonoBehaviour
{
	[SerializeField] private string scriptableDirectoryPath = "Assets/Prefabs/Rooms";
	[SerializeField] private int nbOfRooms = 5;
	[SerializeField] private Vector2Int roomSize = new Vector2Int(11, 9);
	private GameObject[] possiblesRooms;
	private ExitEnum[] possibleExits;

    List<Node> nodes;
    List<Connection> connections;

    private void Awake()
    {
		// retrieve all possible rooms
		string[] fileNames = Directory.GetFiles(scriptableDirectoryPath).Where(path => !path.EndsWith(".meta")).ToArray();
		possiblesRooms = new GameObject[fileNames.Length];
		for (int i = 0; i < fileNames.Length; i++) { possiblesRooms[i] = (GameObject)AssetDatabase.LoadAssetAtPath(fileNames[i], typeof(GameObject)); }

		possibleExits = System.Enum.GetValues(typeof(ExitEnum)).Cast<ExitEnum>().ToArray();

		while (nodes == null)
		{
			connections = new List<Connection>();
			nodes = GenerateListOfNode(InitListOfRoom(), nbOfRooms);
		}

		foreach (Node n in nodes) { GenerateRandomRoom(n.position * roomSize); }

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
			// info on current node
			Vector2Int pos = new Vector2Int(nl[i].position.x, nl[i].position.y);

			// place current node + 1
			if (nl[i].exits.Contains(ExitEnum.Up))
				pos = new Vector2Int(pos.x, pos.y + 1);
			if (nl[i].exits.Contains(ExitEnum.Down))
				pos = new Vector2Int(pos.x, pos.y - 1);
			if (nl[i].exits.Contains(ExitEnum.Left))
				pos = new Vector2Int(pos.x - 1, pos.y);
			if (nl[i].exits.Contains(ExitEnum.Right))
				pos = new Vector2Int(pos.x + 1, pos.y);

			// prevent overlaping between nodes
			List<ExitEnum> pathToRemove = new List<ExitEnum>();
			if (CheckIfNodeListContainPos(nl, new Vector2Int(pos.x, pos.y + 1))) // up
				pathToRemove.Add(ExitEnum.Up);
			if (CheckIfNodeListContainPos(nl, new Vector2Int(pos.x, pos.y - 1))) // down
				pathToRemove.Add(ExitEnum.Down);
			if (CheckIfNodeListContainPos(nl, new Vector2Int(pos.x + 1, pos.y))) // right
				pathToRemove.Add(ExitEnum.Right);
			if (CheckIfNodeListContainPos(nl, new Vector2Int(pos.x - 1, pos.y))) // left
				pathToRemove.Add(ExitEnum.Left);

			// retrieve possible exit for new node
			ExitEnum[] pe = possibleExits.Except(pathToRemove).ToArray();

			// restart if path stucked
			if (pe.Length == 0)
			{
				Debug.LogError("Retry");
				return null;
			}

			// creqte new node
			Node n = new Node();
			n.position = pos;
			n.difficulty = 0;
			n.exits.Add(pe[Random.Range(0, pe.Length)]);
			
			nl.Add(n);
			
			connections.Add(new Connection(new Node[] { nl[i], nl[i + 1] }));
		}
		
		return nl;
    }

	private bool CheckIfNodeListContainPos(List<Node> nl, Vector2Int pos)
	{
		if (nl.Where(node => node.position == pos).ToArray().Length > 0)
			return true;
		else
			return false;
	}

	private void GenerateRandomRoom(Vector2 pos)
	{
		Instantiate(possiblesRooms[Random.Range(0, possiblesRooms.Length)], pos, Quaternion.identity);
	}

    void OnDrawGizmosSelected()
    {
        int nb = 0;
		if (nodes == null)
			return;

        foreach (Node item in nodes)
        {
            Gizmos.color = Color.white;
            Vector3 itemPos = new Vector3(item.position.x, item.position.y, 0) * (Vector2)roomSize;
            Gizmos.DrawWireSphere(itemPos, 0.3f);
            Handles.Label(itemPos, nb.ToString());
            nb++;
        }
        
        foreach  (Connection conect in connections)
        {
            Gizmos.color = Color.white;
            Vector3 conectPosOrigin = new Vector3(conect.linkedNodes[0].position.x, conect.linkedNodes[0].position.y, 0) * (Vector2)roomSize;
            Vector3 conectPosDestination = new Vector3(conect.linkedNodes[1].position.x, conect.linkedNodes[1].position.y, 0) * (Vector2)roomSize;
            Gizmos.DrawLine(conectPosOrigin, conectPosDestination);
        }
        
    }

    private List<Connection> GenerateListOfConnection(List<Node> nodes)
    {

        return connections;
    }
}