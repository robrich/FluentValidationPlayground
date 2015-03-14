namespace FluentValidationPlayground.Rest {
	using System.Collections.Generic;
	using System.Linq;

	public interface ICustomerRepository {
		List<Customer> GetAll();
		Customer GetById(int CustomerId);
		void Save(Customer Customer);
	}

	public class CustomerRepository : ICustomerRepository {
		private static readonly List<Customer> allCustomers = new List<Customer>();
 
		public List<Customer> GetAll() {
			return allCustomers.ToList(); // new array pointing to the same objects
		}

		public Customer GetById(int CustomerId) {
			return (
				from c in allCustomers
				where c.CustomerId == CustomerId
				select c
			).FirstOrDefault();
		}

		public void Save(Customer Customer) {
			if (Customer.CustomerId < 1) {
				// Invent an auto-incrementing id
				if (allCustomers.Count == 0) {
					Customer.CustomerId = 1;
				} else {
					Customer.CustomerId = (
						from c in allCustomers
						orderby c.CustomerId descending
						select c.CustomerId
					).First() + 1;
				}
			} else {
				// Remove the one we're about to replace if any
				Customer dbCustomer = this.GetById(Customer.CustomerId);
				if (dbCustomer != null) {
					allCustomers.Remove(dbCustomer);
				}
			}
			allCustomers.Add(Customer);
		}

	}
}
