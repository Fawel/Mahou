using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class danmaku_fly : MonoBehaviour {
	public List<Point> pointarray;
	public	Point pn;
    public GameObject prefab;
	Vector3 pos; Vector3 direction; Vector3 mod_pos;
	public Point[] array;
	public float rot_x; public float rot_z;  float Rot_angle; public Vector3 previous_position;
	public float rot_angle
	{
		get{return Rot_angle;} set{Rot_angle = value * Mathf.Deg2Rad;}
	}
	public float Accel { get{return accel;} set{accel = value * Mathf.Deg2Rad;}}
//    public Vector3 previous_position { get { return pos; } set { pos = value; } }
//    public float rotation=0;
//    public float Rotation {
//        get { return rotation; } set { rotation = value; }
//    }
//    public float exist;
//    public float angle_speed=0;
//    public float Angle_speed
//    {
//        get { return angle_speed; }
//        set { angle_speed = value * Mathf.Deg2Rad; }
//    }
//    public bool done = false;
//    public float acceleration;
//    public float Acceleration { get { return acceleration; } set { acceleration = value; } }

	public float speed = 1f; public float accel=0f;
    void Start () 
	{
//		pointarray = new List<Point> (3);
//		Point a = new Point (1, 0, 0); Point b = new Point (1, 1, 0); Point c = new Point (2, 1, 0);
//		pointarray.Add(a); pointarray.Add(b); pointarray.Add(c);
//
//		pos = Vector3.zero;
		previous_position= transform.position;
        prefab = Resources.Load("Prefab/danmaku") as GameObject;
		Destroy(this.gameObject, 1.5f);
    }

    // Update is called once per frame
    void Update() 
	{
	//	pn=pn.transfer_points (pn, get_axis ("x"), get_axis ("z"), (Point)transform.position);
		transform.position = Vector3.Lerp (transform.position, (Vector3)pn, 0.5f);
			







//		if (Rot_angle != 0) {
//			direction.Set (0, 0, 0);
//			rot_z = Mathf.Sin (Rot_angle) * Mathf.Rad2Deg * 1;
//			rot_x = Mathf.Cos (Rot_angle) * Mathf.Rad2Deg * 1;
//			direction.z -= rot_z;
//			direction.x += rot_x;
//		}
//		else
//			direction.Set (1, 0, 0);
//		direction = transform.TransformDirection (direction).normalized;
//		pos = previous_position + direction*speed*Time.deltaTime;
//		transform.position = Vector3.Lerp(previous_position, pos,0.5f);
//		//transform.position += transform.TransformDirection(direction).normalized;
//		Rot_angle += accel; previous_position = transform.position;
    }
//	public Point get_axis(string a)
//	{
//		Vector3 axis=new Vector3();
//		switch(a)
//		{
//		case "x":
//			axis = transform.TransformDirection (new Vector3 (1, 0, 0)) - transform.position;
//			break;
//		case "z":
//			axis = transform.TransformDirection (new Vector3 (0, 0, 1)) - transform.position;
//			break;
//		default:
//			print ("Ось координат введена некорректно");
//			break;
//		}
//		return new Point (axis.x, axis.z,0);
//	}

    //Переводит в "локальные" координаты вектор "вперед" относительно локальной координаты префаба
    //direction = transform.TransformDirection(new Vector3(1f, 0f, 0f));

    //Двигаем в этом направлении
    //     transform.position += direction* Time.deltaTime * speed;



    //Движение по дуге
    // Для движения по дуге нужны данные об угловой скорости, пуля долна хранить текущий поворот, угловое ускорение. 
    // Дефолтный метод поворота в transform берет угол в радианах, необходимо перевести значения поворота и угловой скорости. Желательно менять сразу в свойствах get; set;.
    //  angle_speed = angle_speed * Mathf.Deg2Rad; 
    // В начале каждого фрейма поворачиваем пулю на угол
    //this.transform.Rotate(0, rotation, 0);
    // Переводим вектор движения вперед на вектор движения, иначе пуля будет двигаться вдоль оси Х независимо от поворота стреляющего. Нормализуемвектор для удобного изменеия скорости.
    //direction = transform.TransformDirection(pos).normalized;
    //Перемещаем пулю
    //transform.position += direction* Time.deltaTime * speed;
    //Увеличиваем поворот пули на значение угловой сокрости
    //rotation += angle_speed;

}
