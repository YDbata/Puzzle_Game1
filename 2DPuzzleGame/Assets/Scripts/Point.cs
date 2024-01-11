using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Point
{
    public int x;
    public int y;

    public Point(int nx, int ny)
    {
        x = nx;
        y = ny;
    }

    public void add(Point p)
    {
        x += p.x;
        y += p.y;
    }

    public void mult(int m)
    {
        x *= m;
        y *= m;
    }



    public Vector2 ToVector()
    {
        return new Vector2(x, y);
    }

    // 같은 지점인지 확인하기 위해 만듬
    public bool Equals(Point p)
    {
        return (x == p.x && y == p.y);
    }

    public static Point fromVector(Vector2 v)
    {
        return new Point((int)v.x, (int)v.y);
    }

    public static Point fromVector(Vector3 v)
    {
        return new Point((int)v.x, (int)v.y);
    }

    public static Point add(Point p, Point o)
    {
        return new Point(p.x + o.x, p.y + o.y);
    }

    public static Point mult(Point p, int m)
    {
        return new Point(p.x * m, p.y * m);
    }

    // 같은 주소를 참조하지 않도록 Point를 복사하는 함수
    public static Point clone(Point p)
    {
        return new Point(p.x, p.y);
    }
    public static Point zero
    {
        get { return new Point(0, 0); }
    }
    public static Point one
    {
        get { return new Point(1, 1); }
    }
    public static Point Up
    {
        get { return new Point(0, 1); }
    }
    public static Point Down
    {
        get { return new Point(0, -1); }
    }
    public static Point Right
    {
        get { return new Point(1, 0); }
    }
    public static Point Left
    {
        get { return new Point(-1, 0); }
    }
}
