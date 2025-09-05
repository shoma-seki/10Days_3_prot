using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPScript : MonoBehaviour
{
    BossScript boss;
    int maxHP;
    int HP;

    Image image;

    // Start is called before the first frame update
    void Start()
    {
        boss = FindAnyObjectByType<BossScript>();
        HP = boss.HP;
        maxHP = HP;

        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        HP = boss.HP;
        image.fillAmount = (float)HP / (float)maxHP;
    }
}
