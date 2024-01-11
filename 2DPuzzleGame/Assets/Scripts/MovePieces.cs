using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 움직임을 추적
/// </summary>
public class MovePieces : MonoBehaviour
{
    public static MovePieces instance;
    Match3 game;

    NodePiece moving;
    Point newIndex;
    Vector2 mouseStart;

    // Start is called before the first frame update
    void Start()
    {
        game = GetComponent<Match3>();
    }

    // Update is called once per frame
    void Update()
    {
        if(moving != null)
        {
            Vector2 dir = ((Vector2)Input.mousePosition - mouseStart);
            Vector2 nDir = dir.normalized;
            Vector2 aDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));

            newIndex = Point.clone(moving.index);
            Point add = Point.zero;
            if(dir.magnitude > 32) // mouse의 시작점에서 32픽셀보다 떨어져 있다면
            {
                // add를 만든다 (1,0), (-1,0), (0,1), (0, -1) 중에서
                if(aDir.x > aDir.y)
                {
                    add = (new Point((nDir.x > 0) ? 1 : -1, 0));
                }else if(aDir.y > aDir.x)
                {
                    add = (new Point(0, (nDir.y > 0) ? 1 : -1));
                }
            }
            newIndex.add(add);

            // 원래 position에서 16픽셀만큼 조각을 이동시키기 위한 pos계산
            Vector2 pos = game.getPositionFromPoint(moving.index);
            if (!newIndex.Equals(moving.index))
            {
                pos += Point.mult(new Point(add.x, -add.y), 16).ToVector();
            }

            moving.MovePositionTo(pos);
        }
    }

    public void MovePiece(Point index)
    {
        if (moving != null) return;
        moving = 
    }
}
