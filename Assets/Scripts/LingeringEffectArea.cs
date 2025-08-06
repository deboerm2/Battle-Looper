using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LingeringEffectArea : EffectArea
{
    //the same as base except for areaDuration = effectDuration;
    public override void SetArea(Vector2[] positions, float lineLength)
    {
        for (int i = 0; i < lineLength / 10; i++)
        {
            //exponentially lose duration based on length
            effectDuration *= Mathf.Pow(effect.durationMult, i);
        }
        areaCol.points = positions;
        areaDuration = effectDuration;

        mesh = areaCol.CreateMesh(true, true);
        GetComponent<MeshFilter>().mesh = mesh;
        StartCoroutine(Lifetime(areaDuration));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //empty so it ignores EffectArea OnTriggerEnter2D
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EntityStats>())
        {
            collision.gameObject.GetComponent<EntityStats>().ApplyEffect(effect, 0.1f);
        }
    }
}
