using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float movementX;
    [SerializeField] private Animator anim;
    [SerializeField] private SoundPlayer soundPlayer;
    // TODO add whistle sound + other sound clips as instance vars

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer(){
        movementX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(movementX, 0f, 0f) * moveSpeed * Time.deltaTime;
    }
}
