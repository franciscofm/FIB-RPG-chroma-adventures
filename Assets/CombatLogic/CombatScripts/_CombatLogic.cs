using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _CombatLogic {

	//static GameObject active;
	static List<GameObject> allies = new List<GameObject>();
	static List<GameObject> enemies = new List<GameObject>();
	static List<GameObject> units = new List<GameObject> ();

	static GameObject[] turns = new GameObject[100];

	public enum Team { Ally, Enemy };
	public enum Type { Amor, Odio, Sida };

	/*
	 * Limpia las listas de allies, enemies y units
	 * */
	public static void clear(){
		allies.Clear ();
		enemies.Clear ();
		units.Clear ();
		for (int i = 0; i < turns.Length; ++i)
			turns [i] = null;
	}

	/*
	 * Anade g a units y el equipo t
	 * */
	public static void addUnit(Team t, GameObject g) {
		units.Add (g);
		if (t == Team.Ally)
			allies.Add (g);
		else
			enemies.Add (g);
	}

	/*
	 * Elimina g de units y el equipo t
	 * */
	public static void removeUnit(Team t, GameObject g) {
		units.Remove (g);
		if (t == Team.Ally)
			allies.Remove (g);
		else
			enemies.Remove (g);
	}

	/*
	 * Prepara el vector de turnos (turns) con los primeros turnos de todas las unidades
	 * */
	public static void start(){
		int maxInit = units [0].GetComponent<_Stats> ().initiative;
		int maxSpeed = units [0].GetComponent<_Stats> ().speed;
		int selected = 0; //para ignorarlo en el for de poner el siguiente turno
		//active = units [0];
		for (int i = 1; i < units.Count; ++i) {
			_Stats t = units [i].GetComponent<_Stats> ();
			if (t.initiative > maxInit) {
				//select it
				maxInit = t.initiative;
				maxSpeed = t.speed;
				selected = i;
			} else if (t.initiative == maxInit) {
				if (t.speed > maxSpeed) {
					//select it
					maxInit = t.initiative;
					maxSpeed = t.speed;
					selected = i;
				}
			}
		}
		for (int i = 0; i < units.Count; ++i) {
			GameObject t = units [i];
			_Stats s = units [i].GetComponent<_Stats> ();
			if (i == selected) {
				//ponerlo en pos 0
				turns[0] = t;
			} else {
				//ponerlo a distancia turno
				int pos = Mathf.CeilToInt(s.turns * (1f + s.speed * 0.1f));
				placeTurn (t, s, pos);
			}
		}
	}

	//Pone el objeto t en la posicion pos (s es el script _Stats de t)
	private static void placeTurn(GameObject t, _Stats s, int pos){
		if (turns [pos] == null)
			turns [pos] = t;
		else {
			GameObject other = turns [pos];
			if (s.initiative > other.GetComponent<_Stats> ().initiative) {
				//move turns[pos] to turns[pos+1]
				for (int j = (turns.Length - 2); (pos + 1) < j; --j)
					turns [j] = turns [j - 1];
				turns [pos] = t;
			} else {
				//move t to turns[pos+1]
				for (int j = (turns.Length - 2); (pos + 2) < j; --j)
					turns [j] = turns [j - 1];
				turns [pos + 1] = t;
			}
		}
	}

	public static void printTimeLine(){
		string output = "";
		for (int i = 0; i < turns.Length; ++i)
			if (turns [i] != null)
				output += i+ ": " +turns [i].name + Environment.NewLine;
		Debug.Log (output);
	}

	/*
	 * Funcion llamada mientras estamos en seleccion de habilidad
	 * */
	public static int previewNextTurn(_Skill script) {
		int t = 0;
		if (script.speed == 1f) {
			//TODO: Llamar al controlador del HUD con la misma posicion
			Debug.Log ("Instant action");
		} else {
			//TODO: LLamar al controlador del HUD con siguiente pos
			_Stats stats = turns[0].GetComponent<_Stats>();
			t = Mathf.CeilToInt(script.speed * (1f + stats.speed * 0.1f)) / 2;
			Debug.Log ("Long action ("+t+")");
		}
		return t;
	}

	/*
	 * Funcion llamada al inicio de lanzar una habilidad
	 * */
	public static void startAction(_Skill script) {
		_Stats stats = turns[0].GetComponent<_Stats>();
		stats.armor = 0f;
		if (script.speed == 1f) {
			stats.nextSkill = null;
		} else {
			int pos = Mathf.CeilToInt(script.speed * (1f + stats.speed * 0.1f)) / 2;
			stats.nextSkill = script;
			placeTurn (turns [0], stats, pos);
		}
		//TODO: llamar a las animaciones/transiciones
		//TODO: llamar a endTurn con los parametros de tiempo
	}

	/*
	 * Realiza una habilidad(skill) de attacker a target
	 * Si se puede realizar la accion se debe comprobar antes de esta funcion
	 * */
	public static void attack(GameObject attacker, GameObject target, _Skill skill) {
		_Stats statsAttacker = attacker.GetComponent<_Stats> ();
		_Stats statsTarget = target.GetComponent<_Stats> ();
		int damage = (int)(statsAttacker.attack * skill.attack);
		statsAttacker.cost -= skill.cost;
		if (skill.both) {
			for (int i = 0; i < units.Count; ++i) {
				//aplicar dano a todos los enemigos
				_Stats tmp = units [i].GetComponent<_Stats> ();
				tmp.getAttack (damage,statsAttacker.type);
				tmp.armor = skill.armor;
			}
		} else {
			statsTarget.getAttack (damage,statsAttacker.type);
			statsTarget.armor = skill.armor;
		}
	}

	/*
	 * Funcion llamada al haber un cambio de speed en los _Stats de una unidad
	 * si pos es -1, su busca el t en el array primero
	 * */
	static void recalculateTurn(GameObject t, int pos) {
		if (pos == -1) {
			bool found = false;
			for (pos = 0; (pos < turns.Length) && (!found); ++pos) {
				if (turns [pos] == t)
					found = true;
			}
		}
		_Stats s = t.GetComponent<_Stats> ();
		int newPos = Mathf.CeilToInt (s.turns * (1f + s.speed * 0.1f));
		if (pos != newPos) {
			turns [pos] = null;
			placeTurn (t, s, newPos);
		}
	}

	/*
	 * Funcion llamada cuando acaba el turno actual
	 * nextTurn es el siguiente turno en que el pj actual volvera a actuar
	 * */
	public static void endTurn(int nextTurn){
		if (allies.Count == 0) {
			//Hemos perdido
		} else if (enemies.Count == 0) {
			//Se ha acabado el combate
		} else {
			//Desplazamos el array de turnos
			bool found = false;
			int count = 0;
			int dist = 0;
			GameObject t = turns [0];
			for (int i = 1; (i < turns.Length) && (count < units.Count - 1); ++i) {
				if (turns [i] != null) {
					++count;
					if (!found) {
						found = true;
						dist = i;
					}
					turns [i - dist] = turns [i];
				}
			}
			_Stats s = t.GetComponent<_Stats> ();
			placeTurn (t, s, Mathf.CeilToInt(s.turns * (1f + s.speed * 0.1f)));
		}
	}
}
