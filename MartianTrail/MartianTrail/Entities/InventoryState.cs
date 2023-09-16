namespace MartianTrail.Entities
{
    public record InventoryState
    {
        public int NumberOfBatteries { get; set; }
        public int Food { get; set; }
        public int Furs { get; set; }
        public int LaserCharges { get; set; }
        public int AtmosphereSuits { get; set; }
        public int MediPacks { get; set; }
        public int Credits { get; set; }
    }
}
