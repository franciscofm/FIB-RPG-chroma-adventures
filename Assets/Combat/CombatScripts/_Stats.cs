using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Stats : MonoBehaviour {
	
	[Tooltip("Amigo o enemigo")]
	public _CombatLogic.Team team;	
	[Tooltip("Tipo personaje")]
	public _CombatLogic.Type type;
	[Tooltip("Icono")]
	public Sprite icon;
	[Tooltip("Poder de las habilidades")]
	public int attack;
	[Tooltip("Vida")]
	public int health;
	[Tooltip("Coste")]
	public int cost;
	[Tooltip("Reduccion del dano en %")]
	public float armor;

	[Tooltip("Distancia entre turnos")]
	public int turns;
	[Tooltip("Determina primer turno y desempate con disputa de posicion")]
	public int initiative;
	[Tooltip("Cargas de velocidad [-3,3]")]
	public int speed;

	[Tooltip("Debug pursposes: habilidad cargando")]
	public _Skill nextSkill = null;
    public bool preparingSkill = false;
	public GameObject nextTarget = null;
	public string nextSkillTrigger = "";
	[Tooltip("Habilidades del personaje")]
	public _Skill[] setSkills;

    public void launchFastAttack(int i)
    {
        _CombatLogic.attack(gameObject, nextTarget, setSkills[i]);
    }

    public void launchChargedAttack() {
        _CombatLogic.attack(gameObject, nextTarget, nextSkill);
    }

	public void getAttack(int damage, _CombatLogic.Type type) {
		if (damage < 0) {
			health -= damage;
		} else {
			health -= (int) (damage * armor);
		}
		if (
			(this.type == _CombatLogic.Type.Amor && type == _CombatLogic.Type.Odio) ||
			(this.type == _CombatLogic.Type.Odio && type == _CombatLogic.Type.Sida) ||
			(this.type == _CombatLogic.Type.Sida && type == _CombatLogic.Type.Amor)) {
				speed = Mathf.Clamp (speed + 1, -3, +3);
		} else {
			speed = Mathf.Clamp (speed - 1, -3, +3);
		}
		if (health <= 0) {
			_CombatLogic.diesUnit (team, gameObject);
			Destroy (gameObject);
		}
	}

}
