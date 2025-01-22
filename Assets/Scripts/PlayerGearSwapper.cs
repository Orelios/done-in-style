using UnityEngine;

public class PlayerGearSwapper : MonoBehaviour
{
    [SerializeField] private DaredevilGearSO defaultGearToEquip;
    private DaredevilGearSO _currentGearEquipped;
    public DaredevilGearSO CurrentGearEquipped => _currentGearEquipped;
    
    [SerializeField] private float swapCooldown;
    private float lastSwapTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentGearEquipped = defaultGearToEquip;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapGear(DaredevilGearSO gearToSwapTo)
    {
        if (Time.time < lastSwapTime + swapCooldown)
            return;
        
        lastSwapTime = Time.time;
        
        _currentGearEquipped = gearToSwapTo;
    }
}
