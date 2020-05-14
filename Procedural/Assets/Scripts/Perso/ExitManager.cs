using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExitManager : MonoBehaviour
{
	public Door upDoor { get; set; }
	public Door downDoor { get; set; }
	public Door leftDoor { get; set; }
	public Door rightDoor { get; set; }

	private void Awake()
	{
		Door[] doors = GetComponentsInChildren<Door>();
		foreach (Door d in doors) { d.SetState(Door.STATE.WALL); }
		float t = 0;

		t = doors.Max(b => b.transform.position.y);
		upDoor = doors.Where(d => d.transform.position.y == t).ToArray()[0];

		t = doors.Min(b => b.transform.position.y);
		downDoor = doors.Where(d => d.transform.position.y == t).ToArray()[0];

		t = doors.Min(b => b.transform.position.x);
		leftDoor = doors.Where(d => d.transform.position.x == t).ToArray()[0];

		t = doors.Max(b => b.transform.position.x);
		rightDoor = doors.Where(d => d.transform.position.x == t).ToArray()[0];
	}

	public void SetExits(List<ExitEnum> le)
	{
		if (le.Contains(ExitEnum.Up))
			upDoor.SetState(Door.STATE.OPEN);
		if (le.Contains(ExitEnum.Down))
			downDoor.SetState(Door.STATE.OPEN);
		if (le.Contains(ExitEnum.Left))
			leftDoor.SetState(Door.STATE.OPEN);
		if (le.Contains(ExitEnum.Right))
			rightDoor.SetState(Door.STATE.OPEN);
	}
}
