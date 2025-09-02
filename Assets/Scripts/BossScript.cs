using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    Vector3 position;
    Vector3 direction;
    Vector3 velocity;

    PlayerScript player;

    //攻撃フェーズ
    enum AttackFase
    {
        None, Move, Shot, Rush
    }

    AttackFase attackFase;
    bool GoNextPhase;

    //Move
    [Header("Moveフェーズ")]
    float moveTime;
    [SerializeField] float kMoveTime;
    [SerializeField] float moveSpeed;

    //float phaseInterval;
    //[SerializeField] float kPhaseInterval;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerScript>();
        attackFase = AttackFase.Move;

        position = transform.position;

        moveTime = kMoveTime;
    }

    // Update is called once per frame
    void Update()
    {
        PhaseChange();
        Phase();
    }

    void PhaseChange()
    {
        //phaseInterval -= Time.deltaTime;
        if (GoNextPhase)
        {
            //phaseInterval = kPhaseInterval;

            if (attackFase == AttackFase.None || attackFase == AttackFase.Move)
            {
                int rand = Random.Range(0, 1000);
                if (rand < 500)
                {
                    attackFase = AttackFase.Shot;
                }
                else
                {
                    attackFase = AttackFase.Rush;
                }
            }
            GoNextPhase = false;
        }

        if (GoNextPhase)
        {
            if (attackFase == AttackFase.Shot || attackFase == AttackFase.Rush)
            {
                attackFase = AttackFase.Move;
            }
            GoNextPhase = false;
        }
    }

    void Phase()
    {
        switch (attackFase)
        {
            case AttackFase.None:



                break;

            case AttackFase.Move:

                MovePhase();

                break;

            case AttackFase.Shot:



                break;

            case AttackFase.Rush:



                break;
        }
    }

    void MovePhase()
    {
        if (moveTime == kMoveTime)
        {
            Vector3 eToPDirection = (player.transform.position - transform.position).normalized;
            direction = RotateAroundAxis(eToPDirection.normalized, 30, Vector3.up);
        }

        moveTime -= Time.deltaTime;
        if (moveTime < 0)
        {
            GoNextPhase = true;
        }

        //動かす
        velocity = direction.normalized * moveSpeed;
        position += velocity * Time.deltaTime;
        position.y = 1.88f;
        transform.position = position;
    }

    Vector3 RotateAroundAxis(Vector3 v, float angleDeg, Vector3 axis)
    {
        return Quaternion.AngleAxis(angleDeg, axis.normalized) * v;
    }
}
