using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	[SerializeField] private int pointNum = 20;
	private Vector2Int[] points;

	private void Awake()
	{
		points = new Vector2Int[pointNum];
		points[0] = new Vector2Int(0, 0);

		GenerateOther(points[0]); 
	}

	private void GenerateOther(Vector2Int o)
	{
		List<Vector2Int> lp = new List<Vector2Int>(points);
	}
}