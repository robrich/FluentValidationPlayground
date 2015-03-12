namespace FluentValidationPlayground.Win {
	using System;
	using System.Collections.Generic;
	using FluentValidation.Results;
	using FluentValidationPlayground.Rest;

	public static class Program {
		public static void Main(string[] args) {

			Customer customer = new Customer();
			CustomerValidator validator = new CustomerValidator();
			ValidationResult results = validator.Validate(customer);

			bool validationSucceeded = results.IsValid;
			Console.WriteLine("Customer is valid: " + validationSucceeded);
			IList<ValidationFailure> failures = results.Errors;

			foreach (ValidationFailure failure in failures) {
				Console.WriteLine(failure);
			}

			Console.WriteLine("Push any key to exit");
			Console.ReadKey();

		}
	}
}
