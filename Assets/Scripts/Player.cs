using UnityEngine;

public class Player : MonoBehaviour, Danhable

{
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private Transform camara;
    [SerializeField] private InputManagerSO inputManager;
    [SerializeField] private float factorGravedad;
    [SerializeField] private float alturaDeSalto;
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem particulas;

    [Header("Deteccion Suelo")]
    [SerializeField] private Transform pies;
    [SerializeField] private float radioDeteccion;
    [SerializeField] private LayerMask queEsSuelo;

    private CharacterController controller;
    private Vector3 direccionMovimiento;
    private Vector3 direccionInput;
    private Vector3 velocidadVertical;

    [Header ("Sistema Combate")]
    [SerializeField] private float vidas;
    [SerializeField] private float distanciaDisparo;
    [SerializeField] private float danhoDisparo;

    [SerializeField] ControlGameOver controlGameOver;
    private AudioSource audioSource;

    private void OnEnable()
    {
        inputManager.OnSaltar += Saltar;
        inputManager.OnMover += Mover; ;
        inputManager.OnRecargar += Recargar;
        inputManager.OnDisparar += Disparar;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        AplicarMovimiento();

        ActualizarMovimiento();
        ManejarVelocidadVertical();

    }
    private void Disparar()
    {
        anim.SetTrigger("Shoot");
        audioSource.Play();
        particulas.Play();
        //saber si impacta con algo
        if (Physics.Raycast(camara.position, camara.forward,out RaycastHit hitInfo, distanciaDisparo))
        {
            // saber si lo impactado es dañable
            if (hitInfo.transform.TryGetComponent(out Danhable sistemaDanho))
            {
                if (!hitInfo.transform.CompareTag("Player"))
                {
                    sistemaDanho.RecibirDanho(danhoDisparo);
                }
                
            }
        }
    }

    private void Recargar()
    {
        anim.SetTrigger("Reload");
    }

    private void Mover(Vector2 ctx)
    {
        direccionInput = new Vector3(ctx.x, 0, ctx.y);
                
    }

    private void Saltar()
    {
        if (EstoyEnSuelo())
        {
            velocidadVertical.y = Mathf.Sqrt(-2 * factorGravedad * alturaDeSalto);
            
        }
        
    }
        
    private void AplicarMovimiento()
    {
        direccionMovimiento = camara.forward * direccionInput.z + camara.right * direccionInput.x;
        direccionMovimiento.y = 0;
        controller.Move(direccionMovimiento * velocidadMovimiento * Time.deltaTime);
    }

    private void ManejarVelocidadVertical()
    {
        // Si hemos aterizado...
        if (EstoyEnSuelo() && velocidadVertical.y < 0)
        {
            //se resetea velocidad vertical
            velocidadVertical.y = 0;

        }
        AplicarGravedad();
    }

    private void ActualizarMovimiento()
    {
        if (direccionMovimiento.sqrMagnitude > 0)
        {
            anim.SetBool("Walking", true);
            RotarHaciaDestino();
        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }

    private void AplicarGravedad()
    {
        velocidadVertical.y += factorGravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);
    }
    private bool EstoyEnSuelo()
    {
        return Physics.CheckSphere(pies.position, radioDeteccion, queEsSuelo);
        
    }
    private void RotarHaciaDestino()
    {
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
        transform.rotation = rotacionObjetivo;
    }
    void Danhable.RecibirDanho(float danho)
    {
        vidas -= danho;
        if (vidas <= 0)
        {
            anim.SetTrigger("Death");
            controlGameOver.GameOver();
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pies.position, radioDeteccion);
    }
}
