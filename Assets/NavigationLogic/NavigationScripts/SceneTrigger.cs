using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrigger : MonoBehaviour {

    public GameObject loadPrefab;
    public SceneScript myScene;
    public int startPointId = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            myScene.SetScene(loadPrefab, startPointId);
        }
    }
}
