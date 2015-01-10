namespace VersionOne.Bugzilla.XmlRpcProxy
{
	public class User
	{
		public readonly int ID;
		public readonly string Name;
		public readonly string Login;

		private User(GetUserResult result)
		{
			ID = result.id;
			Name = result.name;
			Login = result.login;
		}

		public static User Create(GetUserResult result)
		{
			return new User(result);
		}

		public override string ToString()
		{
			return string.Format("{0} - {1}", ID, Name);
		}
	}
}