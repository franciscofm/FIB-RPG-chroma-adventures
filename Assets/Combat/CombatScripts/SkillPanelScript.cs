using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanelScript : MonoBehaviour {

    public GameObject[] button; //Los tres botones del canvas (izquierda)

    public GameObject description; //Cuadrado del centro

    public GameObject enemy; //Barra derecha

    public void updatePanel(_Stats stats) {
        //Cambiar componente en el script de los botones a la skill que toque (stats.setSkills[i])
        for(int i=0; i<button.Length; ++i)
        {
            button[i].GetComponent<ButtonScript>().setSkill(stats.setSkills[i]);
        }
    }
}
