using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    InputScript input;

    Vector3 position;
    Vector3 direction;
    Vector3 velocity;
    [SerializeField] float speed;

    Vector3 aim;

    //line
    LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        input = FindAnyObjectByType<InputScript>();

        position = transform.position;

        //line
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = input.lStick;
        aim = input.rStick;

        Move();
        Rotate();
        DrawLineFront();
    }

    void Move()
    {
        velocity = direction * speed;
        position += velocity * Time.deltaTime;
        transform.position = position;
    }

    void Rotate()
    {
        transform.rotation = Quaternion.LookRotation(aim);
    }

    void DrawLineFront()
    {
        //�O�ɐ���`��
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + aim * 10f);
    }
}
