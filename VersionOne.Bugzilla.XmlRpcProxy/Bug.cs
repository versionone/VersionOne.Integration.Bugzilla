namespace VersionOne.Bugzilla.XmlRpcProxy
{
    public class Bug
    {
        public readonly int ID;
        public readonly string Name;
    	public readonly string Description;
        public readonly int ProductID;
        public readonly int ComponentID;
    	public readonly int AssignedToID;
        public readonly string Priority;

        private Bug(GetBugResult result)
        {
            ID = result.id;
            Name = result.name;
        	Description = result.description;
            ProductID = result.productid;
            ComponentID = result.componentid;
        	AssignedToID = result.assignedtoid;
            Priority = result.priority;
        }

        public static Bug Create(GetBugResult result)
        {
            return new Bug(result);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", ID, Name);
        }
    }
}
