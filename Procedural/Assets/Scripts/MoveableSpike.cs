using Packages.Rider.Editor.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


public class MoveableSpike : MonoBehaviour
{
	public GameObject spike_Gameobject;
	public int Timer;
	public bool test = true;
	private float time = 0;

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (Player.Instance == null)
			return;
		if (collision.attachedRigidbody.gameObject != Player.Instance.gameObject)
			return;

		Player.Instance.ApplyHit(null);
	}

	//Update, start coroutine

	private void Update()
	{
		time = time + Time.deltaTime;

		if (time > Timer)
		{
			time = time - Timer;
			if (test == true)
			{
				spike_Gameobject.SetActive(false);
				test = false;

			}

			else
			{

				spike_Gameobject.SetActive(true);
				test = true;

			}
		}

	}

}