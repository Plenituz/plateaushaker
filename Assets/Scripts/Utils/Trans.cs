using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Trans : MonoBehaviour {
    public Vector2 size;
    public Vector2 pos;
    public int ZOrder = 0;
    private bool updateSize = true;
    private bool updatePos = true;
    public bool ignorePos = false;
    public bool ignoreScale = false;
    public bool squareOff = false;
    public bool forceUpdate = false;

    public void SetPos(Vector2 vec)
    {
        pos = vec;
        updatePos = true;
    }

    public void SetSize(Vector2 vec)
    {
        size = vec;
        updateSize = true;
    }

    public void ForceUpdatePos()
    {
        float x = P.pocP(pos.x, Side.W);
        float y = P.pocP(pos.y, Side.H);
        transform.localPosition = new Vector2(x, y);
    }

    void Awake()
    {
        P.init();
        updateSize = true;
        updatePos = true;
    }

    void Update()
    {
        if (!Application.isPlaying || forceUpdate)
        {
            updateSize = true;
            updatePos = true;
        }
        if (updateSize && !ignoreScale)
        {
            if (!Application.isPlaying)
            {
                float width = P.pocSEditor(size.x, Side.W);
                float height = P.pocSEditor(size.y, Side.H) * (squareOff ? Camera.main.aspect : 1);
                transform.localScale = new Vector3(width, height, 1f);
            }
            else
            {
                float width = P.pocSGame(size.x, Side.W);
                float height = P.pocSGame(size.y, Side.H) * (squareOff ? Camera.main.aspect : 1);
                transform.localScale = new Vector2(width, height);
            }
            updateSize = false;
        }
        if (updatePos && !ignorePos)
        {
            float x = P.pocP(pos.x, Side.W);
            float y = P.pocP(pos.y, Side.H);
            transform.localPosition = new Vector3(x, y, ZOrder);
            updatePos = false;
        }
    }
}
