using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour {

	public SceneScript myScene;
	public SceneScript.SceneEvent eventS;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.transform.tag == "Player")
		{
			myScene.SetEvent (eventS);
		}
	}
}
