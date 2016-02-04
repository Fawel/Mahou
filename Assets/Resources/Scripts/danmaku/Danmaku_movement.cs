using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
// Список типов движения
public enum movement_type
{
    simple,
    second,
	points
}
public class Danmaku_movement : MonoBehaviour
{
	Vector3 direction; float rot_z, rot_x; Vector3 pos;
    Danmaku bullet;
    void Start() {
		bullet = GetComponent<Danmaku>();
    }
    public Vector3 movement_direction;
    
	public void Simple_Move(float _rotation, float _angular_acceleration, float _speed, float _acceleration, Vector3 _previous_position)
    {
		if (_rotation != 0) {
			direction.Set (0, 0, 0);
			rot_z = Mathf.Sin (_rotation) * Mathf.Rad2Deg * 1;
			rot_x = Mathf.Cos (_rotation) * Mathf.Rad2Deg * 1;
			direction.z -= rot_z;
			direction.x += rot_x;
		}
		else
			direction.Set (1, 0, 0);
		direction = transform.TransformDirection (direction).normalized;
		pos = _previous_position + direction*_speed;
		transform.position = Vector3.Lerp(_previous_position, pos,0.5f);
    }


	public void second_move (float _rotation, float _angular_acceleration, float _speed, float _acceleration, Vector3 _previous_position, float _max_rotation)
	{
		if (_max_rotation < 0) {
			if (_rotation <= _max_rotation)
				Simple_Move (_rotation, _angular_acceleration, _speed, _acceleration, _previous_position);
			else {
				direction.Set (1, 0, 0);
				direction = transform.TransformDirection (direction).normalized;
				pos = _previous_position + direction * _speed;
				transform.position = Vector3.Lerp (_previous_position, pos, 0.5f);
			}
		} else {
			if (_rotation >= _max_rotation)
				Simple_Move (_rotation, _angular_acceleration, _speed, _acceleration, _previous_position);
			else {
				direction.Set (1, 0, 0);
				direction = transform.TransformDirection (direction).normalized;
				pos = _previous_position + direction * _speed;
				transform.position = Vector3.Lerp (_previous_position, pos, 0.5f);
			}
		}
	}

	public void point_move(Point _Point, float _speed, float _acceleration)
	{
			movement_direction =(Vector3) _Point;
		transform.position = Vector3.Lerp (transform.position, (Vector3)_Point, 0.5f);
		if (transform.position == (Vector3)_Point) 
		{ 
			bullet.arraycounter++;	
		}
	}
}
	/// <summary>
	/// Класс точек, по которым перемещается пуля.
	/// </summary>
public class Point
{
	float x, z, y;
	public float get_z	{get {return z;}}
	public float get_y	{get {return y;}}
	public float get_x	{get {return x;}}

	public Point(float _x, float _z, float _y)
	{
		x=_x;
		z=_z;
		y = _y;
	}

	//Для конвертации класса точек в вектор и наоборот.
	public static explicit operator Vector3(Point _point)
	{
		return new Vector3 (_point.x, _point.y, _point.z);
	}
	public static explicit operator Point(Vector3 _vector)
	{
		return new Point (_vector.x, _vector.z, _vector.y);
	}
}
