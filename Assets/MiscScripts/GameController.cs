using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject navigationPlayerPrefab;
    public GameObject firstScenePrefab;
    public float minRandTime = 5f;
    public float maxRandTime = 8f;

    CharacterMovement navPlayer;
    public CharacterMovement NavPlayer
    {
        get { return navPlayer; }
    }
    SceneScript navScene;

    float timeCounter = 0;

    public enum GameControllerState
    {
        Navigation,
        Battle
    }

    private GameControllerState state = GameControllerState.Navigation;
    public GameControllerState State
    {
        get { return state; }
    }

    // Use this for initialization
    void Start () {
        navScene = (Instantiate(firstScenePrefab, Vector3.zero, Quaternion.identity)).GetComponent<SceneScript>();
        navScene.gc = this;
        navPlayer = (Instantiate(navigationPlayerPrefab, Vector3.zero, Quaternion.identity)).GetComponent<CharacterMovement>();
        navPlayer.transform.position = navScene.transform.GetChild(0).GetChild(0).position;
        timeCounter = Random.Range(minRandTime, maxRandTime);
    }
	
	// Update is called once per frame
	void Update () {
        if(state == GameControllerState.Navigation)
        {
            timeCounter -= Time.deltaTime;
            if (timeCounter <= 0)
            {
                timeCounter = Random.Range(minRandTime, maxRandTime);
                if (navPlayer.PlayerMoving) StartCombatState();
            }

        }
        else if(state == GameControllerState.Battle)
        {

        }
	}

    void StartCombatState()
    {
        Debug.Log("FUISH FUISH!");
        /*
        navScene.gameObject.SetActive(false);
        navPlayer.gameObject.SetActive(false);
        */
        //combScene.gameObject.SetActive(true);
        //enable allys
        //enable enemies
        //init combat controller

    }

    void StartNavigationState()
    {

        //combScene.gameObject.SetActive(false);
        //disable allys
        //disable enemies
        navScene.gameObject.SetActive(true);
        navPlayer.gameObject.SetActive(true);
    }

    public void SetScene(GameObject scene)
    {
        navScene = scene.GetComponent<SceneScript>();
        navScene.gc = this;
    }
}
