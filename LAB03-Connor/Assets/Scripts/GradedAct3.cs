using UnityEngine;
using UnityEngine.AI;

public class GradedAct3 : MonoBehaviour
{

    public GameObject theTarget;
    private NavMeshAgent theAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        theAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        theAgent.destination = theTarget.transform.position;
    }
}
