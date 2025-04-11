using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class fenceBehaviour1 : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private bool rotate = false;

    private void Start()
    {
      int memoryCount = playerMovement.memoryCount;      
    }
    void Update()
    {
        if (playerMovement.memoryCount >= 5 && rotate == false)
        {
            FenceRotation();
        }
    }

    private void FenceRotation()
    {
        rotate = true;
        transform.Rotate( 0, 90, 0 );
    }

   
}
