using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetPlayer : MonoBehaviour
{

    void Update() { transform.position = GameManager.instance.Player.transform.position; }
    
}
