using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public int damages = 1;
    public float duration = 0.3f;
    public float knockbackSpeed = 3;
    public float knockbackDuration = 0.5f;
	public LayerMask destroyOnHit;
	public bool canAttackDistance;
	public float speed;
	public GameObject target;

	[System.NonSerialized]
    public GameObject owner;

	private void Start()
	{
		target = GameObject.Find("Player");
		transform.LookAt(target.transform.position);
	}

	// Update is called once per frame
	void Update () {
		if (duration <= 0.0f)
			return;

		duration -= Time.deltaTime;
		if (duration <= 0.0f)
		{
			GameObject.Destroy(gameObject);
		}
		
		if(canAttackDistance)
		{
			ShootTarget(target.transform.position);
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if(((1 << collision.gameObject.layer) & destroyOnHit) != 0)
		{
			GameObject.Destroy(gameObject);
		}
	}

	private void ShootTarget(Vector3 targetPos)
	{
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}
}
