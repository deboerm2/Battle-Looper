using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectArea : MonoBehaviour
{
    public Effects effect;
    protected float effectDuration;
    protected PolygonCollider2D areaCol;
    protected float areaDuration = 0.5f;
    protected Mesh mesh;


    // Start is called before the first frame update
    void Start()
    {
        areaCol = GetComponentInChildren<PolygonCollider2D>();
        effectDuration = effect.duration;
    }

    public virtual void SetArea(Vector2[] positions, float lineLength)
    {
        for(int i = 0; i < lineLength / 10; i++)
        {
            //exponentially lose duration based on length
            effectDuration *= Mathf.Pow(effect.durationMult, i);
        }
        areaCol.points = positions;

        mesh = areaCol.CreateMesh(true, true);
        GetComponent<MeshFilter>().mesh = mesh;
        StartCoroutine(Lifetime(areaDuration));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<EntityStats>())
        {
            collision.gameObject.GetComponent<EntityStats>().ApplyEffect(effect, effectDuration);
        }
    }

    protected IEnumerator Lifetime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(transform.parent.gameObject);
    }


}
