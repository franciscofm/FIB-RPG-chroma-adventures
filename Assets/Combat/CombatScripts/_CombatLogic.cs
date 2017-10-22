using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _CombatLogic {

    //static GameObject active;
    static List<GameObject> allies;
    static List<GameObject> enemies;
	static List<GameObject> units = new List<GameObject> ();

	static GameObject[] turns = new GameObject[100];

	public enum Team { Ally, Enemy };
	public enum Type { Amor, Odio, Sida };

	public static CombatHudController HUD;

	public static void setAll(List<GameObject> all, List<GameObject> enem) {
		allies = all;
		enemies = enem;
        for (int i = 0; i < turns.Length; ++i)
            turns[i] = null;
        units.Clear();
        for (int i = 0; i < allies.Count; ++i)
			units.Add (allies [i]);
		for (int i = 0; i < enemies.Count; ++i)
			units.Add (enemies [i]);
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

	public static void diesUnit(Team t, GameObject g) {
        for(int i = 0; i < turns.Length; ++i)
        {
            if(g == turns[i])
            {
                turns[i] = null;
                HUD.addTimelineImage(null, i);
                break;
            }
        }
		units.Remove (g);
		if (t == Team.Ally)
			allies.Remove (g);
		else
			enemies.Remove (g);
		HUD.diesUnit (t, g);
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
				HUD.addTimelineImage (s.icon,0);
			} else {
				//ponerlo a distancia turno
				int pos = Mathf.CeilToInt((s.turns-1) * (1f + s.speed * 0.1f));
				placeTurn (t, s, pos);
			}
        }        
        HUD.updateHUD(); //Test purposes
    }

	//Pone el objeto t en la posicion pos (s es el script _Stats de t)
	private static void placeTurn(GameObject t, _Stats s, int pos){
        pos = Mathf.Clamp(pos,0, turns.Length - 1);
		if (turns [pos] == null) {
			turns [pos] = t;
			HUD.addTimelineImage (s.icon,pos);
		}
		else {
			GameObject other = turns [pos];
			if (s.initiative > other.GetComponent<_Stats> ().initiative) {
				//move turns[pos] to turns[pos+1]
				for (int j = (turns.Length - 2); pos <= j; --j) {
					turns [j+1] = turns [j];
					if (turns [j + 1] != null) {
						HUD.addTimelineImage (turns [j+1].GetComponent<_Stats> ().icon, j+1);
					} else HUD.addTimelineImage(null, j+1);

                }
                turns [pos] = t;
				HUD.addTimelineImage (s.icon,pos);
			} else {
				//move t to turns[pos+1]
				for (int j = (turns.Length - 2); (pos + 1) < j; --j) {
					turns [j+1] = turns [j];
					if (turns [j + 1] != null) {
                        HUD.addTimelineImage(turns[j + 1].GetComponent<_Stats>().icon, j + 1);
                    } else HUD.addTimelineImage(null, j + 1);
                }
                turns [pos + 1] = t;


                HUD.addTimelineImage (s.icon,pos+1);
			}
		}
	}

	public static void printTimeLine(){
		string output = "";
		for (int i = 0; i < turns.Length; ++i)
			if (turns [i] != null)
				output += i+ ": " +turns [i].name + Environment.NewLine;
	}

	/*
	 * Funcion llamada mientras estamos en seleccion de habilidad
	 * */
	public static int previewNextTurn(_Skill script) {
		if (script.speed == 1f) {
			return 0;
		} else {
			_Stats stats = turns[0].GetComponent<_Stats>();
			int t = Mathf.CeilToInt(script.speed * (1f + stats.speed * 0.1f)) / 2;
			if (turns [t] == null)
				return t;
			if (stats.initiative <= turns [t].GetComponent<_Stats> ().initiative)
				return t + 1;
			return t;
		}
	}

	/*
	 * Funcion llamada al inicio de lanzar una habilidad
	 * */
	public static void startAction(GameObject target, _Skill script) {
		HUD.setUserActive (false);
		_Stats stats = turns[0].GetComponent<_Stats>();
		stats.armor = 0f;
		//Animacion que desencadena la mierda que toque
		turns [0].GetComponent<Animator> ().SetTrigger ("Skill" + script.pos); 	//en primer caso llama attack y endTurn
																				//en ser cargada no llamara nada
		//Lanzamos accion inmediata
		if (script.speed == 1f) {
			stats.nextSkill = null;
            stats.preparingSkill = false;
			stats.nextTarget = target;
            int pos = Mathf.CeilToInt(stats.turns * (1f + stats.speed * 0.1f));
            placeTurn(turns[0], stats, pos);
            turns[0] = null;
            HUD.addTimelineImage(null, 0);
            //Lanzamos accion con carga
        }
        else {
			int pos = Mathf.CeilToInt(stats.turns * (1f + stats.speed * 0.1f)) / 2;
			stats.nextSkill = script;
            stats.preparingSkill = true;
            stats.nextTarget = target;
			stats.nextSkillTrigger = "Skill" + script.pos;
            placeTurn (turns [0], stats, pos);
			endTurn ();
		}
	}

	/*
	 * Realiza una habilidad(skill) de attacker a target
	 * Si se puede realizar la accion se debe comprobar antes de esta funcion
	 * */
	public static void attack(GameObject attacker, GameObject target, _Skill skill) {
        if (attacker == null || target == null || skill == null) return;
		_Stats statsAttacker = attacker.GetComponent<_Stats> ();
		if (target == null)
			target = selectNewTarget (statsAttacker.team);
		_Stats statsTarget = target.GetComponent<_Stats> ();
		int damage = (int)(statsAttacker.attack * skill.attack);
		statsAttacker.cost -= skill.cost;
		if (skill.both) {
			if (skill.area) {
				for (int i = 0; i < units.Count; ++i) {
					//aplicar dano a todos los enemigos
					_Stats tmp = units [i].GetComponent<_Stats> ();
					tmp.getAttack (damage, statsAttacker.type);
					tmp.armor = skill.armor;
					recalculateTurn (units [i], tmp);
				}
			} else {
				statsAttacker.getAttack (damage, statsAttacker.type);
				recalculateTurn (attacker, statsAttacker);
				statsTarget.getAttack (damage, statsAttacker.type);
				recalculateTurn (target, statsTarget);
			}
		} else {
			if (skill.area) {
				List<GameObject> targets;
				targets = (statsTarget.team == Team.Ally) ? allies : enemies;
				for (int i = 0; i < targets.Count; ++i) {
					_Stats tmp = targets [i].GetComponent<_Stats> ();
					tmp.getAttack (damage, statsAttacker.type);
					tmp.armor = skill.armor;
					recalculateTurn (targets [i], tmp);
				}
			} else {
				statsTarget.getAttack (damage, statsAttacker.type);
				statsTarget.armor = skill.armor;
				recalculateTurn (target, statsTarget);
			}
		}
	}

	static GameObject selectNewTarget(Team team){
		return (team == Team.Ally) ? allies [0] : enemies [0];
	}

	/*
	 * Funcion llamada al haber un cambio de speed en los _Stats de una unidad
	 * si pos es -1, su busca el t en el array primero
	 * */
	static void recalculateTurn(GameObject t, _Stats s) {
        int pos = -1;
		bool found = false;
		for (pos = 0; (pos < turns.Length) && (!found); ++pos) {
			if (turns [pos] == t)
				found = true;
		}
        if (pos < 0 || pos >= turns.Length) return;
		int newPos = Mathf.Clamp(Mathf.CeilToInt (pos * (1f + s.speed * 0.1f)), 0, turns.Length-1);
		if (pos != newPos) {
			turns [pos] = null;
			placeTurn (t, s, newPos);
		}
	}

	/*
	 * Funcion llamada cuando acaba el turno actual
	 * */
	public static void endTurn(){

		if (allies.Count == 0) {
			HUD.gameOver ();
		} else if (enemies.Count == 0) {
			HUD.winGame ();
			for (int i = 0; i < allies.Count; ++i) {
				allies[i].GetComponent<Animator> ().SetTrigger ("Win");
			}
		} else {
			//Desplazamos el array de turnos
			int count = 0;
			int dist = 0;

            for (int i = 1; (i < turns.Length) && (count < units.Count); ++i) {

                if (turns [i] != null) {

                    ++count;
					if (dist == 0) {
						dist = i;
					}
					turns [i - dist] = turns [i];
                    turns[i] = null;
					HUD.addTimelineImage (turns [i - dist].GetComponent<_Stats>().icon,(i-dist));
                    HUD.addTimelineImage(null, i);

                }

            }

            GameObject t = turns[0];
            _Stats s = t.GetComponent<_Stats> ();
			s.armor = 0f;
			if (s.preparingSkill) {
				//TODO: lanzar accion 
				turns [0].GetComponent<Animator> ().SetTrigger (s.nextSkillTrigger); //esta llamara attack y endTurn

				s.nextSkill = null;
                s.preparingSkill = false;
                s.nextTarget = null;
				s.nextSkillTrigger = "";


                int pos = Mathf.CeilToInt(s.speed * (1f + s.speed * 0.1f));
                placeTurn(turns[0], s, pos);
                turns[0] = null;
            } else if (s.team == Team.Enemy) {
                //TODO: IA del enemigo (random)
				startAction(allies[UnityEngine.Random.Range(0,allies.Count)],s.setSkills[UnityEngine.Random.Range(0,s.setSkills.Length)]);
			} else {
                //TODO: habilitar control del jugador
                HUD.updateHUD();
				HUD.setUserActive(true);
			}
		}
	}

	public static GameObject getTurnZero() {
		return turns [0];
	}
}
