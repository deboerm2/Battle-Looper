using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingManager : MonoBehaviour
{
    public static DrawingManager instance;

    public Queue<GameObject> loadedBrushes;

    Camera mainCam;
    LineRenderer currentBrush;
    EdgeCollider2D edgeCol;
    int simplifyCounter;


    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
            instance = this;

        loadedBrushes = new Queue<GameObject>();
        mainCam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && loadedBrushes.Count != 0)
        {
            StartBrush();
        }
        
        if (Input.GetKeyUp(KeyCode.Mouse0) && currentBrush != null)
        {
            LoopBrush();
        }
    }
    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0) && currentBrush != null)
        {
            Drawing();
        }
    }

    void StartBrush()
    {
        currentBrush = loadedBrushes.Dequeue().GetComponent<LineRenderer>();
        edgeCol = currentBrush.GetComponent<EdgeCollider2D>();
    }

    void Drawing()
    {
        List<Vector2> edges = new List<Vector2>();

        //if mouse position changed, make a new point
        if (currentBrush.positionCount ==0 || mainCam.ScreenToWorldPoint(Input.mousePosition) != currentBrush.GetPosition(currentBrush.positionCount - 1))
        { 
            currentBrush.positionCount++;
            
            currentBrush.SetPosition(currentBrush.positionCount - 1,
                currentBrush.gameObject.transform.InverseTransformPoint(mainCam.ScreenToWorldPoint(Input.mousePosition)));

            for(int i = 2; i < currentBrush.positionCount-1; i++)
            {
                edges.Add(currentBrush.GetPosition(i-2));
            }
            edgeCol.SetPoints(edges);
            
            
            simplifyCounter++;
            if (simplifyCounter > 1)
            {
                currentBrush.Simplify(0.01f);
            }
            DetectLoop();
        }
        
    }

    void LoopBrush()
    {
        currentBrush.GetComponent<Brush>().PrepEffectArea(edgeCol.points);
        currentBrush.loop = true;
        simplifyCounter = 0;
        currentBrush = null;
    }

    void DetectLoop()
    {
        if (currentBrush.positionCount > 2)
        {
            //raycast from mouse to previous point
            RaycastHit2D hit2D = Physics2D.Raycast(currentBrush.GetPosition(currentBrush.positionCount - 1),
                    currentBrush.GetPosition(currentBrush.positionCount - 1) - currentBrush.GetPosition(currentBrush.positionCount - 2),
                    Vector2.Distance(currentBrush.GetPosition(currentBrush.positionCount - 1), currentBrush.GetPosition(currentBrush.positionCount - 2)));

            if ( hit2D && hit2D.collider.gameObject == currentBrush.gameObject)
            {
                LoopDetected();
            }
        }
    }
    void LoopDetected()
    {
        //start code on line to spawn effect and delete itself
        currentBrush.GetComponent<Brush>().PrepEffectArea(edgeCol.points);
        simplifyCounter = 0;
        currentBrush = null;
    }
}
