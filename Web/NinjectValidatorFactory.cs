namespace FluentValidationPlayground.Web {
	using System;
	using FluentValidation;
	using FluentValidationPlayground.Rest;

	public class NinjectValidatorFactory : ValidatorFactoryBase {

		public override IValidator CreateInstance(Type validatorType) {
			if (!ServiceLocator.HasService(validatorType)) {
				// There isn't a validator for this entity so everything is valid
				// https://github.com/ninject/Ninject.Web.Mvc.FluentValidation/blob/master/src/Ninject.Web.Mvc.FluentValidation/NinjectValidatorFactory.cs
				return null;
			}
			return ServiceLocator.GetService(validatorType) as IValidator;
		}

	}
}
