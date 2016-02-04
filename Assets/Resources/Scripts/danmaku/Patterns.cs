using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
[RequireComponent(typeof(FireData))]
[RequireComponent(typeof(Danmaku_movement))]
public class Patterns : MonoBehaviour {
  FireData fire_data;

	// Use this for initialization
	void Start () {

		fire_data = this.gameObject.GetComponent<FireData>();
	}
	// Update is called once per frame
  void Update () { 
	}
	// Пример паттерна.
	public IEnumerator pattern_1()
    {
		shoot_move (fire_data,10, 0.1f, 0, 0, danmaku_types.Bullet, new float[4]{ 0.2f, 0.001f, 0f, 0f}, new float[]{ 1 }, movement_type.simple);
		yield return new WaitForSeconds(0.1f);
		shoot_move (fire_data,10,0.1f,0,0, danmaku_types.Bullet, new float[5]{0.2f,0.001f,-45f,-10f,-90f},new float[]{1},movement_type.second);
    }
	// Пример паттерна движения по точкам.
	public IEnumerator pattern_2()
	{
		// Лист впихнут исключительно для тестов.
		List<Point> array = new List<Point> (3);
		array.Add (new Point (1, 0, transform.position.y));
		array.Add (new Point (1, 1, transform.position.y));
		array.Add (new Point (1, 2, transform.position.y));
		shoot_move (fire_data, 1, 0.1f, 0, 0, danmaku_types.Bullet, array, new float[]{ 5,1 });
		yield break;
	}
	// Эти методы нужны, чтобы с помощью WaitForSeconds можно было делать задержку между выстрелами.
	// Задержка происходит между НАЧАЛАМИ залпов.
	void shoot_move(FireData _data, int _time, float _reload_time,
		float _rotation, float _init_rotation_speed, danmaku_types _type, float[] _danmaku_move_params,float [] _bullet_params, movement_type movement)
	{
 		StartCoroutine(_data.move(_time,  _reload_time, _rotation,  _init_rotation_speed,  _type, _danmaku_move_params,_bullet_params, movement));
	}
	void shoot_move(FireData _data, int _sum, float _reload_time, float _rotation, float _init_rotation_speed, danmaku_types _type, List<Point> _PointArray, float [] _bullet_params)
	{
		StartCoroutine (_data.point_move(_sum, _reload_time, _rotation, _init_rotation_speed, _type, _PointArray, _bullet_params));
	}
}
