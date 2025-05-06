using UnityEngine;
/// <summary>
/// Player Controller Class
/// </summary>
public class PlayerController : MonoBehaviour
{

    [HideInInspector]
    public Vector2 spawnPos;

    [SerializeField] private GameObject handHolder;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool onGround;
    [SerializeField] private TerrainGeneration terrainGenerator;
    [SerializeField] private TileClass selectedTile;
    [SerializeField] private int playerRange;

    public InventoryManager inventory;

    private Rigidbody2D rb;
    private Animator anim;

    private Vector2Int mousePos;
    private float horizontal;
    private bool hit;
    private bool place;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void Spawn()
    {
        GetComponent<Transform>().position = spawnPos;
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
        float jump = Input.GetAxisRaw("Jump");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if (horizontal > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (horizontal < 0)
            transform.localScale = new Vector3(1, 1, 1);

        //Jumping
        if (vertical > 0.1f || jump > 0.1f)
        {
            if (onGround)
                movement.y = jumpForce;
        }

        //Autojumping
        if (FootRaycast() && !HeadRaycast() && movement.x != 0)
        {
            if (onGround)
                movement.y = jumpForce * 0.6f;//jump multiplier for autojumping
        }

        rb.velocity = movement;
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        hit = Input.GetMouseButtonDown(0);
        place = Input.GetMouseButtonDown(1);

        //set mouse pos
        mousePos.x = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f);
        mousePos.y = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f);

        if (Vector2.Distance(transform.position, mousePos) <= playerRange &&
            Vector2.Distance(transform.position, mousePos) > 1f)
        {
            if (place && inventory.selectedItem != null && inventory.selectedItem.itemType == ItemClass.ItemType.Block)
            {
                terrainGenerator.CheckTile(inventory.selectedItem.tile, mousePos.x, mousePos.y, false);
                inventory.Remove(inventory.selectedItem, 1);
            }
        }
        
        if (Vector2.Distance(transform.position, mousePos) <= playerRange)
        {
            if (hit)
            {
                //terrainGenerator.RemoveTile(mousePos.x, mousePos.y);
                terrainGenerator.BreakTile(mousePos.x, mousePos.y, inventory.selectedItem);              
            }
        }

        anim.SetFloat("horizontal", horizontal);
        anim.SetBool("hit", hit || place);

        if (inventory.selectedItem != null)
        {
            handHolder.GetComponent<SpriteRenderer>().sprite = inventory.selectedItem.itemIcon;
            if (inventory.selectedItem.itemType == ItemClass.ItemType.Block)
            {
                handHolder.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
            }
            else
            {
                handHolder.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            handHolder.GetComponent<SpriteRenderer>().sprite = null;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //Use the item
            if (inventory.selectedItem != null)
                inventory.selectedItem.Use(this);
        }
    }

    /*
    private void OnValidate()
    {
        Debug.DrawRay(transform.position - (Vector3.up * 0.5f), -Vector2.right * transform.localScale.x, Color.white, 10f);
        Debug.DrawRay(transform.position + (Vector3.up * 0.5f), -Vector2.right * transform.localScale.x, Color.white, 10f);
    }
    */

    private bool FootRaycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - (Vector3.up * 0.5f), -Vector2.right * transform.localScale.x, 1f/*ray length*/, layerMask);
        return hit;
    }

    private bool HeadRaycast()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.up * 0.5f), -Vector2.right * transform.localScale.x, 1f/*ray length*/, layerMask);
        return hit;
    }
}
