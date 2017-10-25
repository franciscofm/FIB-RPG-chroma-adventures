using UnityEngine;
using UnityEngine.UI;

public class DescriptionScript : MonoBehaviour {

	public Text descripcion;
	public Image image;

	string salto;
	// Use this for initialization
	void Start () {
		salto = System.Environment.NewLine;
	}
	
	public void setText(_Skill skill) {
		descripcion.text = "Damage multiplier: "+skill.attack + salto;
		descripcion.text += "Turn multiplier: "+skill.speed + salto;
		descripcion.text += ((skill.area) ? "Area: Yes" : "Area: No" )+ salto;
		descripcion.text += ((skill.both) ? "Both: Yes" : "Area: No" )+ salto;

		image.sprite = skill.image;
	}
}
