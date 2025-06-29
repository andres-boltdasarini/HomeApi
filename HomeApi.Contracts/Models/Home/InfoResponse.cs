namespace HomeApi.Contracts.Models.Home
{
    public class InfoResponse
    {
        public int FloorAmount { get; set; }
        public string? Telephone { get; set; }
        public string? Heating { get; set; }
        public string? Material { get; set; }
        public AddressInfo? AddressInfo { get; set; }
        public int CurrentVolts { get; set; }
        public bool GasConnected { get; set; }
        public int Area { get; set; }
    }

    public class AddressInfo
    {
        public string House { get; set; }
        public string Building { get; set; }
        public string Street { get; set; }
    }
}
