namespace FluentValidationPlayground.Rest {
	using FluentValidation;

	public class Customer {
		public int CustomerId { get; set; }
		public string Surname { get; set; }
		public string Forename { get; set; }
		public bool HasDiscount { get; set; }
		public int Discount { get; set; }
		public string Address { get; set; }
		public string Postcode { get; set; }
	}

	public class CustomerValidator : AbstractValidator<Customer> {
		public CustomerValidator() {
			this.RuleFor(customer => customer.Surname).NotEmpty();
			this.RuleFor(customer => customer.Forename).NotEmpty().WithMessage("Please specify a first name");
			this.RuleFor(customer => customer.Discount).NotEqual(0).When(customer => customer.HasDiscount);
			this.RuleFor(customer => customer.Address).Length(20, 250);
			this.RuleFor(customer => customer.Postcode).Must(this.BeAValidPostcode).WithMessage("Please specify a valid postcode");
		}

		private bool BeAValidPostcode(string postcode) {
			int result;
			return !string.IsNullOrEmpty(postcode) && postcode.Length == 5 && int.TryParse(postcode, out result);
		}
	}
}
