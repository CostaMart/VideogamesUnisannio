using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class TPSAnimatorManager : MonoBehaviour
{
    // riferimetno alla posizione desiderata durante la mira
    [Tooltip("Ik settings")]
    [Header("IK Settings")]
    public Animator _animator;
    [Tooltip("in order to activate IK functinoalities, it is required to apply a constraint ad pass it to this cript. This is the constraint that will move the character upper body part")]
    public MultiAimConstraint aimConstraint;

    [Tooltip("this constraint is required to move the character left hand in order to grab the weapon")]
    public TwoBoneIKConstraint twoBoneIKConstraint;

    [Tooltip("represents the postion of the weapon when the character is not aiming")]
    public Transform relaxedWeaponStand;

    [Tooltip("represents the postion of the weapon when the character is aiming")]
    public Transform aimingWeaponStand;

    public RigBuilder rb;

    [Tooltip("reference to the transform representing where the left hand should be placed on the weapon when the character is aiming")]
    public Transform wpnFrontHandle;

    [Tooltip("reference to the left hand Ik target transform")]
    public Transform IKLeftHand;
    Animator animator;

    public GameObject weapon;

    private bool aiming = false;
    private bool lastWas = false;

    // actions
    public PlayerInput playerInput;

    private InputAction aim;
    private InputAction move;
    private InputAction reload;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // input system setup
        move = playerInput.actions["Move"];
        aim = playerInput.actions["Aim"];
        reload = playerInput.actions["Reload"];
        playerInput.actions["Aim"].performed += ctx => { aiming = true; animator.SetBool("aiming", aiming); };
        playerInput.actions["Aim"].canceled += ctx => { aiming = false; animator.SetBool("aiming", aiming); };
        move.performed += ctx => { animator.SetBool("walking", true); };
        move.canceled += ctx => { animator.SetBool("walking", false); };
        reload.performed += ctx => { animator.SetTrigger("reloading"); };

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        // se  stiamo mirando spostiamo l'arma nella posizione di mira, si controlla lastwas per non farlo ripetutamente
        if (aiming)
        {
            if (!lastWas)
            {
                lastWas = true;
                weapon.transform.SetParent(aimingWeaponStand);
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localRotation = Quaternion.identity;

                //1* qui cambiamo il punto verso cui il personaggio si rivolge, quando miriamo lo facciamo girare leggermente più a destra per un effetto migliore
                var newSourceObject = aimConstraint.data.sourceObjects;
                newSourceObject.SetWeight(0, 0);
                newSourceObject.SetWeight(1, 1);

                var newConstraintData = aimConstraint.data;
                newConstraintData.offset = new Vector3(-27.3f, 0, 0);
                aimConstraint.data = newConstraintData;
                aimConstraint.data.sourceObjects = newSourceObject;
                twoBoneIKConstraint.weight = 1;
                rb.Build();
            }

            // qui spostiamo la mano sinistra nella posizione corretta per afferrare l'arma, questa è l'unica cosa che è necessario fare sempre ad ogni frame
            IKLeftHand.position = wpnFrontHandle.position;
            IKLeftHand.rotation = wpnFrontHandle.rotation;
        }

        // se non stiamo mirando spostiamo l'arma nella posizione di riposo, si controlla lastwas per non farlo ripetutamente
        if (!aiming && lastWas)
        {
            lastWas = false;
            weapon.transform.SetParent(relaxedWeaponStand);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;

            // questo è il duale del commento 1*, se non stiamo mirando vogliamo il personaggio rivolto in avanti 
            var newSourceObject = aimConstraint.data.sourceObjects;
            newSourceObject.SetWeight(0, 1);
            newSourceObject.SetWeight(1, 0);
            aimConstraint.data.sourceObjects = newSourceObject;
            var newConstrintData = aimConstraint.data;
            newConstrintData.offset = Vector3.zero;
            aimConstraint.data = newConstrintData;
            twoBoneIKConstraint.weight = 0;
            rb.Build();

        }


    }

}