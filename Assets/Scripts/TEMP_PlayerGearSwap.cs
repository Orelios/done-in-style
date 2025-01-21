using UnityEngine;

public class TEMP_PlayerGearSwap : MonoBehaviour
{
    [Header("Gear Swap Configs")]
    [SerializeField] private GearType currentGearEquipped;
    [SerializeField] private GearType defaultGearToEquip;
    
    [Header("Color Configs")]
    [SerializeField] private Color skateboardColor;
    [SerializeField] private Color rollerSkatesColor;
    [SerializeField] private Color pogoStickColor;
    
    private SpriteRenderer _spriteRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentGearEquipped = defaultGearToEquip;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = skateboardColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && (int)currentGearEquipped != 0)
        {
            currentGearEquipped = GearType.Skateboard;
            _spriteRenderer.color = skateboardColor;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && (int)currentGearEquipped != 1)
        {
            currentGearEquipped = GearType.RollerSkates;
            _spriteRenderer.color = rollerSkatesColor;   
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3) && (int)currentGearEquipped != 2)
        {
            currentGearEquipped = GearType.PogoStick;
            _spriteRenderer.color = pogoStickColor;
        }
    }
}

public enum GearType
{
    Skateboard = 0,
    RollerSkates = 1,
    PogoStick = 2
}
