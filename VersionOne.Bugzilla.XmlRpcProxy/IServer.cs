/***************************************************************************************
 * This code uses XML-RPC.NET Copyright (c) 2006 Charles Cook
***************************************************************************************/
using CookComputing.XmlRpc;

namespace VersionOne.Bugzilla.XmlRpcProxy
{
    public struct GetBugResult
    {
        public int id;
        public string name;
    	public string description;
        public int productid;
        public int componentid;
    	public int assignedtoid;
        public string priority;
    }

	public struct GetProductResult
	{
		public GetProductResult(int id, string name, string description)
		{
			this.id = id;
			this.name = name;
			this.description = description;
		}

		public int id;
		public string name;
		public string description;
	}

	public struct GetUserResult
	{
		public GetUserResult(int id, string name, string login)
		{
			this.id = id;
			this.name = name;
			this.login = login;
		}

		public int id;
		public string name;
		public string login;
	}

    public interface IServer : IXmlRpcProxy
    {
        [XmlRpcMethod("V1.Version")]
        string Version();

        [XmlRpcMethod("User.login")]
		XmlRpcStruct Login(XmlRpcStruct args);

        [XmlRpcMethod("V1.GetBugs")]
        int[] GetBugs(XmlRpcStruct args);

        [XmlRpcMethod("V1.GetBug")]
        GetBugResult GetBug(XmlRpcStruct args);

		[XmlRpcMethod("V1.AcceptBug")]
        bool AcceptBug(XmlRpcStruct args);

		[XmlRpcMethod("V1.ResolveBug")]
        bool ResolveBug(XmlRpcStruct args);
		
		[XmlRpcMethod("V1.ReassignBug")]
        bool ReassignBug(XmlRpcStruct args);
		
		[XmlRpcMethod("V1.UpdateBug")]
        bool UpdateBug(XmlRpcStruct args);

    	[XmlRpcMethod("V1.GetProduct")]
        GetProductResult GetProduct(XmlRpcStruct args);

		[XmlRpcMethod("V1.GetUser")]
        GetUserResult GetUser(XmlRpcStruct args);

        [XmlRpcMethod("User.logout")]
        void Logout();

        [XmlRpcMethod("V1.GetFieldValue")]
        string GetFieldValue(XmlRpcStruct args);
    }
}