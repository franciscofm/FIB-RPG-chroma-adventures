using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject navigationPlayer;
    public GameObject firstScene;

    GameObject navPlayer;
    GameObject navScene;

	// Use this for initialization
	void Start () {
        navScene = Instantiate(firstScene, Vector3.zero, Quaternion.identity);
        navPlayer = Instantiate(navigationPlayer, Vector3.zero, Quaternion.identity);
        navPlayer.transform.position = navScene.transform.GetChild(0).GetChild(0).position;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
