using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class _Skill {
	[Tooltip("Posicion de la habilidad en el array (si no peta)")]
	public int pos;
	[Tooltip("Multiplicador de velocidad >= 1f (1f = instant)")]
	public float speed;
	[Tooltip("Multiplicador de poder de ataque")]
	public float attack;
	[Tooltip("Indice de defensa (%)")]
	public float armor;
	[Tooltip("Afecta al grupo entero")]
	public bool area;
	[Tooltip("Afecta a los dos equipos")]
	public bool both;


	[Tooltip("Imagen en HUD")]
	public Sprite image;
	[Tooltip("Nombre en HUD")]
	public string name;
	[Tooltip("Coste (Mana/Overcharge)")]
	public int cost;

}
