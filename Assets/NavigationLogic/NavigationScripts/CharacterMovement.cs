using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    Rigidbody2D myRig;
    SpriteRenderer sprite;
    public float speed = 5;
    public float speedMod = 1;
    Animator anim;
    bool isMoving = false;

    public bool PlayerMoving
    {
        get { return isMoving; }
    }

    private void Awake()
    {
        myRig = GetComponent<Rigidbody2D>();
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

	// Update is called once per frame
	void Update () {
        bool up = false;
        bool down = false;
        bool right = false;
        bool left = false;
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        if (Mathf.Abs(xAxis) < 0.15f) xAxis = 0;
        if (Mathf.Abs(yAxis) < 0.15f) yAxis = 0;

        up = (yAxis >= 0.15f);
        down = (yAxis <= -0.15f);
        left = (xAxis <= -0.15f);
        right = (xAxis >= 0.15f);

        isMoving = up || down || left || right;

        up = up && Mathf.Abs(yAxis) > Mathf.Abs(xAxis);
        down = down && Mathf.Abs(yAxis) > Mathf.Abs(xAxis);
        left = left && Mathf.Abs(xAxis) > Mathf.Abs(yAxis);
        right = right && Mathf.Abs(xAxis) > Mathf.Abs(yAxis);

        float xSpeedMod = (speedMod <= .2f) ? speedMod * 5f : (speedMod < 1) ? 1 : speedMod;
        myRig.velocity = new Vector2(xAxis*xSpeedMod, yAxis*speedMod)*speed;

        anim.SetBool("Up", up);
        anim.SetBool("Down", down);
        anim.SetBool("Left", left);
        anim.SetBool("Right", right);
    }

    public void SetLayer(int layer)
    {
        sprite.sortingOrder = layer;
    }

}
