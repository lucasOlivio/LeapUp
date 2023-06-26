using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public GameObject cameraObj; // Camera to follow Y;
    public GameObject groundPrefab; // Ground object to manage
    public Sprite groundBlockSprite; // Sprite referencing the normal ground
    public Sprite deathBlockSprite; // Sprite for the ground where player dies if touches
    public int offset;

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    [SerializeField] private float incrementAmount;   // The amount to increment the Y position per second
    private Vector3 initialPosition;
    private float cameraY; // Minimum distance Y to mantain from camera

    void Awake()
    {
        EventManager.PlayerStart += PlayerStart;
        EventManager.GameStart += GameStart;

        initialPosition = transform.position;

        // Get the SpriteRenderer component attached to the Ground GameObject
        spriteRenderer = groundPrefab.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraY = cameraObj.transform.position.y + offset;
    }

    // Update is called once per frame
    void Update()
    { }

    private void FixedUpdate()
    {
        IncreaseY();
    }

    void IncreaseY()
    {
        float currentDistance = cameraObj.transform.position.y - transform.position.y;
        if (currentDistance > cameraY)
        {
            float y = currentDistance - cameraY;
            GameUtils.ChangePosition(this.gameObject, y, 1);
        }
        else if (GameManager.state == GameManager.GameStates.Playing)
        {
            GameUtils.ChangePosition(this.gameObject, (incrementAmount * Time.fixedDeltaTime), 1);
        }
    }

    void PlayerStart()
    {
        // Change the sprite to the death block
        spriteRenderer.sprite = deathBlockSprite;
    }

    void GameStart()
    {
        transform.position = initialPosition;
        // Change the sprite to the ground block
        spriteRenderer.sprite = groundBlockSprite;
    }
}
