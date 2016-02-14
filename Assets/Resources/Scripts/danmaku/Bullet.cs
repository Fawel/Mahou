using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class Bullet : Danmaku {
    Danmaku_movement dan_mov;
    void Start () {
        damage = 1;
		if (Coordx.Length != 0)
			CreatePointList ();
        dan_mov = this.gameObject.GetComponent<Danmaku_movement>();
        Destroy(this.gameObject, 15f);
	}
	
	void FixedUpdate () {
        switch (type)
        {
            case movement_type.simple:
			dan_mov.Simple_Move(Rotation, Angular_acceleration, Speed, Acceleration,transform.position);
                break;
		case movement_type.second:
			dan_mov.second_move(Rotation,Angular_acceleration,Speed, Acceleration,transform.position, Max_rotation);
			break;
		case movement_type.points:
			if (arraycounter <= PointArray.Count-1) {
				try
					{
					transform.LookAt ((Vector3)PointArray [arraycounter], Vector3.zero);
					dan_mov.point_move (PointArray [arraycounter], Speed, Acceleration);
					}
				catch(ArgumentOutOfRangeException e) { arraycounter = 0; throw new ArgumentOutOfRangeException ();}
				} 

			break;
		default:
			break;
        }
		Rotation=(Angular_acceleration!=0)?Rotation+Angular_acceleration : Rotation;
		Speed = (Acceleration != 0) ? Speed + Acceleration : Speed;
    }
		
			

}
