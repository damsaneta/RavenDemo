namespace Demo.Model.EF.Dtos
{
    public class ProductInventoryDto
    {
        public int ProductId { get; set; }

        public short LocationId { get; set; }

        public string Shelf { get; set; }

        public byte Bin { get; set; }

        public short Quantity { get; set; }

        public string LocationName { get; set; }

        public string ProductName { get; set; 
}
    }
}
