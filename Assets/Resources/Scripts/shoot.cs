//using UnityEngine;
//using System.Collections;
//
//public class shoot : MonoBehaviour {
//    public get_rotation gr;
//    private GameObject danmaku;
//    public float parent_rotation;
//    public danmaku_fly bullet_class;
//    Vector3 loc;
//    // Use this for initialization
//    void Start () { 
//        danmaku = bullet_class.prefab;
//        parent_rotation = gr.rotaion;
//        //bullet_class = GetComponent<danmaku_fly>();
//
//        StartCoroutine(StartBulletPattern());
//        //danmaku = new GameObject();
//      //  danmaku=GameObject.Instantiate(Resources.Load("Prefab/danmaku") as GameObject, transform.position, Quaternion.Euler(0, 25, 0)) as GameObject;
//    }
//	
//	// Update is called once per frame
//	void Update () {
//        parent_rotation = gr.rotaion;    
//    }
//    //IEnumerator shoot_phase()
//    //{
//
//    //    StartCoroutine(shoot_type_1(20, 0.1f,120,2, danmaku.transform));
//    //    yield return new WaitForSeconds(0.3f);
//    //}
//    IEnumerator shoot_type_1(int sum, float reload_time, float spread, float volly, Transform ammo)
//    {
//        loc = new Vector3(transform.position.x - transform.parent.position.x, transform.position.y - transform.parent.position.y, transform.position.z - transform.parent.position.z);
//        bullet_class.previous_position = loc;
//        float angle = this.transform.eulerAngles.y;
//        if (sum <= 0)
//        {
//            
//            Instantiate(ammo, transform.position, Quaternion.Euler(0, 0, 0));
//        }
//        else
//        {
//            while (volly > 0)
//            {
//                for (int i = 0; i < sum; i++)
//                {
//                    loc = new Vector3(transform.position.x - transform.parent.position.x, transform.position.y - transform.parent.position.y, transform.position.z - transform.parent.position.z);
//                    bullet_class.previous_position = loc;
//                    Instantiate(ammo, transform.position, Quaternion.Euler(0, angle, 0));
//                    angle += spread / sum;
//                    if (reload_time > 0)
//                        yield return new WaitForSeconds(reload_time);
//                }
//                angle = this.transform.eulerAngles.y;
//                volly--;
//            }
//            
//        }
//    } // стрельба пульками с небольшим углом между ними
//    IEnumerator shoot_type_2(int sum, float reload_time, float volly, Transform ammo)
//    {
//        float angle = this.transform.eulerAngles.y;
//        if(sum ==0)
//        {
//            Instantiate(ammo, transform.position, Quaternion.Euler(0, 0, 0));
//        }
//        else
//        {
//            while (volly > 0)
//            {
//                for (int i = 0; i < sum; i++)
//                {
//                    Instantiate(ammo, transform.position, Quaternion.Euler(0, angle, 0));
//                    angle += 360 / sum;
//                }
//                volly--;
//                angle = this.transform.eulerAngles.y;
//                yield return new WaitForSeconds(reload_time);
//            }
//        }
//    } // круговая стрельба
//    IEnumerator shoot_type_3(float reload_time, float volly, Transform ammo)
//    {
//        float angle = this.transform.eulerAngles.y-2*(45/3);
//            while (volly > 0)
//            {
//                for (int i = 0; i < 3; i++)
//                {
//                    Instantiate(ammo, transform.position, Quaternion.Euler(0, angle, 0));
//                    angle += 45 / 3;
//                }
//                volly--;
//            angle -= 2*(45 / 3);
//                yield return new WaitForSeconds(reload_time);
//            }
//        
//    } // стрельба несколькими пульками одновременно, с некоторым углом между ними
//    IEnumerator shoot_type_4(int sum, float reload_time, float spread, float volly, Transform ammo)
//    {
//        float angle = this.transform.eulerAngles.y - 180;
//        if (sum <= 0)
//        {
//            Instantiate(ammo, transform.position, Quaternion.Euler(0, 0, 0));
//        }
//        else
//        {
//            while (volly > 0)
//            {
//                for (int i = 0; i < sum; i++)
//                {
//                    Instantiate(ammo, transform.position, Quaternion.Euler(0, angle, 0));
//                    angle += spread / sum;
//                    if (reload_time > 0)
//                        yield return new WaitForSeconds(reload_time);
//
//
//                }
//                angle = this.transform.eulerAngles.y - spread / 2;
//            }
//            volly--;
//        }
//    }
//    IEnumerator shoot_type_5(int sum, float reload_time, float rotation, float angle_speed, float acceleration, Transform ammo)
//    {
//        bullet_class.Acceleration = acceleration;
//        for (int i = 0; i < sum; i++)
//        {
//            bullet_class.Rotation = rotation;
//            bullet_class.Angle_speed = angle_speed;
//            loc = new Vector3(transform.position.x - transform.parent.position.x, transform.position.y - transform.parent.position.y, transform.position.z - transform.parent.position.z);
//            bullet_class.previous_position = loc;
//            Instantiate(ammo, transform.position, Quaternion.Euler(0, parent_rotation, 0));
//            if (reload_time > 0)
//                yield return new WaitForSeconds(reload_time);
//        }
//    }//стрельба по не прямой траектории
//    IEnumerator shoot_type_6(int sum, float reload_time, float acceleration, float volly, Transform ammo)
//    {
//        loc = new Vector3(transform.position.x - transform.parent.position.x, transform.position.y - transform.parent.position.y, transform.position.z - transform.parent.position.z);
//        bullet_class.previous_position = loc;
//        bullet_class.Acceleration = acceleration;
//            while (volly > 0)
//            {
//                for (int i = 0; i < sum; i++)
//                {
//                    loc = new Vector3(transform.position.x - transform.parent.position.x, transform.position.y - transform.parent.position.y, transform.position.z - transform.parent.position.z);
//                    bullet_class.previous_position = loc;
//                    Instantiate(ammo, transform.position, Quaternion.Euler(0, 0, 0));
//                    if (reload_time > 0)
//                        yield return new WaitForSeconds(reload_time);
//                }
//                volly--;
//            }
//        
//    } // стрельба пульками с ускорением
//
//    IEnumerator StartBulletPattern()
//    {
//        //StartCoroutine(Blast(this.transform, bullet_class.transform, 40, 1, 120, 0.1f));
//        
//
//        //StartCoroutine(shoot_type_1(10, 0.1f, 0, 1, danmaku.transform));
//        //StartCoroutine(shoot_type_2(10, 2f, 2, danmaku.transform));
//        //StartCoroutine(shoot_type_3(1f, 3, danmaku.transform));
//        //StartCoroutine(shoot_type_4(150, 0.1f, 180, 1, danmaku.transform));
//        StartCoroutine(shoot_type_5(10, 0.1f, 0f,0f,0f, danmaku.transform));
//        //StartCoroutine(shoot_type_6(10, 0.1f, 0.2f, 1, danmaku.transform));
//        yield return new WaitForSeconds(3.0f);
//        StartCoroutine(StartBulletPattern());
//    }
//}
