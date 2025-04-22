using UnityEngine;

public class IdleKid : MonoBehaviour

{

    public Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("idle", true);
    }
}
