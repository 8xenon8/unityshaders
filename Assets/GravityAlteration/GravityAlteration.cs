using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAlteration : MonoBehaviour
{
    Player player;
    GameObject aligner;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        aligner = transform.Find("PlayerAligner").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        GravityHelper.SetGravity(transform.position - player.gameObject.transform.position);
        //aligner.transform.rotation = Quaternion.LookRotation();
    }
}
