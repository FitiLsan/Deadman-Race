using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public GameObject Car = null;    
    public float ChaseSpeed = 1.5f;
    public int CamNumber = 0;
    private List<Transform> FollowPos = new List<Transform>();
    
    private void Update()
    {
        if (Car == null)
            return;

        if (FollowPos.Count == 0)
        {
            foreach (Transform child in Car.transform)
            {
                if (child.gameObject.name.StartsWith(Globals.NameFollowPos) == true)
                {
                    FollowPos.Add(child);
                    CamNumber = 0;
                }
            }
            
        }

        this.transform.LookAt(Car.transform);
        float followspeed = Mathf.Abs(Vector3.Distance(this.transform.position, FollowPos[CamNumber].position)) * ChaseSpeed;
        this.transform.position = Vector3.MoveTowards(this.transform.position, FollowPos[CamNumber].position, followspeed * Time.deltaTime);
    }
}
