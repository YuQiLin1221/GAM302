using Fusion;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{

    public GameObject weapon;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weapon.GetComponent<BoxCollider>().enabled = true;
        }else if (Input.GetMouseButtonUp(0))
        {
            weapon.GetComponent <BoxCollider>().enabled = false;
        }
    }
}
