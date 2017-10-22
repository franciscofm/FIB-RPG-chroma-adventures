using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

    _Skill skill;
    public GameObject Canvas;
    CombatHudController chc;
    Text _name;

    private void Start()
    {
        chc = Canvas.GetComponent<CombatHudController>();
        _name = GetComponentInChildren<Text>();
    }

	public void setSkill(_Skill s)
    {
        skill = s;
        _name.text = s.name;
    }
	
	public void pressButton()
    {
        _CombatLogic.startAction(chc.getTarget(), skill);
    }
}
