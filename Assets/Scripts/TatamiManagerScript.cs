using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TatamiManagerScript : MonoBehaviour
{
    TatamiScript[] tatamis;
    int tatamiCount;

    // Start is called before the first frame update
    void Start()
    {
        tatamis = FindObjectsByType<TatamiScript>(FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < tatamis.Length; i++)
        {
            if (tatamis[i].IsColored)
            {
                tatamiCount++;
            }
        }

        Debug.Log("tatamiCount = " + tatamiCount);
    }

    private void LateUpdate()
    {
        tatamiCount = 0;
    }
}
