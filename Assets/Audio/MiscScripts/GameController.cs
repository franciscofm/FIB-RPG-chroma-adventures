using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject dummy1;
	public GameObject dummy2;
	public GameObject dummy3;
	public GameObject dummy4;
	public GameObject dummy5;
	public GameObject dummy6;

    public GameObject navigationPlayerPrefab;
    public GameObject firstScenePrefab;
    public GameObject audioControllerPrefab;
	public GameObject hudPrefab;

    public float minRandTime = 5f;
    public float maxRandTime = 8f;

    CharacterMovement navPlayer;
    public CharacterMovement NavPlayer
    {
        get { return navPlayer; }
    }
    SceneScript navScene;
    AudioManager audioMan;
	CombatHudController chc;

    float timeCounter = 0f;

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
        audioMan = (Instantiate(audioControllerPrefab, Vector3.zero, Quaternion.identity)).GetComponent<AudioManager>();
        audioMan.PlayNavMusic();

		chc = (Instantiate(hudPrefab, Vector3.zero, Quaternion.identity)).transform.GetChild(0).GetComponent<CombatHudController>();
		chc.setGC (this);

        timeCounter = Random.Range(minRandTime, maxRandTime);
    }

    bool initBattle = false;
	
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
            if(!initBattle)
            {
                List<GameObject> allies = new List<GameObject>();
                allies.Add(dummy1);
                allies.Add(dummy2);
                allies.Add(dummy3);
                List<GameObject> enemies = new List<GameObject>();
                enemies.Add(dummy4);
                enemies.Add(dummy5);
                enemies.Add(dummy6);
                chc.init(allies, enemies);
                initBattle = true;
            }
            if(Input.GetButtonDown("Fire1"))
            {
                StartNavigationState();
            }
        }
	}

    void StartCombatState()
    {
        //Debug.Log("FUISH FUISH!");
        state = GameControllerState.Battle;
        
        navScene.gameObject.SetActive(false);
        navPlayer.gameObject.SetActive(false);
        
		chc.transform.parent.gameObject.SetActive(true);
        initBattle = false;

        //init combat controller
        audioMan.PlayCombatMusic();


    }

    void StartNavigationState()
    {
        state = GameControllerState.Navigation;
        chc.winGame();
        chc.transform.parent.gameObject.SetActive(false);
        //disable allys
        //disable enemies
        navScene.gameObject.SetActive(true);
        navPlayer.gameObject.SetActive(true);
        audioMan.PlayNavMusic();
    }

    public void SetScene(GameObject scene)
    {
        navScene = scene.GetComponent<SceneScript>();
        navScene.gc = this;
    }
}
