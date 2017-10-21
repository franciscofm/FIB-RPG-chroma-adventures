using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatHudController : MonoBehaviour {

	//NOTE: this has to be recovered from object in another screen
	public List<GameObject> allies;
	public List<GameObject> enemies;
	public GameObject[] turns;

	public GameObject Timeline;
	public GameObject TimelineIconPrefab;

	public GameObject Canvas;
	public GameObject BattleMenu;
	public GameObject ButtonPrefab;
	public GameObject SkillTooltipPrefab;

	//substitute this
	public GameObject ally;
	public GameObject enemy;
	public GameObject ally_pos;
	public GameObject enemy_pos;

	public int maxNumTimeIcons;

	// Use this for initialization
	void Start () {

		//Timeline creation

		maxNumTimeIcons = 10;

		//TODO init combat?
		_CombatLogic.clear();
		//TODO FIX THISSSSSSSSSSSSSSSSSSSSSSSSSSSS
		//Add every enemy ally passed to me
		for(int i = 0; i < 4; ++i)
			_CombatLogic.addUnit(_CombatLogic.Team.Ally, ally);
		for(int i = 0; i < 4; ++i)
			_CombatLogic.addUnit(_CombatLogic.Team.Enemy, enemy);
		////////////////////////////////////////////
		_CombatLogic.start ();

		for (int i = 0; i < maxNumTimeIcons; ++i) {

			//TODO: get icons from turns???
			GameObject tli = Instantiate (TimelineIconPrefab, Timeline.transform);
			//change stuff from tli (icon)
		}

		//Menu creation
		//TODO: get skills from _skills in turn

		/*Skill[] skills = turns [0].GetComponent<_Stats> ().setSkills;*/


		//set tooltip event listeners
		//First create event data
		EventTrigger trigger;
		//Pointer enters
		EventTrigger.Entry entryPointer;
		//Object selected
		EventTrigger.Entry selected;
		//Pointer exits
		EventTrigger.Entry exitPointer;
		//Object selected
		EventTrigger.Entry deselected;

		//TODO: get skills of character
		int numSkills = 4;

		for (int i = 0; i < numSkills; ++i) {
			//TODO: get proper info from skills

			//Button spawn
			GameObject button = Instantiate (ButtonPrefab, BattleMenu.transform);
			if (i != 0) {
				button.transform.GetChild (0).GetComponent<Text> ().text = "Skill";
			}
			else {
				button.transform.GetChild (0).GetComponent<Text> ().text = "Attack";
				BattleMenu.GetComponent<SelectOnInput> ().selected = button;
			}

			//Button tooltip
			GameObject tooltip = Instantiate (SkillTooltipPrefab, Canvas.transform);
			//TODO: change image from _skill? Set image->
			//tooltip.transform.GetChild(0).GetComponent<Image>();
			if (i != 0) {
				tooltip.transform.GetChild (1).GetComponent<Text> ().text = "Skill";
			}
			else {
				tooltip.transform.GetChild (1).GetComponent<Text> ().text = "Attack";
			}
			tooltip.SetActive (false);

			//Set tooltip event listeners
			trigger = button.GetComponentInParent<EventTrigger>();

			//Pointer enters
			entryPointer = new EventTrigger.Entry();
			entryPointer.eventID = EventTriggerType.PointerEnter;
			entryPointer.callback.AddListener( (eventData) => { tooltip.SetActive(true); } );
			trigger.triggers.Add(entryPointer);

			//Object selected
			selected = new EventTrigger.Entry();
			selected.eventID = EventTriggerType.Select;
			selected.callback.AddListener( (eventData) => { tooltip.SetActive(true); } );
			trigger.triggers.Add(selected);

			//Pointer exits
			exitPointer = new EventTrigger.Entry();
			exitPointer.eventID = EventTriggerType.PointerExit;
			exitPointer.callback.AddListener( (eventData) => { tooltip.SetActive(false); } );
			trigger.triggers.Add(exitPointer);

			//Object selected
			deselected = new EventTrigger.Entry();
			deselected.eventID = EventTriggerType.Deselect;
			deselected.callback.AddListener( (eventData) => { tooltip.SetActive(false); } );
			trigger.triggers.Add(deselected);

		}

		//Spawn Characters
		//TODO: get enemies from combat logic, reposition them appropiatetly
		Vector3 offset = ally_pos.transform.position;
		for (int i = 0; i < 4; ++i) {
			GameObject a = Instantiate (ally,ally_pos.transform);
			offset.x += 1.0f;
			offset.y -= 1.0f;
			//ally_pos.transform.position = offset;
			a.transform.position = offset;
		}

		offset = enemy_pos.transform.position;
		for (int i = 0; i < 4; ++i) {
			GameObject b = Instantiate (enemy,enemy_pos.transform);

			offset.x -= 1.0f;
			offset.y -= 1.0f;
			//enemy_pos.transform.position = offset;
			b.transform.position = offset;
		}

	}
	
	// Update is called once per frame
	void Update () {
		//STEP 1
		//IF ALLY -> RElOAD MENU, 
		//			SELECT ACTION + PREVIEW
		//ELSE DO ENEMY ACTION RANDOM,
		//			DISABLE INPUT

		/*GameObject current = _CombatLogic.getCurrentTurn ();
		_Skill info = (_Skill)current;

		if(info.*/

		//STEP 2
		//EXECUTE ANIMATION ACTION, 
		//			DISABLE INPUT, 
		//			UPDATE TIMELINE
		
	}
}
