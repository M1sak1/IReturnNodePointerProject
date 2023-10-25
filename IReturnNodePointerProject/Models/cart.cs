namespace IReturnNodePointerProject.Models
{
	public class cart
	{
        public cart()
        {
            //just adding making the object
            productIDs = new List<int>();
        }
        public List<int> productIDs { get; set; }
	}
}
