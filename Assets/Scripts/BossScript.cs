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
    float moveTime;
    [Header("Moveフェーズ")]
    [SerializeField] float kMoveTime;
    [SerializeField] float moveSpeed;

    //Shot
    [Header("Shotフェーズ")]
    [SerializeField] GameObject hand;
    Vector3 shotDirection;
    [SerializeField] float shotSpeed;
    float shotInterval;
    [SerializeField] float kShotInterval;
    int shotCount;

    //HP関連
    bool isDamage;
    Color color;

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
        Damage();
    }

    void PhaseChange()
    {
        //phaseInterval -= Time.deltaTime;
        if (GoNextPhase)
        {
            //phaseInterval = kPhaseInterval;

            if (attackFase == AttackFase.None || attackFase == AttackFase.Move)
            {
                //int rand = Random.Range(0, 1000);
                //if (rand < 500)
                //{
                //    attackFase = AttackFase.Shot;
                //    shotInterval = kShotInterval;
                //}
                //else
                //{
                //    attackFase = AttackFase.Rush;
                //}

                attackFase = AttackFase.Shot;
                shotInterval = kShotInterval;
                shotCount = 0;

                GoNextPhase = false;
            }
        }

        if (GoNextPhase)
        {
            if (attackFase == AttackFase.Shot || attackFase == AttackFase.Rush)
            {
                attackFase = AttackFase.Move;
                moveTime = kMoveTime;

                GoNextPhase = false;
            }
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

                ShotPhase();

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
            int rand = Random.Range(0, 1000);
            float angle;
            if (rand < 500)
            {
                angle = 30;
            }
            else
            {
                angle = -30;
            }
            direction = RotateAroundAxis(eToPDirection.normalized, angle, Vector3.up);
        }

        moveTime -= Time.deltaTime;
        if (moveTime < 0)
        {
            GoNextPhase = true;
        }

        //動かす
        velocity = direction.normalized * moveSpeed;
        position += velocity * Time.deltaTime;
        position.y = 1.51f;
        transform.position = position;
    }

    void ShotPhase()
    {
        shotInterval -= Time.deltaTime;
        if (shotInterval < 0)
        {
            shotCount++;
            shotInterval = kShotInterval;

            //発射
            shotDirection = (player.transform.position - transform.position).normalized;
            shotDirection.y = 0;

            GameObject newHand = Instantiate(hand, transform.position, Quaternion.identity);
            newHand.GetComponent<BossHandScript>().Setting(shotDirection.normalized, shotSpeed);
        }

        if (shotCount > 3)
        {
            GoNextPhase = true;
        }
    }

    void Damage()
    {
        if (isDamage)
        {
            isDamage = false;
            color = Color.white;
        }

        color = Color.Lerp(color, Color.red, 5f * Time.deltaTime);
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tatami")
        {
            TatamiScript tatami;
            if (other.gameObject.TryGetComponent<TatamiScript>(out tatami))
            {
                tatami.IsColored = false;
            }
        }

        if (other.tag == "Bullet")
        {
            isDamage = true;
        }
    }

    Vector3 RotateAroundAxis(Vector3 v, float angleDeg, Vector3 axis)
    {
        return Quaternion.AngleAxis(angleDeg, axis.normalized) * v;
    }
}
