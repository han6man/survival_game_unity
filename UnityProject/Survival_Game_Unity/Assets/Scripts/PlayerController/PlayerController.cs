using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public Vector2 spawnPos;
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool onGround;
    [SerializeField] private TerrainGeneration terrainGenerator;
    [SerializeField] private TileClass selectedTile;
    [SerializeField] private int playerRange;

    private Rigidbody2D rb;
    private Animator anim;

    private Vector2Int mousePos;
    private float horizontal;
    private bool hit;
    private bool place;

    public void Spawn()
    {
        GetComponent<Transform>().position = spawnPos;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
            onGround = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
            onGround = false;
    }

    private void FixedUpdate()
    {
        // do stuff
        horizontal = Input.GetAxis("Horizontal");
        float jump = Input.GetAxisRaw("Jump");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if (horizontal > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (horizontal < 0)
            transform.localScale = new Vector3(1, 1, 1);

        if (vertical > 0.1f || jump > 0.1f)
        {
            if (onGround)
                movement.y = jumpForce;
        }

        rb.velocity = movement;
    }

    private void Update()
    {
        hit = Input.GetMouseButtonDown(0);
        place = Input.GetMouseButton(1);

        //set mouse pos
        mousePos.x = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f);
        mousePos.y = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f);

        if (Vector2.Distance(transform.position, mousePos) <= playerRange &&
            Vector2.Distance(transform.position, mousePos) > 1f)
        {
            if (place)
            {
                terrainGenerator.CheckTile(selectedTile, mousePos.x, mousePos.y, false);
            }
        }
        
        if (Vector2.Distance(transform.position, mousePos) <= playerRange)
        {
            if (hit)
                terrainGenerator.RemoveTile(mousePos.x, mousePos.y);
        }

        anim.SetFloat("horizontal", horizontal);
        anim.SetBool("hit", hit || place);
    }
}
