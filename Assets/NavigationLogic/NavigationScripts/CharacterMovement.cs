using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    Rigidbody2D myRig;
    SpriteRenderer sprite;
    public float speed = 5;
    public float speedMod = 1;
    Vector2 walkDirection = Vector2.zero;

    private void Awake()
    {
        myRig = GetComponent<Rigidbody2D>();
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

	// Update is called once per frame
	void Update () {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        if (Mathf.Abs(xAxis) < 0.15f) xAxis = 0;
        if (Mathf.Abs(yAxis) < 0.15f) yAxis = 0;

        walkDirection = new Vector3(xAxis, yAxis);
        myRig.velocity = walkDirection.normalized * speed*speedMod;
    }

    public void SetLayer(int layer)
    {
        sprite.sortingOrder = layer;
    }
}
