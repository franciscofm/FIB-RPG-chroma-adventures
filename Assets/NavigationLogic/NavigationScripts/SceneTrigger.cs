using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrigger : MonoBehaviour {

    public GameObject loadPrefab;
    public int startPointId = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            GameObject lp = Instantiate(loadPrefab, loadPrefab.transform.position, loadPrefab.transform.rotation);
            lp.name = loadPrefab.name;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Destroy(transform.parent.parent.gameObject);
            player.transform.position = lp.transform.GetChild(0).GetChild(startPointId).position;
            lp.SetActive(true);
        }
    }
}
