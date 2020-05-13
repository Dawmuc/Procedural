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
		if (test == true)
		{
			Debug.Log("actif");
			StartCoroutine(Start());
		}
		if (test == false)
		{
			Debug.Log(" pas actif");

			StartCoroutine(Start());
		}
	}
	



	public IEnumerator Start()
	{
		
		//yield return new WaitForSeconds(Timer);
		
		if (test == true)
		{
			yield return new WaitForSeconds(Timer);
			spike_Gameobject.SetActive(false);
			test = false;
			
			yield return null;
		}

		if (test == false)
		{
			yield return new WaitForSeconds(Timer);
			spike_Gameobject.SetActive(true);
			test = true;
			yield return null;
		}
		
		/*
					if
					//Turn My game object that is set to false(off) to True(on).
					this.gameObject.SetActive(true);

					//Turn the Game Oject back off after 1 sec.
					yield return new WaitForSeconds(3);

					//Game object will turn off
					this.gameObject.SetActive(false);*/
	}
	/*
        if (test ==true)
        {
			this.gameObject.SetActive(true);
        }
			if (test == false)
		{
			this.gameObject.SetActive(false);
		}*/
}