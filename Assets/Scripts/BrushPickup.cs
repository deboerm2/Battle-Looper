using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushPickup : MonoBehaviour
{
    public GameObject brushPrefab;

    private void OnMouseOver()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            DrawingManager.instance.loadedBrushes.Enqueue(Instantiate(brushPrefab));
            GameManager.instance.RemovePickup(gameObject);
            Destroy(gameObject);
        }
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            DrawingManager.instance.loadedBrushes.Enqueue(Instantiate(brushPrefab));
            GameManager.instance.RemovePickup(gameObject);
            Destroy(gameObject);
        }
    }
}
