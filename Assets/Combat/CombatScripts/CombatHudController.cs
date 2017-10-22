using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatHudController : MonoBehaviour {

	//
	List<GameObject> allies;
	List<GameObject> enemies;
	public void init(List<GameObject> allies, List<GameObject> enemies) {
		_CombatLogic.clear ();
		_CombatLogic.setAll (allies, enemies);
		_CombatLogic.HUD = this;
		this.allies = allies;
		this.enemies = enemies;
		_CombatLogic.start ();
	}

	public void diesUnit(_CombatLogic.Team t, GameObject g) {
		if (t == _CombatLogic.Team.Ally)
			allies.Remove (g);
		else
			enemies.Remove (g);
	}

	public Color color;
	public GameObject turn; //El prefab
	public GameObject timeLine; //El de escena
	public GameObject skillPanel; //El de escena
	void Start () {
		//TimeLine creation
		RectTransform turnTrans = turn.GetComponent<RectTransform> ();
		float perc = turnTrans.anchorMax.x;
		int displayedTurns = (int) (1f / turnTrans.anchorMax.x);
		for (int i = 0; i <= displayedTurns; ++i) {
			GameObject t = GameObject.Instantiate (turn, timeLine.transform);
			turnTrans = t.GetComponent<RectTransform> ();
			turnTrans.anchorMin = new Vector2 (i * perc, 0f);
			turnTrans.anchorMax = new Vector2 ((i+1) * perc, 1f);
		}
	}

	List<GameObject> timeLineImages = new List<GameObject>();
	public void clearTimelineImages(){
		for (int i = 0; i < timeLineImages.Count; ++i) {
			Image im = timeLineImages [i].GetComponentInChildren<Image> ();
			im.color = color;
			im.sprite = null;
		}
		timeLineImages.Clear ();
	}

	public void removeTimelineImage(int pos) {
		if (pos >= timeLine.transform.childCount)
			return;
		Image i = timeLine.transform.GetChild (pos).GetComponentInChildren<Image> ();
		i.color = color;
	}

	public void addTimelineImage(Sprite im, int pos) {
		if (pos >= timeLine.transform.childCount)
			return;
		Image i = timeLine.transform.GetChild (pos).GetComponentInChildren<Image> ();
		i.sprite = im;
		i.color = Color.white;
	}
	
	// Update is called once per frame
	bool userActive = false;
	void Update () {
		if (!userActive)
			return;
		//seleccionar habilidad
			//seleccionar target
			//cancelar seleccion
	}

	public void setUserActive(bool b) {
		userActive = b;
	}

	public void gameOver() {
		//Sacar pantalla con el titulo de gameOver y cargar menu inicial
		Application.Quit();
	}

	public void winGame() {
		//esperar unos 3~4 segundos mientras acaban las animaciones
		//llamar al super controllador y cargar navegacion
	}
}
