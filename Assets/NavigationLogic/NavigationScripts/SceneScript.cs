using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneScript : MonoBehaviour {

    public GameController gc;

	public enum SceneEvent
	{
		Card,
		Boss
	}
	public void SetScene(GameObject loadPrefab, int startPointId)
    {
        GameObject lp = Instantiate(loadPrefab, loadPrefab.transform.position, loadPrefab.transform.rotation);
        lp.name = loadPrefab.name;
        gc.SetScene(lp);
        Destroy(gameObject);
        gc.NavPlayer.transform.position = lp.transform.GetChild(0).GetChild(startPointId).position;
    }

	public void SetEvent(SceneEvent eventS)
	{
		if (eventS == SceneEvent.Card)
			gc.SetCard ();
		else if (eventS == SceneEvent.Boss)
			gc.SetBoss ();
	}
}
