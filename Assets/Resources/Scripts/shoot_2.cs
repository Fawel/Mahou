using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class shoot_2 : MonoBehaviour {
	public get_rotation gr;
	private GameObject danmaku;
	public float parent_rotation;
	public danmaku_fly bullet_class;
	public List<Point> pointarray;
	public Point a;
	// Use this for initialization
	void Start () {
	//	pointarray = new List<Point> (3);
	//	a = new Point (1, 0, 0); Point b = new Point (1, 1, 0); Point c = new Point (2, 1, 0);
//		pointarray.Add(a); pointarray.Add(b); pointarray.Add(c);

		danmaku = bullet_class.prefab;
		parent_rotation = gr.rotaion;
		//bullet_class = GetComponent<danmaku_fly>();

		StartCoroutine(StartBulletPattern());
	}

	
	// Update is called once per frame
	void Update () {
		parent_rotation = gr.rotaion;    
	}

	IEnumerator shoot_type_1(int sum, float reload_time, float spread, float speed, float volly, float rot, float accel, Transform ammo)
	{ 
		float angle = this.transform.eulerAngles.y;
		bullet_class.rot_angle = rot;
		bullet_class.speed = speed;
		bullet_class.Accel = accel;
		if (sum <= 0) {
			Instantiate (ammo, transform.position, Quaternion.Euler (0, 0, 0));
		} else {
			while (volly > 0) {
				for (int i = 0; i < sum; i++) {
					Instantiate (ammo, transform.position, Quaternion.Euler (0, angle, 0));
					angle += spread;
					if (reload_time > 0)
						yield return new WaitForSeconds (reload_time);
				}
				angle = this.transform.eulerAngles.y;
				volly--;
			}

		}
	}

	IEnumerator shoot_type_2(int sum, float reload_time, float volly, Transform ammo)
	    {
	        float angle = this.transform.eulerAngles.y;
	        if(sum ==0)
	        {
	            Instantiate(ammo, transform.position, Quaternion.Euler(0, 0, 0));
	        }
	        else
	        {
	            while (volly > 0)
	            {
	                for (int i = 0; i < sum; i++)
	                {
	                    Instantiate(ammo, transform.position, Quaternion.Euler(0, angle, 0));
	                    angle += 360 / sum;
	                }
	                volly--;
	                angle = this.transform.eulerAngles.y;
	                yield return new WaitForSeconds(reload_time);
	            }
	        }
	    } // круговая стрельба

	IEnumerator shoot_type_3(int sum, float reload_time, List<Point> array,  Transform ammo)
	{
		float angle = this.transform.eulerAngles.y;
		if(sum ==0)
		{
			Instantiate(ammo, transform.position, Quaternion.Euler(0, 0, 0));
		}
		else
		{
				for (int i = 0; i < sum; i++)
				{
				bullet_class.pn = a;
					Instantiate(ammo, transform.position, Quaternion.Euler(0, angle, 0));
				}
				yield return new WaitForSeconds(reload_time);
		}
	}


	IEnumerator StartBulletPattern()
	{
//		StartCoroutine (shoot_type_1 (10, 0.1f, 0, 5, 1, 45, 0, danmaku.transform));
//		StartCoroutine(shoot_type_2(10, 0.1f, 1, danmaku.transform));
		StartCoroutine(shoot_type_3(3,0.1f,pointarray,danmaku.transform));
		yield return new WaitForSeconds(3.0f);
		StartCoroutine(StartBulletPattern());
	}
}

//public class Point
//{
//	float x, z, y;
//	public Point(float _x, float _z, float _y)
//	{
//		x=_x;
//		z=_z;
//		y = _y;
//	}
//
//	public Point transfer_points(Point _point, Point _axis_x, Point _axis_z, Point _offset)
//	{
//		//Point _oldpoint = _point;
//		_point.x = _point.x * _axis_x.x + _point.x * _axis_z.x + _offset.x;
//		_point.z = _point.z * _axis_x.z + _point.z * _axis_z.z+_offset.z;
//		_point.y = _offset.y;
//		return _point;
//	}
//	public static explicit operator Vector3(Point _point)
//	{
//		return new Vector3 (_point.x, _point.y, _point.z);
//	}
//	public static explicit operator Point(Vector3 _vector)
//	{
//		return new Point (_vector.x, _vector.z,_vector.y);
//	}
//}
