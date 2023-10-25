namespace IReturnNodePointerProject.Models
{
	public class prodAmalgam
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public Double price { get; set; }
		public string Author { get; set; }
		public string Genre { get; set; }
		public string Stock { get; set; }
		public int ProductID { get; set; }
		//this is only used in the cart, but true to the classes name, it is an amalgam
		public int Quantity { get; set; }
	}
}
