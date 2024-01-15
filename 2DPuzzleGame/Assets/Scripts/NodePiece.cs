using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public int value;
    public Point index;

    [HideInInspector]
    public Vector2 pos;

    [HideInInspector]
    public RectTransform rect;

    bool updating;
    Image img;

    public void Initialize(int v, Point p, Sprite piece)
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        value = v;
        SetIndex(p);
        img.sprite = piece;
    }

    public void SetIndex(Point p)
    {
        index = p;
        ResetPosition();
        UpdateName();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    public void MovePosition(Vector2 move)
    {
        rect.anchoredPosition += move * Time.deltaTime*16f;
    }

    /// <summary>
    /// Node의 position을 이동시킨다. 0110 새벽
    /// </summary>
    /// <param name="pos"></param>
    public void MovePositionTo(Vector2 move)
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, move, Time.deltaTime * 16f);
    }

    public void ResetPosition()
    {
        pos = new Vector2(32 + (64 * index.x), -32 - (64 * index.y));
    }

    void UpdateName()
    {
        transform.name = "Node [" + index.x + ", " + index.y + "]";
    }

    public bool UpdatePiece()
    {
        return true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (updating) return;
        //Debug.Log("Grap :" + transform.name);
        MovePieces.instance.MovePiece(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (updating) return;
        //Debug.Log("Drop :" + transform.name);
        MovePieces.instance.DropPiece();
    }
}
