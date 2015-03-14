namespace FluentValidationPlayground.Web {
	using System.Web.Mvc;
	using System.Web.Optimization;
	using System.Web.Routing;
	using FluentValidation.Mvc;
	using FluentValidationPlayground.Web.App_Start;

	public class MvcApplication : System.Web.HttpApplication {
		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			// Rig FluentValidate into MVC via attributes:
			//This does it with `[Validator(typeof(PersonValidator))]` but we want to avoid that: FluentValidationModelValidatorProvider.Configure();

			// Rig FluentValidate into MVC via IoC
			NinjectValidatorFactory factory = new NinjectValidatorFactory();
			ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(factory));
			DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

		}
	}
}
