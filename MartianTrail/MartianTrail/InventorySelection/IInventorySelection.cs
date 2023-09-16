using MartianTrail.Entities;
using MartianTrail.PlayerInteraction;

namespace MartianTrail.InventorySelection
{
    public interface IInventorySelection
    {
        InventoryState SelectInitialInventory(IPlayerInteraction playerInteraction);
    }
}
