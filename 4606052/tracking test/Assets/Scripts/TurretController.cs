using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField]
    public float rotationSpeed = 1f;
    [SerializeField]
    public GameObject projectile;
    [SerializeField]
    public float fireRate = 1f;
    [SerializeField]
    public float accuracyMod = 0f;
    float rotationModifier = 1f;
    float fireRateTimestamp;
    public GameObject target;
    int closestTarget = 0;
    float closestDistance = float.MaxValue;
    bool arraySearch = false;
    float timeToImpact;
    Quaternion targetAngle;
    void Awake()
    {
        GetTarget();
    }

    void FixedUpdate()
    {
        if (arraySearch) { GetTarget(); arraySearch = false; } // the arraySearch bool exists because FindObjectsByType can be performance intensive with a lot of objects
                                                               // on larger games where performance is a concern you may want to use a timer to only run it every couple of frames
                                                               // another solution could be to run the search every x amount of time and then check the array in a 
                                                               // most games i can think of do something like this where the list of targets refreshes every second or so
                                                               // for a small game or demo like this its totally fine to run each frame so thats what i do 
                                                               // since it means it will instantly update the target list if you spawn or delete targets while the game is running
        if (target != null) 
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, PredictiveGuidance(target), rotationSpeed * rotationModifier);
        }
        arraySearch = true;
    }

    public GameObject GetTarget()
    {
        var targetArray = FindObjectsByType<TargetComponent>(FindObjectsSortMode.None);
        closestDistance = float.MaxValue;
        for (int i = 0; i < targetArray.Length; i++)
        {
            if (Vector3.Distance(transform.position, targetArray[i].transform.position) < closestDistance)
            {
                if (Vector3.Distance(transform.position, targetArray[i].transform.position) / projectile.GetComponent<ProjectileController>().speed > projectile.GetComponent<ProjectileController>().lifetime) 
                    continue;
                closestTarget = i;
                closestDistance = Vector3.Distance(transform.position, targetArray[i].transform.position);
            }
            target = targetArray[closestTarget].gameObject;
        }
        if (target == null) {return null;}
        else
            return (target);
    }

    public Quaternion PredictiveGuidance(GameObject uid) // calculates the angle to face to hit a target in motion.
    {
        Rigidbody2D rigid = uid.GetComponent<Rigidbody2D>();
        float distance = Vector2.Distance(uid.transform.position, transform.position); // current distance from target
        float projectileVelocity = projectile.GetComponent<ProjectileController>().speed; // get projectiles velocity
        Vector3 targetVelocity = rigid.velocity; // get target velocity
        if (targetVelocity == null) { targetVelocity = new Vector3(0, 0, 0); } // prevent null
        timeToImpact = distance / projectileVelocity; // time it will take for the projectile to reach the target
        Vector3 predictedPosition = uid.transform.position + targetVelocity * timeToImpact; // predict target position at impact time
        Vector3 angle = predictedPosition - transform.position;

        for (int i = 0; i < 4; i++) // iterate the time to impact calculation to increase accuracy
        // sure this increases accuracy but is a bit lazy since you can get the correct time to impact without iterating by using quadratics and no iterations
        {
            distance = Vector2.Distance(predictedPosition, transform.position); // current distance from targets future position
            timeToImpact = distance / projectileVelocity; // figure out new time to impact
            predictedPosition = uid.transform.position + targetVelocity * timeToImpact; // predict target position at impact time
        }

        angle = predictedPosition - transform.position; // figure out angle to point at the targets position
        targetAngle = Quaternion.LookRotation(Vector3.forward, angle); // make sure the target rotates to put its forward at the target

        if (timeToImpact > projectile.GetComponent<ProjectileController>().lifetime) // if projectile wont reach the target then dont rotate and set the target to null
        {
            target = null;
            return transform.rotation;
        }
        else
        {
            float diff = Mathf.Abs(Mathf.DeltaAngle(targetAngle.eulerAngles.z, transform.rotation.eulerAngles.z));
            if (diff < 0.1f) // if we are pointing almost directly at the target just snap to the target
                             // RotateTowards wont reach the exact angle on a moving target so this helps prevent some edge cases
            {
                transform.rotation = targetAngle;
            }
            else if (diff < 10f) // if we are close to pointing at the target then increase rotation speed to further help make sure its actually aiming at the desired angle
            {
                rotationModifier = rotationSpeed * (Mathf.InverseLerp(10, 0, diff) + 1);
            }
            else { rotationModifier = 1f; }

            if (diff < 1.5f) // if pointing near the target then fire
            {
                TryShoot(transform.forward);
            }
            return targetAngle;
        }
    }

    bool TryShoot(Vector2 direction) // left this as a bool cause i thought i might want to use it to check if i can fire or not and make decisions based on that but never used it in the end
    {
        if (Time.time < fireRateTimestamp) return false; // if we cant fire then return false
        fireRateTimestamp = Time.time + 1/fireRate; // if we can fire then start a new timer fire
        Quaternion fireDirection = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, Random.Range(-accuracyMod, accuracyMod))); // add innaccuraccy to the fire direction (rule of cool)
        Instantiate(projectile, transform.position, fireDirection); // fire projectile
        return true; // we fired so return true
    }
}
