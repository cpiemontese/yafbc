using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float constantVelocity = 1.0f;
    public float impulseMagnitude = 1.0f;

    private bool jumped = false;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        jumped = Input.GetKeyDown(KeyCode.Space);
    }

    void FixedUpdate() {
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = new Vector2(constantVelocity, rigidbody2D.velocity.y);
        if (jumped) {
            rigidbody2D.AddForce(impulseMagnitude * Vector2.up, ForceMode2D.Impulse);
        }
    }
}
