using UnityEngine;

public class ObjectBreakingScript : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject ObjectVisual;

    [Header("Shard Movement")]
    [SerializeField] private float HSpeed = 1.5f;
    [SerializeField] private float initialUpForce = 3f;
    [SerializeField] private float gravity = 12f;
    [SerializeField] private float smallBounceForce = 1f;
    [SerializeField] private float groundY = -3f;
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private float fadeDelay = 0.4f;

    //Shards
    [Header("Shards")]
    [SerializeField] private GameObject shardParent;


    private void OnTriggerEnter(Collider other)
    {
        //Break on Colliding With Player Damage Goes Here
        if (other.TryGetComponent<BasePlayerDamage>(out BasePlayerDamage hit))
        {
            BreakPot();
            ObjectVisual.SetActive(false);
        }
    }

    private void BreakPot()
    {
        foreach (Transform shard in shardParent.transform)
        {
            shard.gameObject.SetActive(true);
            shard.localPosition = Vector3.zero;
            shard.GetComponent<ShardBounce>().StartBounce(HSpeed, initialUpForce, gravity, 
                smallBounceForce, groundY, fadeSpeed, fadeDelay);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) 
        {
            //Temp Testing Logic for Pot Breaking
            BreakPot();
            ObjectVisual.SetActive(false);
        }
    }

    
}
