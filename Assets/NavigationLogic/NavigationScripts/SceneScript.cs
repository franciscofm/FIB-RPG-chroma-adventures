using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneScript : MonoBehaviour {

    public GameController gc;
	public void SetScene(GameObject loadPrefab, int startPointId)
    {
        GameObject lp = Instantiate(loadPrefab, loadPrefab.transform.position, loadPrefab.transform.rotation);
        lp.name = loadPrefab.name;
        gc.SetScene(lp);
        Destroy(gameObject);
        gc.NavPlayer.transform.position = lp.transform.GetChild(0).GetChild(startPointId).position;
    }
}
