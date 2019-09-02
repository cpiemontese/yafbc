using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text timeInFlightText;
    public float constantVelocity = 1.0f;
    public float impulseMagnitude = 1.0f;
    public event EventHandler resetEventHandlers;

    private bool jumped = false;
    private Rigidbody2D rigidbody2d;
    private float timeInFlight = 0.0f;

    public void Reset() {
        timeInFlight = 0.0f;
        transform.SetPositionAndRotation(Vector3.zero, transform.rotation);
        rigidbody2d.velocity = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start() {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        jumped = Input.GetKeyDown(KeyCode.Space);
        timeInFlight += Time.deltaTime;
        timeInFlightText.text = "Time in flight: " + TimeSpan.FromSeconds(timeInFlight).ToString(@"hh\:mm\:ss"); 
    }

    void FixedUpdate() {
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = new Vector2(constantVelocity, rigidbody2D.velocity.y);
        if (jumped) {
            rigidbody2D.AddForce(impulseMagnitude * Vector2.up, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Tile") {
            Reset();
            resetEventHandlers(this, null);
        }
    }
}
