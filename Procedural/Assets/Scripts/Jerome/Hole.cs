using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public GameObject player;

    public int degats;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Player.Instance == null)
            return;
        if(collision.attachedRigidbody.gameObject != Player.Instance.gameObject)
            return;

        Player.Instance.ApplyHit(null);
    }
}