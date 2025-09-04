using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildScript : MonoBehaviour
{
    Vector3 position;
    Vector3 direction;
    Vector3 velocity;
    [SerializeField] float speed;
    [SerializeField] float accel;

    [SerializeField] float fallSpeed;
    [SerializeField] float fallAccel;

    PlayerScript player;

    enum Phase
    {
        Fall, Chase
    }
    Phase phase;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerScript>();

        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        switch (phase)
        {
            case Phase.Fall:

                fallSpeed += fallAccel * Time.deltaTime;
                position.y -= fallSpeed * Time.deltaTime;

                break;

            case Phase.Chase:

                if (speed < accel) { speed += accel * Time.deltaTime; }

                direction = (player.transform.position - transform.position).normalized;
                velocity = direction.normalized * speed;
                position += velocity * Time.deltaTime;

                break;
        }

        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tatami")
        {
            phase = Phase.Chase;
        }
    }
}
