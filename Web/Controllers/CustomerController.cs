namespace FluentValidationPlayground.Web.Controllers {
	using System;
	using System.Collections.Generic;
	using System.Web.Mvc;
	using FluentValidation.Results;
	using FluentValidationPlayground.Rest;

	public class CustomerController : Controller {
		private readonly ICustomerRepository customerRepository;

		public CustomerController(ICustomerRepository CustomerRepository) {
			if (CustomerRepository == null) {
				throw new ArgumentNullException("CustomerRepository");
			}
			this.customerRepository = CustomerRepository;
		}

		public ActionResult Index() {
			List<Customer> all = this.customerRepository.GetAll();
			return View(all);
		}

		public ActionResult Edit(int id) {
			Customer customer = id < 1 ? new Customer() : this.customerRepository.GetById(id);
			if (customer == null) {
				return this.View("NotFound");
			}
			return this.View(customer);
		}

		[HttpPost]
		public ActionResult Edit(int id, Customer Customer) {
			Customer.CustomerId = id;

			if (!ModelState.IsValid) {
				return this.View(Customer); // fix your errors
			}

			/*
			CustomerValidator validator = new CustomerValidator();
			ValidationResult results = validator.Validate(Customer);

			bool validationSucceeded = results.IsValid;
			IList<ValidationFailure> failures = results.Errors;
			if (!validationSucceeded) {
				this.ModelState.AddModelError("FluentValidation", "FluentValidation must validate separately from the built-in MVC validation");
				foreach (ValidationFailure failure in failures) {
					this.ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
				}
				return this.View(Customer); // fix your errors
			}
			*/

			this.customerRepository.Save(Customer);

			return this.RedirectToAction("Index");
		}

	}
}
