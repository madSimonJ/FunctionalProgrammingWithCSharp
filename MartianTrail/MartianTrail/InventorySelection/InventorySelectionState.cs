namespace MartianTrail.InventorySelection;

public record InventorySelectionState
{
    public int NumberOfBatteries { get; set; }
    public int Food { get; set; }
    public int LaserCharges { get; set; }
    public int AtmosphereSuits { get; set; }
    public int MediPacks { get; set; }
    public int Credits { get; set; }
    public bool PlayerIsHappyWithSelection { get; set; }
}