using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public abstract class Danmaku : MonoBehaviour {
	public movement_type type=movement_type.simple;
    PlayerMovement pl;
    protected int damage;
    bool elegance_triggered = false;
    public float Speed;
    public float Acceleration;
	public float Max_rotation;
    public float Max_speed;
    public float Rotation;
    public float Angular_acceleration;
    public GameObject Bullet_prefab;
	// Лист точек по которым двигается пуля.
	protected List<Point> PointArray;

	public float[] Coordx, Coordz;

	public int arraycounter=0;

	/// <summary>
	/// Метод для обработки сталкнивания пуль с коллайдером Elegance.
	/// </summary>
	/// <param name="_collider">Collider.</param>
 	public	virtual void  OnTriggerEnter(Collider _collider)
    {
        if (_collider.gameObject.name == "Elegance_collider")
        {
            pl = _collider.transform.parent.GetComponent<PlayerMovement>();
            if (elegance_triggered == false)
            {
                pl.elegance_score++;
                elegance_triggered = true;
            }
        }
        else if (_collider.gameObject.name == "Collider")
        {
            pl = _collider.transform.parent.GetComponent<PlayerMovement>();
            pl.health -= damage;
            pl.elegance_score--;
            this.gameObject.SetActive(false);
        }
    }
	public virtual void OnTriggerStay(Collider _collider)
    {
        if (_collider.gameObject.name == "Collider")
        {
            pl = _collider.transform.parent.GetComponent<PlayerMovement>();
            pl.health -= damage;
            pl.elegance_score--;
            this.gameObject.SetActive(false);
        }
    }

	/// <summary>
	/// Присваивает значение поворота пули и переводит в градусы. 
	/// </summary>
	/// <value>The rotation.</value>
    public float _rotation
    {
    set { Rotation = value * Mathf.Deg2Rad; }
    }
	/// <summary>
	/// Присваивает значение углового ускорения и переводит в градусы.
	/// </summary>
	/// <value>The angular acceleration.</value>
    public float _angular_acceleration {
		set { Angular_acceleration = value * Mathf.Deg2Rad; }
    }
	public float _max_rotation
	{
		set { Max_rotation = value * Mathf.Deg2Rad; }
	}
	/// <summary>
	/// Создает из двух массивов единый лист точек.
	/// </summary>
	// По хорошему надо делать полную копию листа дабы избежать этого дурдома с массивами. 
	public void CreatePointList()
	{
		PointArray = new List<Point> (Coordx.Length);
		for (int i = 0; i < Coordx.Length; i++) 
		{
			PointArray.Add(new Point(Coordx[i], Coordz[i], transform.position.y));
		}
	}
}
