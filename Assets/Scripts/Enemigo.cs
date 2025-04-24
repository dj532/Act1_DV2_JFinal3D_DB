using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour, Danhable
{
    private NavMeshAgent agent;
    private Player target;
    private Animator anim;
    private float vidas = 100f;

    [Header ("Sistema Combate")]
    [SerializeField] private Transform puntoAtaque;
    [SerializeField] private float radioAtaque;
    [SerializeField] private float danhoAtaque;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = FindFirstObjectByType<Player>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.transform.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            EnfocarObjetivo();

            LanzarAtaque();
        }
    }

    private void LanzarAtaque()
    {
        agent.isStopped = true;
        anim.SetBool("Attacking", true);
    }
    private void Atacar()
    {
        Collider[] collidersTocados = Physics.OverlapSphere(puntoAtaque.position, radioAtaque);
        foreach(Collider coll in collidersTocados)
        {
            if(coll.TryGetComponent(out Danhable danhable))
            {
                danhable.RecibirDanho(danhoAtaque);
            }
        }
    }
    private void EnfocarObjetivo()
    {
        Vector3 direccionAObjetivo = (target.transform.position - transform.position).normalized;
        direccionAObjetivo.y = 0;
        Quaternion rotacionAObjetivo = Quaternion.LookRotation(direccionAObjetivo);
        transform.rotation = rotacionAObjetivo;
    }

    private void FindeAtaque()
    {
        agent.isStopped = false;
        anim.SetBool("Attacking", false);
    }

    public void RecibirDanho(float danho)
    {
        vidas -= danho;
        if (vidas <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoAtaque.position, radioAtaque);
    }
}
