
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
public class BasicOrc : MonoBehaviour
{
    public AIPath aiPath;
    protected virtual void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
