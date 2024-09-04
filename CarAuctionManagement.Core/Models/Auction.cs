namespace CarAuctionManagement.Core.Models
{
    public class Auction
    {
        public Guid Id { get; }
        public Guid VehicleId { get; }
        public bool IsActive { get; private set; }

        public Auction(Guid id, Guid vehicleId, bool isActive)
        {
            if (id == default)
                throw new ArgumentException($"Value can't be empty {nameof(Guid)}.", nameof(id));

            if (vehicleId == default)
                throw new ArgumentException($"Value can't be empty {nameof(Guid)}.", nameof(vehicleId));

            Id = id;
            VehicleId = vehicleId;
            IsActive = isActive;
        }

        public static Auction Create(Guid vehicleId)
        {
            return new Auction(Guid.NewGuid(), vehicleId, true);
        }
    }
}
