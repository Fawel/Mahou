using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum danmaku_types
{
	Bullet
}
public class FireData : MonoBehaviour {
    public GameObject shootung_object;
    /// <summary>
    /// Простейший метод создания пули
    /// </summary>
    /// <param name="_sum">Число выпускаемых пуль</param>
    /// <param name="_reload_time">Задержка между выстрелами</param>
    /// <param name="_rotation">Угол на который повернута пуля при инициализации</param>
    /// <param name="_danmaku_angular_acceleration">Угловая скорость пули</param>
    /// <param name="_init_rotation_speed">Угловая разница для создания следующей пули</param>
    /// <param name="_type">Тип пули</param>
	/// <param name="_danmaku_move_params">Параметры движения пули (зависят от типа движения)</param>
	/// <param name="movement">Тип движения пули.</param>
	/// <param name="_bullet_params">Параметры пули.</param>
    /// <returns></returns>

	public IEnumerator move(int _sum, float _reload_time,
		float _rotation, float _init_rotation_speed, danmaku_types _type, float[] _danmaku_move_params,float [] _bullet_params, movement_type movement)
	{
		// При инициализации пули, её экземпляр класса принимает null.
		// Поэтому создаем два экземпляра - первый хранит тип пули, второй используется для инициализации пули
		// и копирует тип из первого экземпляра
		Danmaku create_new = set_danmaku_type (_type); 
		Danmaku clone_bullet;
		for (float timer = 0; timer < _sum; timer += 1) 
        {
			clone_bullet = create_new;
			set_move_params (clone_bullet,_danmaku_move_params, movement);
			set_bul_params (_bullet_params, _type);
			clone_bullet = Instantiate(clone_bullet.transform, transform.position, Quaternion.Euler(0,transform.rotation.eulerAngles.y+_rotation, 0)) as Danmaku;
            _rotation += _init_rotation_speed;
            if (_reload_time > 0)
            {
                yield return new WaitForSeconds(_reload_time);
            }
        }
	}
	/// <summary>
	/// Перемещаем пулю движением по заранее заданным точкам.
	/// </summary>
	/// <returns></returns>
	/// <param name="_sum">Число пуль</param>
	/// <param name="reload_time">Задержка между выстрелами</param>
	/// <param name="_rotation">Угол на который повернута пуля при инициализации. Используется при смещении массива пуль.</param>
	/// <param name="_PointArray">Массив точек, по которым будет двигаться пуля.</param>
	public IEnumerator point_move(float _sum, float _reload_time, float _rotation, float _init_rotation_speed, danmaku_types _type, List<Point> _PointArray, float [] _bullet_params)
	{
		Danmaku create_new = set_danmaku_type (_type); 
		Danmaku clone_bullet;
		for (float timer = 0; timer < _sum; timer += 1) 
		{
			clone_bullet = create_new;
			clone_bullet.type = movement_type.points;
			set_move_params (clone_bullet, _bullet_params, rotate_axis(_PointArray));
			set_bul_params (_bullet_params, _type);

			clone_bullet = Instantiate(clone_bullet.transform, transform.position, Quaternion.Euler(0, shootung_object.transform.rotation.eulerAngles.y+_rotation, 0)) as Danmaku;
			_rotation += _init_rotation_speed;
			if (_reload_time > 0)
			{
				yield return new WaitForSeconds(_reload_time);
			}
		}
	}

	/// <summary>
	/// Задает параметры движения пули в зависимости от типа движения
	/// </summary>
	/// <param name="_params">Массив значений.</param>
	/// <param name="_type">Тип движения пули.</param>
	void set_move_params(Danmaku _bullet, float [] _params, movement_type _type)
	{
		_bullet.type = _type;
		switch (_type) 
		{
		// Порядок значения в массиве и тип движения определяют за что отвечает каждая цифра в массиве параметров.
		// Неудобно, нет подсказки при вводе.
		case movement_type.simple:

			_bullet.Speed = _params [0];
			_bullet.Acceleration = _params [1]; 
			_bullet._rotation = _params [2];
			_bullet._angular_acceleration = _params [3];
			break; 
		case movement_type.second:
			_bullet.Speed = _params [0];
			_bullet.Acceleration = _params [1]; 
			_bullet._rotation = _params [2];
			_bullet._angular_acceleration = _params [3];
			_bullet._max_rotation = _params [4];
			break;
		default:
			print ("Неправильно задан тип движения");
			break;
		}
	}

	// Задает параметры движения для типа движения "по точкам".
	void set_move_params(Danmaku _bullet, float [] _params, List<Point> _array)
	{
		_bullet.Speed = _params [0];
		_bullet.Acceleration = _params [1];
		// Здесь нужно сделать deep copy листа в класс пули. Не смог, поставил массивы скалярых величин.
		_bullet.Coordx=new float[_array.Count]; _bullet.Coordz=new float[_array.Count];
		for (int i = 0; i < _array.Count; i++) 
		{
			_bullet.Coordx [i] = _array [i].get_x;
			_bullet.Coordz [i] = _array [i].get_z;
		}
		_bullet._rotation = 0;
		_bullet._angular_acceleration = 0;
	}
	/// <summary>
	/// Получаем класс пули, которой стреляем.
	/// </summary>
	/// <returns>Возвращает класс пули.</returns>
	Danmaku set_danmaku_type(danmaku_types _type)
	{
		Danmaku _bullet=null;
		switch (_type) 
		{
		//Для оптимизации стоит хранить такие вещи в object pool, а не грузить каждый раз.
		case danmaku_types.Bullet:
			GameObject bullet_obj = Resources.Load ("Prefab/Bullet")as GameObject;
			_bullet = bullet_obj.GetComponent<Bullet> ();
			break;
		default:
			print ("Неправильно задан тип пули");
			break;
		}
		return _bullet;
	}
	/// <summary>
	/// Задает параметры пули.
	/// </summary>
	/// <param name="_bul_params">Массив параметров.</param>
	/// <param name="_type">Класс пули.</param>
	void set_bul_params(float[]_bul_params, danmaku_types _type)
	{
		switch (_type) 
		{
		case danmaku_types.Bullet:
			print ("Set bullet params");
			break;
		default:
			break;
		}
			
	}

	/// <summary>
	/// Поворот осей координат при типе движения "по отчкам".
	/// </summary>
	/// <returns>Возвращает список точек с учетом поворота осей и положения стрелка.</returns>
	/// <param name="_PointList">Исходный список точек, заданный относительно мирового центра координат.</param>
	List<Point> rotate_axis(List<Point> _PointList)
	{
		List<Point> new_list = new List<Point> (_PointList.Count);
		//Получаем координаты точек. В юнити градусы увеличиваются при движении по часовой стрелке.
		float rotation = (360-shootung_object.transform.rotation.eulerAngles.y) * Mathf.Deg2Rad, rotation_x = Mathf.Deg2Rad* (360 - shootung_object.transform.rotation.eulerAngles.y+90);
		Point get_axis_z = new Point((float)Math.Cos(rotation),(float)Math.Sin(rotation), transform.position.y), 
		get_axis_x = new Point((float)Math.Cos(rotation_x), (float)Math.Sin(rotation_x), transform.position.y);
		// Смещаем точки с учетом поворота осей и положения стрелка.
		foreach (Point points in _PointList) 
		{
			
			new_list.Add (new Point ((points.get_x * get_axis_z.get_x + points.get_z * get_axis_x.get_x)+transform.position.x, 
				(points.get_x * get_axis_z.get_z + points.get_z * get_axis_x.get_z)+transform.position.z,transform.position.y));

		}
		return new_list;
	}
}
