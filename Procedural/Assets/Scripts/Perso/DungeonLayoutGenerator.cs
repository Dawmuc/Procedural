using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEditor;
using System.Linq.Expressions;

public enum ExitEnum
{
	Up = 0,
	Down,
	Left,
	Right,
}

public class DungeonLayoutGenerator : MonoBehaviour
{
	[SerializeField] private string[] scriptableDirectoryPath;
	[SerializeField] private int nbOfRooms;
	[SerializeField] private int nbOfRoomInSameDirection = 2;
    [SerializeField] private Vector2Int roomSize = new Vector2Int(11, 9);
	[SerializeField] private int[] otherPathLength;
	[SerializeField] private bool generation;
	private int randomStart;
	private List<GameObject> possiblesRooms;
	private List<ExitEnum> possibleExits;

    private List<Node> nodes;
	List<Connection> tconnections;
	List<Connection> connections;

    private int randomBlock;
    public  GameObject startRoom;
    public  GameObject endRoom;


    private void Awake()
    {
        nbOfRooms = Random.Range(10, 20);
        // retrieve all possible rooms
        if (scriptableDirectoryPath != null)
		{
			List<string> fileNames = new List<string>();
			for(int i = 0; i < scriptableDirectoryPath.Length; i++) { fileNames = AddToList(fileNames, Directory.GetFiles(scriptableDirectoryPath[i]).Where(path => !path.EndsWith(".meta")).ToList()); }
            possiblesRooms = new List<GameObject>();
            for (int y = 0; y < fileNames.Count; y++) { possiblesRooms.Add((GameObject)AssetDatabase.LoadAssetAtPath(fileNames[y], typeof(GameObject))); }
		}
        possiblesRooms.Remove(startRoom);
        possiblesRooms.Remove(endRoom);



        possibleExits = System.Enum.GetValues(typeof(ExitEnum)).Cast<ExitEnum>().ToList();

		// First Path
		nodes = InitListOfRoom();
		while (nodes.Count == 1)
		{
			tconnections = new List<Connection>();
			nodes = GenerateListOfNode(nodes[0], nodes, nbOfRooms);
		}
		connections = tconnections;

		// Other Path
		int it = 0;
		int passDone = 0;
		List<Node> _nodes = new List<Node>();
		while (passDone != otherPathLength.Length)
		{
			it++;
			tconnections = connections;
			_nodes = new List<Node>(nodes);

			for (int i = 0; i < otherPathLength.Length; i++)
			{
				bool canResume = false;
				int it2 = 0;
				while (!canResume)
				{
					it2++;
					int pnc = _nodes.Count;
					_nodes = GenerateListOfNode(nodes[Random.Range(0, nodes.Count - 1)], _nodes, otherPathLength[i] + 1);
					if (pnc < _nodes.Count)
					{
						canResume = true;
						passDone++;
					}
					if (it2 >= 10)
					{
						Debug.LogError("restart hallway");
						break;
					}
				}
			}

			if (it >= 30)
			{
				Debug.LogError("abort generation");
				break;
			}
		}
		nodes = _nodes;
		connections = tconnections;

    }
	private void Start()
	{
        if (generation)
        {
            randomBlock = Random.Range(1, 10);
            int consec = 0;

            for (int i = 0; i < nbOfRooms; i++)
            {
                if (i == 0)
                {
                    GenerateStart(nodes[i]);
                }
                else if (i == nbOfRooms - 1)
                {
                    GenerateEnd(nodes[i]);
                }
                else
                {
                    GenerateRooms(nodes[i]);
                    /*
                    if (consec > 3 && consec >= randomBlock)
                    {
                        if(nodes[i-1].exits.Contains(ExitEnum.Up))
                            nodes[i].exits.Remove(ExitEnum.Up);
                        if (nodes[i - 1].exits.Contains(ExitEnum.Down))
                            nodes[i].exits.Remove(ExitEnum.Down);
                        if (nodes[i - 1].exits.Contains(ExitEnum.Right))
                            nodes[i].exits.Remove(ExitEnum.Right);
                        if (nodes[i - 1].exits.Contains(ExitEnum.Left))
                            nodes[i].exits.Remove(ExitEnum.Left);

                        GenerateRooms(nodes[i]);
                        
                        consec = 0;
                    }
                    else
                    {
                        GenerateRooms(nodes[i]);
                    }
                    consec++;
                    
                }
                */
                }
            }
            for (int i = nbOfRooms ; i < nodes.Count; i++)
            {
                GenerateRooms(nodes[i]);
                
            }
            
        }
		Debug.Log("Fini");
	}

	private List<Node> InitListOfRoom()
    {
        Node originNode = new Node();
        originNode.difficulty = 0;
        originNode.position = Vector2Int.zero;

        return new List<Node>() { originNode };
    }

    private List<Node> GenerateListOfNode(Node o, List<Node> nodeList, int nbOfRooms, bool Main = false)
    {
		List<Node> nln = new List<Node>() { o };
		List<Node> usedPos = new List<Node>(nodeList);
		List<Connection> nlc = new List<Connection>(tconnections); 
		int _nbOfRoomInSameDirection = nbOfRoomInSameDirection;

		for (int i = 0; i < nbOfRooms - 1; i++)
		{
			// info on current node
			Vector2Int pos = new Vector2Int(nln[i].position.x, nln[i].position.y);

			// place current node + 1
			ExitEnum dir = ExitEnum.Down;

			// Set or Read nln[i] exit direction
			if (i == 0)
			{
				List<ExitEnum> pp = new List<ExitEnum>(possibleExits);

				if (o.exits.Contains(ExitEnum.Up) || CheckIfNodeListContainPos(usedPos, new Vector2Int(pos.x, pos.y + 1)))
					pp.Remove(ExitEnum.Up);
				if (o.exits.Contains(ExitEnum.Down) || CheckIfNodeListContainPos(usedPos, new Vector2Int(pos.x, pos.y - 1)))
					pp.Remove(ExitEnum.Down);
				if (o.exits.Contains(ExitEnum.Left) || CheckIfNodeListContainPos(usedPos, new Vector2Int(pos.x - 1, pos.y)))
					pp.Remove(ExitEnum.Left);
				if (o.exits.Contains(ExitEnum.Right) || CheckIfNodeListContainPos(usedPos, new Vector2Int(pos.x + 1, pos.y)))
					pp.Remove(ExitEnum.Right);

				if (pp.Count == 0)
				{
					Debug.LogError("no path available from origin");
					return nodeList;
				}
				dir = pp[Random.Range(0, pp.Count)];
				o.exits.Add(dir);
			}
			else
			{
				if (nln[i].exits.Contains(ExitEnum.Up) && !CheckIfNodeListContainPos(usedPos, new Vector2Int(pos.x, pos.y + 1)))
					dir = ExitEnum.Up;
				if (nln[i].exits.Contains(ExitEnum.Down) && !CheckIfNodeListContainPos(usedPos, new Vector2Int(pos.x, pos.y - 1)))
					dir = ExitEnum.Down;
				if (nln[i].exits.Contains(ExitEnum.Left) && !CheckIfNodeListContainPos(usedPos, new Vector2Int(pos.x - 1, pos.y)))
					dir = ExitEnum.Left;
				if (nln[i].exits.Contains(ExitEnum.Right) && !CheckIfNodeListContainPos(usedPos, new Vector2Int(pos.x + 1, pos.y)))
					dir = ExitEnum.Right;
			}

			// Place the new node
			ExitEnum exitToAddToNewNode = ExitEnum.Down;
			switch (dir)
			{
				case ExitEnum.Up:
					pos = new Vector2Int(pos.x, pos.y + 1);
					exitToAddToNewNode = ExitEnum.Down;
					break;
				case ExitEnum.Down:
					pos = new Vector2Int(pos.x, pos.y - 1);
					exitToAddToNewNode = ExitEnum.Up;
					break;
				case ExitEnum.Left:
					pos = new Vector2Int(pos.x - 1, pos.y);
					exitToAddToNewNode = ExitEnum.Right;
					break;
				case ExitEnum.Right:
					pos = new Vector2Int(pos.x + 1, pos.y);
					exitToAddToNewNode = ExitEnum.Left;
					break;
			}

			// Select Exit Direction of the new node
			List<ExitEnum> pathToRemove = new List<ExitEnum>();
			ExitEnum[] pe = new ExitEnum[0];
			bool isGoingForward = false;
			if (_nbOfRoomInSameDirection != 0 && i != 0)
			{
				isGoingForward = true;
				switch (dir)
				{
					case ExitEnum.Up:
						if (CheckIfNodeListContainPos(usedPos, new Vector2Int(pos.x, pos.y + 1)))
							isGoingForward = false;
						break;
					case ExitEnum.Down:
						if (CheckIfNodeListContainPos(usedPos, new Vector2Int(pos.x, pos.y - 1)))
							isGoingForward = false;
						break;
					case ExitEnum.Left:
						if (CheckIfNodeListContainPos(usedPos, new Vector2Int(pos.x - 1, pos.y)))
							isGoingForward = false;
						break;
					case ExitEnum.Right:
						if (CheckIfNodeListContainPos(usedPos, new Vector2Int(pos.x + 1, pos.y)))
							isGoingForward = false;
						break;
				}

				if (isGoingForward)
				{
					pe = new ExitEnum[] { dir };
					_nbOfRoomInSameDirection -= 1;
				}
			}
			if(!isGoingForward)
			{
				// prevent overlaping between nodes
				if (CheckIfNodeListContainPos(usedPos, new Vector2Int(pos.x, pos.y + 1))) // up
					pathToRemove.Add(ExitEnum.Up);
				if (CheckIfNodeListContainPos(usedPos, new Vector2Int(pos.x, pos.y - 1))) // down
					pathToRemove.Add(ExitEnum.Down);
				if (CheckIfNodeListContainPos(usedPos, new Vector2Int(pos.x + 1, pos.y))) // right
					pathToRemove.Add(ExitEnum.Right);
				if (CheckIfNodeListContainPos(usedPos, new Vector2Int(pos.x - 1, pos.y))) // left
					pathToRemove.Add(ExitEnum.Left);

				pathToRemove.Add(dir);

				// retrieve possible exit for new node
				pe = possibleExits.Except(pathToRemove).ToArray();

				// restart if path stucked
				if (pe.Length == 0)
				{
					Debug.LogError("Hallway cannot continue");
					return nodeList;
				}

				_nbOfRoomInSameDirection = Random.Range(1, nbOfRoomInSameDirection + 1);
			}

			// create new node
			Node n = new Node();
			n.position = pos;
			n.difficulty = 0;
			n.exits.Add(exitToAddToNewNode);

			if (i != nbOfRooms - 2)
				n.exits.Add(pe[Random.Range(0, pe.Length)]);
			

			nln.Add(n);
			usedPos.Add(n);
			
			nlc.Add(new Connection(new Node[] { nln[i], nln[i + 1] }));

		}

		nln.Remove(o);
		tconnections = AddToList(tconnections, nlc.Except(tconnections).ToList());
		return AddToList(nodeList, nln);
    }

	private bool CheckIfNodeListContainPos(List<Node> nl, Vector2Int pos)
	{
		if (nl.Where(node => node.position == pos).ToArray().Length > 0)
			return true;
		else
			return false;
	}

	private void GenerateRooms(Node n)
	{
		ExitManager em = Instantiate(possiblesRooms[Random.Range(0, possiblesRooms.Count)], (Vector2)(n.position * roomSize), Quaternion.identity).GetComponent<ExitManager>();
        em.SetExits(n.exits);
	}

    private void GenerateStart(Node n)
    {
        ExitManager em = Instantiate(startRoom, (Vector2)(n.position * roomSize), Quaternion.identity).GetComponent<ExitManager>();
        em.SetExits(n.exits);
    }

    private void GenerateEnd(Node n)
    {
        ExitManager em = Instantiate(endRoom, (Vector2)(n.position * roomSize), Quaternion.identity).GetComponent<ExitManager>();
        em.SetExits(n.exits);
    }

    private List<string> AddToList(List<string> ls, List<string> nls)
	{
		List<string> t = new List<string>();

		foreach (string s in ls)
		{
			t.Add(s);
		}
		foreach (string s in nls)
		{
			t.Add(s);
		}

		return t;
	}
	private List<Node> AddToList(List<Node> ln, List<Node> nln)
	{
		List<Node> t = new List<Node>();

		foreach (Node s in ln)
		{
			t.Add(s);
		}
		foreach (Node s in nln)
		{
			t.Add(s);
		}

		return t;
	}
	private List<Connection> AddToList(List<Connection> lc, List<Connection> nlc)
	{
		List<Connection> t = new List<Connection>();

		foreach (Connection s in lc)
		{
			t.Add(s);
		}
		foreach (Connection c in nlc)
		{
			t.Add(c);
		}

		return t;
	}
	private List<ExitEnum> AddToList(List<ExitEnum> el, List<ExitEnum> nel)
	{
		List<ExitEnum> t = new List<ExitEnum>();

		foreach (ExitEnum s in el)
		{
			t.Add(s);
		}
		foreach (ExitEnum c in nel)
		{
			t.Add(c);
		}

		return t;
	}

	void OnDrawGizmosSelected()
    {
		if (nodes == null )
			return;
		
		Gizmos.color = Color.white;

		for (int i = 0; i < nodes.Count; i++)
		{
			Vector3 itemPos = new Vector3(nodes[i].position.x, nodes[i].position.y, 0) * (Vector2)roomSize;
			Gizmos.DrawWireSphere(itemPos, 0.3f);
			Handles.Label(itemPos, (i+1).ToString());
		}

		if (connections == null)
			return;

		foreach (Connection conect in connections)
		{
			Vector3 conectPosOrigin = new Vector3(conect.linkedNodes[0].position.x, conect.linkedNodes[0].position.y, 0) * (Vector2)roomSize;
			Vector3 conectPosDestination = new Vector3(conect.linkedNodes[1].position.x, conect.linkedNodes[1].position.y, 0) * (Vector2)roomSize;
			Gizmos.DrawLine(conectPosOrigin, conectPosDestination);
		}
	}

}