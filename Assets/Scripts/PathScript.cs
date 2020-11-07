using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour
{
    public Color lineColor;

    public List<Transform> nodes = new List<Transform>();

    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        Transform[] transforms = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for(int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i] != transform)
            {
                nodes.Add(transforms[i]);
            }
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode = Vector3.zero;

            if (i == 0)
            {
                Gizmos.DrawWireSphere(currentNode, 0.3f);
            }

            if(i > 0)
            {
                previousNode = nodes[i - 1].position;
                Gizmos.DrawLine(previousNode, currentNode);
                Gizmos.DrawWireSphere(currentNode, 0.3f);
            }
        }
    }
}
