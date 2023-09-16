using MartianTrail.Common;

namespace MartianTrail.Entities
{
    public record GameState
    {
        public int DistanceTraveled { get; set; }
        public bool ReachedDestination { get; set; }
        public bool PlayerIsDead { get; set; }
        public int CurrentSol { get; set; }
        public PlayerActions UserActionSelectedThisTurn { get; set; }
        public InventoryState Inventory { get; set; }
    }
}
