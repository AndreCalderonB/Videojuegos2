using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    float v, h;
    float cameraRot;
    float vel;


    public bool medic;

    public bool soldado;
    bool active;
    bool move;
    bool shoot;
    bool gunOut = false;
    bool hasActivated = false;


    public Image Mov;
    public int MaxMov;

    public Image hpbar; 
    
    public float maxhp;
    public float hp;

    private float initX;
    private float initZ;


    private float currX;
    private float currZ;

    private float distX;
    private float distZ;

    private float currDist;
    private float distPerc;

    public AudioSource SonidoDisparar;

    Animator anim;
    GameObject gun;
    Camera camera;
    public GameObject healthpack;
    public RawImage aimcross;
    public RawImage etiquetaJugador;

    public RawImage etiquetaSoldado;

    float probabilidadDeDsiparo;



    // Start is called before the first frame update
    void Start()
    {
        SonidoDisparar = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        active = false;
        move = false;
        this.initX = this.gameObject.transform.position.x;
        this.initZ = this.gameObject.transform.position.z;
        this.currX = this.gameObject.transform.position.x;
        this.currZ = this.gameObject.transform.position.z;
        gun = this.transform.Find("Bip001").gameObject.transform.Find("Bip001 Pelvis").gameObject.transform.Find("Bip001 Spine").gameObject.transform.Find("Bip001 R Clavicle").gameObject.transform.Find("Bip001 R UpperArm").gameObject.transform.Find("Bip001 R Forearm").gameObject.transform.Find("Bip001 R Hand").gameObject.transform.Find("R_hand_container").gameObject.transform.Find("w_shotgun").gameObject;
        camera = this.transform.Find("cam").gameObject.GetComponent<Camera>();    
        aimcross.enabled = false;
        etiquetaJugador.enabled = false;
        etiquetaSoldado.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        cameraRot = Input.GetAxis("Mouse X");

        this.currX = this.gameObject.transform.position.x;
        this.currZ = this.gameObject.transform.position.z;

        this.distX = Mathf.Abs(currX-initX);
        this.distZ = Mathf.Abs(currZ-initZ);

        this.currDist = Mathf.Sqrt((distX*distX) + (distZ * distZ));

        

        if (active && move){
            
            hpbar.fillAmount = ((this.hp * 100) / maxhp) / 100;
            if (MaxMov - currDist <= 0)
            {
                move = false;
            }
            else
            {
                distPerc = (float)currDist / MaxMov;
                Mov.fillAmount = distPerc;
            }

            if (Input.GetKey(KeyCode.LeftShift)){
                vel=6;
            }else{
                vel=3;
            }
            v = Input.GetAxis("Horizontal");
            h = Input.GetAxis("Vertical"); 
               

            if(v == 0 && h == 0)
            {
                anim.SetFloat("v", 0);
            }
            else
            {
                anim.SetFloat("v", vel);
            }
            transform.Translate(v*Time.deltaTime*vel,0,h*Time.deltaTime*vel);
            transform.Rotate(0, cameraRot, 0);

            if (Input.GetKeyDown(KeyCode.E))
            {
                anim.SetTrigger("wave");
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                if(!gunOut){
                   anim.SetTrigger("gunO"); 
                   gun.SetActive(true);
                   aimcross.enabled = true;
                   
                    gunOut = true;
                    anim.SetBool("gunOut", true);
                }else{
                    gun.SetActive(false);
                    aimcross.enabled = false;
                    
                    gunOut = false;
                    anim.SetBool("gunOut", false);
                }
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                if(medic == true)
                {
                    Instantiate(healthpack, this.transform.position + this.transform.forward * 1f, this.transform.rotation);
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                if(gunOut){
                    var ray = camera.ScreenPointToRay(Input.mousePosition);
                    SonidoDisparar.Play();
                    RaycastHit hit;
                    if(Physics.Raycast(ray, out hit)){
                        if(hit.transform.gameObject.tag == "soldierP1" || hit.transform.gameObject.tag == "soldierP2")
                        {
                            probabilidadDeDsiparo = Random.Range(0,100);
                            if(probabilidadDeDsiparo>40){
                                Debug.Log("Disparo acertado a " + hit.transform.gameObject.tag);
                                Animator hitAnim = hit.transform.GetComponent<Animator>();
                                if(hit.transform.gameObject.GetComponent<Movement>().hp > 0)
                                    hit.transform.gameObject.GetComponent<Movement>().hp -= 50;
                                hitAnim.SetTrigger("damage");

                            }else{
                                Debug.Log("Disparo fallado");
                            }
                        }
                    }
                 }
                anim.SetTrigger("shoot");
            }
            var rayEnemigo = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit enemigo;
            if(Physics.Raycast(rayEnemigo, out enemigo)){
            if(enemigo.transform.gameObject.tag == "soldierP1" || enemigo.transform.gameObject.tag == "soldierP2"){
                    Debug.Log("Estoy pegando");
                    aimcross.color=Color.red;
                }else{
                    aimcross.color = Color.black;
                }
            }
            
            if(medic == true){
                etiquetaJugador.enabled = true;
            }else{
                etiquetaJugador.enabled = false;
            }

            if(soldado == true){
                etiquetaSoldado.enabled = true;
            }else{
                etiquetaSoldado.enabled = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "healthpack")
        {
            this.hp = this.hp + 50;
            if(this.hp > maxhp)
            {
                this.hp = maxhp;
            }
            Destroy(other.gameObject);
        }
    }
    public void activate(){
        
        active = true;
        move = true;
        if (!hasActivated)
        {
            hasActivated = true;
            initX = this.gameObject.transform.position.x;
            initZ = this.gameObject.transform.position.z;
            Mov.fillAmount = 0;
        }
        print(this.hp);
        currX = this.gameObject.transform.position.x;
        currZ = this.gameObject.transform.position.z;
       
    }
    public void deactivate(){
        active = false;
        move = false;
    }
}
