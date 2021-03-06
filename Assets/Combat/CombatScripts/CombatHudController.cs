using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatHudController : MonoBehaviour {

	//
	GameController GC;
	List<GameObject> allies; public Transform alliesPosition;
	List<GameObject> enemies; public Transform enemiesPosition;

	public void setGC(GameController gc) {
		GC = gc;
	}

	public void init(List<GameObject> allies, List<GameObject> enemies) {

		for (int i = 0; i < allies.Count; ++i) {
			SpriteRenderer sprite = alliesPosition.GetChild (i).GetComponent<SpriteRenderer> ();

			bool flip = sprite.flipX;
			Vector3 scale = allies [i].transform.localScale;
			int order = sprite.sortingOrder;

			allies [i] = GameObject.Instantiate (allies [i]);
			allies [i].transform.position = alliesPosition.GetChild (i).transform.position;
			allies[i].transform.localScale = alliesPosition.GetChild (i).transform.localScale;

			sprite = allies [i].GetComponent<SpriteRenderer> ();
			sprite.sortingOrder = order;
			sprite.flipX = flip;
		}
		for (int i = 0; i < enemies.Count; ++i) {
			SpriteRenderer sprite = enemiesPosition.GetChild (i).GetComponent<SpriteRenderer> ();

			bool flip = sprite.flipX;
			Vector3 scale = enemies [i].transform.localScale;
			int order = sprite.sortingOrder;

			enemies [i] = GameObject.Instantiate (enemies [i]);
			enemies [i].transform.position = enemiesPosition.GetChild (i).transform.position;
			enemies[i].transform.localScale = enemiesPosition.GetChild (i).transform.localScale;

			sprite = enemies [i].GetComponent<SpriteRenderer> ();
			sprite.sortingOrder = order;
			sprite.flipX = flip;
		}
        target = enemies[0];

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

	Color color;
	public GameObject turn; //El prefab
	public GameObject timeLine; //El de escena
	public GameObject skillPanel; //El de escena
	void Start () {
		color = turn.GetComponentInChildren<Image> ().color;
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
        i.sprite = null;
	}

	public void addTimelineImage(Sprite im, int pos) {
        if (pos >= timeLine.transform.childCount)
            return;
        if (im == null)
        {
            Image i = timeLine.transform.GetChild(pos).GetComponentInChildren<Image>();
            i.sprite = null;
            i.color = color;
        }
        else
        {            
            Image i = timeLine.transform.GetChild(pos).GetComponentInChildren<Image>();
            i.sprite = im;
            i.color = Color.white;
        }
	}

	public void updateHUD()
    {
        //actualizar skills
        _Stats act = _CombatLogic.getTurnZero().GetComponent<_Stats>();
        //Llamar al hijo que hace el display de todo
        skillPanel.GetComponent<SkillPanelScript>().updatePanel(act);
    }
	// Update is called once per frame
	bool userActive = false;
    GameObject target;
	void Update () {
		if (!userActive)
			return;
		//seleccionar habilidad
			//seleccionar target
			//cancelar seleccion
	}

	public void setUserActive(bool b) {
		userActive = b;
        skillPanel.SetActive(b);
       
	}

    public GameObject getTarget()
    {
        return target;
    }

	public void gameOver() {
		//Sacar pantalla con el titulo de gameOver y cargar menu inicial
		Application.Quit();
	}

	public void winGame() {
		//esperar unos 3~4 segundos mientras acaban las animaciones
		//llamar al super controllador y cargar navegacion
        for(int i = 0; i < allies.Count; ++i)
        {
            Destroy(allies[i]);
        }

        allies.Clear();

        for (int i = 0; i < enemies.Count; ++i)
        {
            Destroy(enemies[i]);
        }
        enemies.Clear();
    }
}
