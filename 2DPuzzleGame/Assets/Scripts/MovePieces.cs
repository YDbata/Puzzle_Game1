using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �������� ����
/// </summary>
public class MovePieces : MonoBehaviour
{
    public static MovePieces instance;
    Match3 game;

    NodePiece moving;
    Point newIndex;
    Vector2 mouseStart;

    private void Awake()
    {
        instance = this;
    }

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
            if(dir.magnitude > 32) // mouse�� ���������� 32�ȼ����� ������ �ִٸ�
            {
                // add�� ����� (1,0), (-1,0), (0,1), (0, -1) �߿���
                if(aDir.x > aDir.y)
                {
                    add = (new Point((nDir.x > 0) ? 1 : -1, 0));
                }else if(aDir.y > aDir.x)
                {
                    add = (new Point(0, (nDir.y > 0) ? 1 : -1));
                }
            }
            newIndex.add(add);

            // ���� position���� 16�ȼ���ŭ ������ �̵���Ű�� ���� pos���
            Vector2 pos = game.getPositionFromPoint(moving.index);
            if (!newIndex.Equals(moving.index))
            {
                pos += Point.mult(add, 16).ToVector();
            }

            moving.MovePositionTo(pos);
        }
    }

    /// <summary>
    /// touch�� ��� �����̵� ��� ����
    /// </summary>
    /// <param name="piece"></param>
    public void MovePiece(NodePiece piece)
    {
        // �̹� �����̴� piece�� ������ �ߴ�
        if (moving != null) return;
        // ���콺(��)�� �����ӵ��� piece��ġ�� ������� ����
        moving = piece;
        mouseStart = Input.mousePosition;
    }

    public void DropPiece()
    {
        // �����̴� piece�� ������ ��ȯ
        if (moving == null) return;
        Debug.Log("Drop");
        
        // ���ο� �ε����� ������ ����� index�� ���� ������ ������ ��ΰ� �ٲ�� ����� flip��Ŵ
        // ���� ��� piece�� ���� �ڸ��� �ű�


        moving = null;
    }
}
 