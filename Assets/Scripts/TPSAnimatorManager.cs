using UnityEngine;
using UnityEngine.Animations.Rigging;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            animator.SetBool("walking", true);
        }
        else
        {
            animator.SetBool("walking", false);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            animator.SetBool("aiming", true);
            aiming = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            animator.SetBool("aiming", false);
            aiming = false;
        }
        if (Input.GetKey(KeyCode.R))
        {
            animator.SetTrigger("reloading");
        }

        // se  stiamo mirando spostiamo l'arma nella posizione di mira, si controlla lastwas per non farlo ripetutamente
        if (aiming && !lastWas)
        {
            lastWas = true;
            weapon.transform.SetParent(aimingWeaponStand);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }

        // se non stiamo mirando spostiamo l'arma nella posizione di riposo, si controlla lastwas per non farlo ripetutamente
        if (!aiming && lastWas)
        {
            lastWas = false;
            weapon.transform.SetParent(relaxedWeaponStand);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }

        if (aiming)
        {

            if (aimConstraint.data.sourceObjects.GetWeight(0) != 0)
            {
                var newSourceObject = aimConstraint.data.sourceObjects;
                newSourceObject.SetWeight(0, 0);
                newSourceObject.SetWeight(1, 1);
                var newConstraintData = aimConstraint.data;
                newConstraintData.offset = new Vector3(-27.3f, 0, 0);
                aimConstraint.data = newConstraintData;
                aimConstraint.data.sourceObjects = newSourceObject;
                rb.Build();
            }


            twoBoneIKConstraint.weight = 1;
            IKLeftHand.position = wpnFrontHandle.position;
            IKLeftHand.rotation = wpnFrontHandle.rotation;

        }
        if (!aiming)
        {
            if (aimConstraint.data.sourceObjects.GetWeight(0) != 1)
            {
                var newSourceObject = aimConstraint.data.sourceObjects;
                newSourceObject.SetWeight(0, 1);
                newSourceObject.SetWeight(1, 0);
                aimConstraint.data.sourceObjects = newSourceObject;
                var newConstrintData = aimConstraint.data;
                newConstrintData.offset = Vector3.zero;
                aimConstraint.data = newConstrintData;
                rb.Build();
            }

            twoBoneIKConstraint.weight = 0;
        }

    }
}
