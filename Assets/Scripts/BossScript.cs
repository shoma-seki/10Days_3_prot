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
    enum AttackPhase
    {
        None, Move, Shot, RushPrepare, Rush
    }

    AttackPhase attackPhase;
    bool GoNextPhase;

    //None
    float noneTime;
    [Header("Noneフェーズ")]
    [SerializeField] float kNoneTime;

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

    //RusPrepare
    float rushPrepareTime;
    [Header("RushPrepareフェーズ")]
    [SerializeField] float kRushPrepareTime;

    //Rush
    float rushTime;
    float rushSpeed;
    Vector3 rushDirection;
    bool isRush;
    public bool IsRush { get { return isRush; } }
    [Header("Rushフェーズ")]
    [SerializeField] float kRushTime;
    [SerializeField] float kRushSpeed;

    //HP関連
    bool isDamage;
    Color color;

    //float phaseInterval;
    //[SerializeField] float kPhaseInterval;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerScript>();
        attackPhase = AttackPhase.Move;

        position = transform.position;

        moveTime = kMoveTime;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("attackPhase = " + attackPhase);

        PhaseChange();
        Phase();
        Damage();
        Rotate();
    }

    void PhaseChange()
    {
        //phaseInterval -= Time.deltaTime;
        if (GoNextPhase)
        {
            //phaseInterval = kPhaseInterval;

            if (attackPhase == AttackPhase.None || attackPhase == AttackPhase.Move)
            {
                int rand = Random.Range(0, 1000);
                if (rand < 500)
                {
                    attackPhase = AttackPhase.Shot;
                    shotInterval = kShotInterval;
                    shotCount = 0;
                }
                else
                {
                    attackPhase = AttackPhase.RushPrepare;
                    rushPrepareTime = kRushPrepareTime;
                }

                //attackFase = AttackFase.Shot;
                //shotInterval = kShotInterval;
                //shotCount = 0;

                GoNextPhase = false;
            }
        }

        if (GoNextPhase)
        {
            if (attackPhase == AttackPhase.Shot || attackPhase == AttackPhase.Rush)
            {
                attackPhase = AttackPhase.Move;
                moveTime = kMoveTime;

                GoNextPhase = false;
            }
        }
    }

    void Phase()
    {
        switch (attackPhase)
        {
            case AttackPhase.None:

                NonePhase();

                break;

            case AttackPhase.Move:

                MovePhase();

                break;

            case AttackPhase.Shot:

                ShotPhase();

                break;

            case AttackPhase.RushPrepare:

                RushPreparePhase();

                break;

            case AttackPhase.Rush:

                RushPhase();

                break;
        }
    }

    void RushPreparePhase()
    {
        rushPrepareTime -= Time.deltaTime;
        if (rushPrepareTime < 0)
        {
            attackPhase = AttackPhase.Rush;
            rushDirection = (player.transform.position - transform.position).normalized;
            rushDirection.y = 0;
            rushTime = kRushTime;
            rushSpeed = kRushSpeed;
        }
    }

    void RushPhase()
    {
        rushTime -= Time.deltaTime;
        if (rushTime < 0)
        {
            GoNextPhase = true;
        }

        velocity = rushDirection * rushSpeed;
        position += velocity * Time.deltaTime;
        transform.position = position;
    }

    void NonePhase()
    {
        noneTime -= Time.deltaTime;
        if (noneTime < 0)
        {
            GoNextPhase = true;
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

        if (other.tag == "Player")
        {
            PlayerScript player;
            if (other.TryGetComponent<PlayerScript>(out player))
            {
                player.KnockBackOn((player.transform.position - transform.position).normalized);
                attackPhase = AttackPhase.None;
                noneTime = kNoneTime;
            }
        }
    }

    void Rotate()
    {
        transform.rotation = Quaternion.LookRotation(velocity.normalized);
        if (attackPhase == AttackPhase.Shot)
        {
            transform.rotation = Quaternion.LookRotation(shotDirection.normalized);
        }
    }

    Vector3 RotateAroundAxis(Vector3 v, float angleDeg, Vector3 axis)
    {
        return Quaternion.AngleAxis(angleDeg, axis.normalized) * v;
    }
}
