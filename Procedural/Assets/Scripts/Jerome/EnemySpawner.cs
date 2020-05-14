using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyList;

    private GameObject player;
    private float distanceToPlayer;
    private bool canSpawn = true;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
       this.gameObject.GetComponent<SpriteRenderer>().color = Color.white; 
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(player.transform.position, this.transform.position);

        if (distanceToPlayer <= 10 && canSpawn)
        {
            SpawnEnnemy();
        }
    }

    private void SpawnEnnemy()
    {
        canSpawn = false;
        int i = Random.Range(0, enemyList.Length - 1);
        GameObject enemy = Instantiate(enemyList[i]);
        enemy.transform.SetParent(null);
        enemy.transform.position = new Vector3(this.gameObject.transform.position.x,this.gameObject.transform.position.y,-2.5f);
        enemy.transform.rotation = Quaternion.Euler(0,0,180);
    }
}
