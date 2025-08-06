using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    private LineRenderer lineRend;

    private float lineLength;
    private EffectArea effectArea;


    private void Start()
    {
        effectArea = GetComponentInChildren<EffectArea>();
        lineRend = GetComponent<LineRenderer>();
    }

    public void PrepEffectArea(Vector2[] positions)
    {
        lineLength = GetLength();
        effectArea.SetArea(positions, lineLength);
        lineRend.enabled = false;
        GetComponent<EdgeCollider2D>().enabled = false;
    }

    float GetLength()
    {
        float length = 0;
        Vector3[] positionHolder = new Vector3[lineRend.positionCount];
        lineRend.GetPositions(positionHolder);
        for (int i = 1; i < positionHolder.Length; i++)
        {
            length += Vector3.Distance(positionHolder[i], positionHolder[i - 1]);
        }
        return length;
    }
}
