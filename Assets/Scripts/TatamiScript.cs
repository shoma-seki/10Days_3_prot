using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TatamiScript : MonoBehaviour
{
    [SerializeField] int num;
    public int Num { get { return num; } }

    bool isColored;
    public bool IsColored { get { return isColored; } set { isColored = value; } }

    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isColored)
        {
            meshRenderer.material.color = Color.red;
        }
        else
        {
            meshRenderer.material.color = Color.white;
        }
    }

    private void LateUpdate()
    {
        //isColored = false;
    }
}
