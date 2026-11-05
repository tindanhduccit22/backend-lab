namespace final.Models
{
    public class Role
    {
        public string RoleId { get; set; }
        public string Name { get; set; }
        public string descriptin { get; set; }

        public virtual ICollection<UserRole> PurchaseOrders { get; set; } = new List<UserRole>();
    }
}
