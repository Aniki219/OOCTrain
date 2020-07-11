using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainController : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 velocity;
    public List<Vector3> nodes;
    // Start is called before the first frame update
    void Start()
    {
        nodes = new List<Vector3>();
        velocity = new Vector3(speed, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (nodes.Count > 1)
        {
            velocity = (nodes[1] - nodes[0]).normalized * speed;
            if (sqrDist(nodes[1]) < 0.1f)
            {
                nodes.RemoveAt(0);
            }
        }
        velocity.x = Mathf.Round(velocity.x * 100f) / 100f;
        velocity.y = Mathf.Round(velocity.y * 100f) / 100f;
        velocity.z = Mathf.Round(velocity.z * 100f) / 100f;
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.Self);

        float angle = Mathf.Atan2(-velocity.z, velocity.x);
        transform.eulerAngles = new Vector3(0, angle * Mathf.Rad2Deg, 0);
    }

    float sqrDist(Vector3 b)
    {
        return Vector3.SqrMagnitude(b - transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Track")
        {
            nodes.Clear();
            Transform trackNodes = other.transform.Find("Nodes");
            if (trackNodes == null) { return; }
            Vector3 firstNode = trackNodes.GetChild(0).position;
            Vector3 lastNode = trackNodes.GetChild(trackNodes.childCount - 1).position;

            bool forwards = (Vector3.SqrMagnitude(transform.position - firstNode) < Vector3.SqrMagnitude(transform.position - lastNode));

            foreach (Transform T in trackNodes)
            {
                if (forwards)
                {
                    nodes.Add(T.position);
                } else
                {
                    nodes.Insert(0, T.position);
                }
            }
        }
    }
}
