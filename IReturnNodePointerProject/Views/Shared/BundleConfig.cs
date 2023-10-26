using System.Web.Optimization;
using System.Web;
namespace IReturnNodePointerProject.Views.Shared
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
				"~/Scripts/jquery-{version}.js"));
			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
				"~/Scripts/bootstrap.js"));
		}
	}

}
