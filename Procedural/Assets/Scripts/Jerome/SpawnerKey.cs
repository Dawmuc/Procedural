using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerKey : MonoBehaviour
{
    public bool doGenerateKey;
    public GameObject Key;

    private void Start()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
    }

    private void Update()
    {
        if (doGenerateKey)
        {
            GenerateKey();
        }
    }

    public void GenerateKey()
    {
        GameObject key = Instantiate(Key, this.gameObject.transform);
        key.transform.SetParent(null);
        key.transform.position = this.gameObject.transform.position;
        key.transform.localScale = new Vector3(1,1,1);
        doGenerateKey = false;
        this.gameObject.SetActive(false);
    }
}
