using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperCollider : MonoBehaviour
{
    //бампер для обнаружения столкновения с другим объектом, кроме земли.
    //событие сработает при ударе бампера. родительский скрипт может что-то делать с этой информацией (например, воспроизводить звук), если он хочет
    public delegate void BumperColliderHit(int objectid, Collider collider);
    public static event BumperColliderHit OnBumperColliderHit;

    private int instanceid;

    private void Start()
    {
        instanceid = this.transform.parent.gameObject.GetInstanceID();
    }
    private void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag(Globals.TagCheckpoint) == false)
        {
            if (OnBumperColliderHit != null)
            {
                //событие столкновения бампера с коллайдером другого объекта
                OnBumperColliderHit(instanceid, other);
            }
        }        
    }
    public void AutoSizePositionBumper()
    {
       // шаг 1.размер бампера
       // сохраняем исходное родительское вращение
       GameObject parent = this.transform.parent.gameObject;
        float rx = parent.transform.eulerAngles.x;
        float ry = parent.transform.eulerAngles.y;
        float rz = parent.transform.eulerAngles.z;
        // повернуть родителя на ноль (временно)
        parent.transform.rotation = Quaternion.identity;
        Bounds bounds = this.transform.parent.gameObject.GetComponent<Renderer>().bounds;
        BoxCollider bumper = this.GetComponent<BoxCollider>();
        //настраивает размер бампера(по родительскому скейлингу)
        float mx = 1 / parent.transform.localScale.x;
        float my = 1 / parent.transform.localScale.y;
        float mz = 1 / parent.transform.localScale.z;
        bumper.size = new Vector3((bounds.size.x * mx) * 1.1f, (bounds.size.y * my) * 0.1f, (bounds.size.z * mz) * 1.1f);
        // recall original parent rotation
        parent.transform.rotation = Quaternion.Euler(rx, ry, rz);

        //Шаг 2. Находим центр позиции бампера
        bumper.center = new Vector3(bumper.center.x, bumper.center.y * 1.3f, bumper.center.z);
    }
}
