using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// рисует линии соединения чекпоинтов и ворота для смены чекпоинта
/// </summary>
public class CheckpointConnect : MonoBehaviour
{
    public Vector3 WallSize = new Vector3(30.0f, 2.0f, 1.0f);
    
    private void OnDrawGizmos()
    {
        if (this.transform.childCount < 2)
        {
            return;
        }

        for (int i = 0; i < this.transform.childCount - 1; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(this.transform.GetChild(i).position, this.transform.GetChild(i + 1).position);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.GetChild(this.transform.childCount - 1).position, this.transform.GetChild(0).position);
    }    
    public void AngleCheckpointWalls()
    {   
        for (int i = 0; i < this.transform.childCount; i++)
        {
            //current & next 
            Transform curr;
            Transform next;
            if (i < this.transform.childCount - 1)
            {
                curr = this.transform.GetChild(i);
                next = this.transform.GetChild(i + 1);
            }
            else
            {
                curr = this.transform.GetChild(i);
                next = this.transform.GetChild(0);
            }

            //step 1. checkpoint collider 
            curr.localScale = WallSize;
            curr.LookAt(next);              
        }
    }    
    public string EditorLabelText()
    {
        return "There are " + this.transform.childCount.ToString() + " Checkpoints";
    }
}
