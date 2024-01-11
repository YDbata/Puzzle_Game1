using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3 : MonoBehaviour
{
    // unity�� üũ ǥ�÷� ���δ�. ���� ���⼭ üũ�� �ϸ� ������ �ǰ� �Ұ��̴�.
    public ArrayLayout boardLayout;

    [Header("UI Elements")]
    public Sprite[] pieces;
    public RectTransform gameBoard;


    [Header("Prefabs")]
    public GameObject nodePiece;

    int width = 9;
    int height = 14;
    Node[,] board;

    System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        
        string seed = getRandomSeed();
        random = new System.Random(seed.GetHashCode());

        InitializeBoard();
        VerifyBoard();
        InstantiateBoard();

    }

    void InitializeBoard()
    {
        board = new Node[width, height];
        for(int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // ���巹�̾ƿ��� üũ�� �Ǿ������� -1, �ƴϸ� ����� ä���� ��ȯ�Ѵ�.
                board[x, y] = new Node((boardLayout.rows[y].row[x])?-1 : fillPiece(), new Point(x, y));
            }
        }
    }

    /// <summary>
    /// ó�����۽� 3���̻� ���ӵ� ������ ���� �����ϴ� �Լ�
    /// </summary>
    void VerifyBoard()
    {
        List<int> remove;
        for(int x = 0;x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Point p = new Point(x, y);
                int val = getValueAtPoint(p);
                // ������ ��� �Ѿ
                if (val <= 0) continue;

                remove = new List<int>();
                while(isConnected(p, true).Count > 0)
                {
                    val = getValueAtPoint(p);
                    if (!remove.Contains(val))
                        remove.Add(val);
                    setValueAtPoint(p, newValue(ref remove));
                }
            }

        }
    }

    void InstantiateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int val = board[x, y].value;
                if (val <= 0) continue;
                GameObject p = Instantiate(nodePiece, gameBoard);
                NodePiece node = p.GetComponent<NodePiece>();
                RectTransform rect = p.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(32 + (64 * x), -32 - (64 * y));
                node.Initialize(val, new Point(x, y),pieces[val - 1]);
                   
            }
        }
    }

    /// <summary>
    /// �� ��ġ���� Match�� �Ͼ���� Ȯ��
    /// </summary>
    /// <param name="p"></param>
    /// <param name="main"></param>
    /// <returns></returns>
    List<Point> isConnected(Point p, bool main)
    {
        List<Point> connected = new List<Point>();
        int val = getValueAtPoint(p);
        Point[] directions =
        {
            Point.Up,
            Point.Right,
            Point.Down,
            Point.Left
        };
        
        // ���� ���⿡ 2�� �̻��� ���� ����� �ִ��� Ȯ���ϴ� �ݺ���(0X000X0)�ÿ��� Ȯ�� �����ϵ��� 
        foreach(Point dir in directions)
        {
            List<Point> line = new List<Point>();

            int same = 0;
            for (int i = 1; i < 3; i++)
            {
                Point check = Point.add(p, Point.mult(dir, i));
                if(getValueAtPoint(check) == val)
                {
                    line.Add(check);
                    ++same;
                }
            }

            // 1���̻��� ����� ���ٸ�츮�� ��ġ�ΰ��� Ȯ���մϴ�.
            if(same > 1)
            {
                AddPoints(ref connected, line);
            }
        }


        // �߰��� �ִ� ������� Ȯ��
        for (int i = 0;i < 2; i++)
        {
            List<Point> line = new List<Point>();

            int same = 0;
            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[i + 2])};
            foreach(Point next in check) { 
                if(getValueAtPoint(next) == val)
                {
                    line.Add(next);
                    same++;
                }
            
            }

            if (same > 1)
                AddPoints(ref connected, line);

        }

        // check for 2x2
        for(int i = 0;i < 4; i++)
        {
            List<Point> square = new List<Point>();
            int same = 0;
            int next = i + 1;

            if (next >= 4)
                next -= 4;

            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[next]), Point.add(p, Point.add(directions[i], directions[next])) };
            foreach (Point pnt in check)
            {
                if (getValueAtPoint(pnt) == val)
                {
                    square.Add(pnt);
                    same++;
                }

            }

            if (same > 2)
                AddPoints(ref connected, square);
        }

        if (main) // 2���̻��� ��ġ�� ���� �ִ��� Ȯ��
        {
            for(int i = 0; i < connected.Count; i++)
            {
                AddPoints(ref connected, isConnected(connected[i], false));
            }
        }

        if(connected.Count > 0)
        {
            connected.Add(p);
        }

        return connected;
    }

    /// <summary>
    /// points�� add�� ������ �߰�
    /// </summary>
    /// <param name="points"></param>
    /// <param name="add"></param>
    void AddPoints(ref List<Point> points, List<Point> add)
    {
        foreach(Point p in add)
        {
            bool doAdd = true;
            
            for(int i =0;i < points.Count;i++)
            {
                if (points[i].Equals(p))
                {
                    doAdd = false;
                    break;
                }
            }

            if (doAdd) points.Add(p);
        }
    }

    /// <summary>
    /// ������� �������� ���ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    int fillPiece()
    {
        int val = 1;
        val = (random.Next(0, 100) / (100 / pieces.Length)) + 1;
        return val;
    }

    /// <summary>
    /// Board���� Point��ġ�� �Ҵ�� ���� ��ȯ
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    int getValueAtPoint(Point p)
    {
        if (p.x < 0 || p.x >= width || p.y < 0 || p.y >= height) return -1;
        return board[p.x, p.y].value;
    }

    /// <summary>
    /// ����Ʈ�� �־��� board ��ġ�� v�� �Ҵ�
    /// </summary>
    /// <param name="p"></param>
    /// <param name="v"></param>
    void setValueAtPoint(Point p, int v)
    {
        board[p.x, p.y].value = v;

    }

    /// <summary>
    /// ���� ��ü�� �� ���� ����� �����ϰ� �����Ͽ� ��ȯ��
    /// </summary>
    /// <param name="remove"></param>
    /// <returns></returns>
    int newValue(ref List<int> remove)
    {
        List<int> available = new List<int>();
        for(int i = 0;i < pieces.Length; i++)
        {
            available.Add(i + 1);
        }

        foreach(int i in remove)
        {
            available.Remove(i);
        }

        if (available.Count <= 0) return 0;
        return available[random.Next(0, available.Count)];
    }

    /// <summary>
    /// ���� �õ带 ��� �Լ�
    /// </summary>
    /// <returns></returns>
    private string getRandomSeed()
    {
        string seed = "";
        string acceptableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()";
        for (int i = 0; i < 20; i++)
            seed += acceptableChars[Random.Range(0, acceptableChars.Length)];
        return seed;
    }

    /// <summary>
    /// Point�� Position�� Vector2�� ��ȯ
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public Vector2 getPositionFromPoint(Point p)
    {
        return new Vector2(32 + (64 * p.x), -32 - (64 * p.y));
    }

}



[System.Serializable]
/// �� ���� ����
public class Node
{
    public int value; // 0 = blank, 1 = cube, 2 = sphere, 3 = cylinder, 4 = pryamid, 5 = diamond, -1 = hole
    public Point index;

    public Node(int v, Point i)
    {
        value = v;
        index = i;
    }
}