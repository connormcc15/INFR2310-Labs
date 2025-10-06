using UnityEngine;
using UnityEngine.AI;

public class Controller : MonoBehaviour
{

    public GameObject Target;
    public NavMeshAgent agent;
    private Animator animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = Target.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool("Attack", true);
        animator.SetBool("Walk", false);
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("Attack", false);
        animator.SetBool("Walk", true);
    }
}
