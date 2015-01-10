namespace VersionOne.ServerConnector {
    public class SortBy {
        public readonly string Attribute;
        public readonly Order Order;

        private SortBy(string attribute, Order order) {
            Attribute = attribute;
            Order = order;
        }

        public static SortBy Ascending(string attribute) {
            return new SortBy(attribute, Order.Ascending);
        }

        public static SortBy Descending(string attribute) {
            return new SortBy(attribute, Order.Descending);
        }
    }
}
