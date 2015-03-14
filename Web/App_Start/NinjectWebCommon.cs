[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(FluentValidationPlayground.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(FluentValidationPlayground.Web.App_Start.NinjectWebCommon), "Stop")]

namespace FluentValidationPlayground.Web.App_Start
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Web;
	using FluentValidation;
	using FluentValidationPlayground.Rest;
	using Microsoft.Web.Infrastructure.DynamicModuleHelper;
	using Ninject;
	using Ninject.Extensions.Conventions;
	using Ninject.Planning.Bindings;
	using Ninject.Web.Common;

	public static class NinjectWebCommon
	{
		private static readonly Bootstrapper bootstrapper = new Bootstrapper();

		/// <summary>
		/// Starts the application
		/// </summary>
		public static void Start()
		{
			DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
			DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
			bootstrapper.Initialize(CreateKernel);
		}

		/// <summary>
		/// Stops the application.
		/// </summary>
		public static void Stop()
		{
			bootstrapper.ShutDown();
		}

		/// <summary>
		/// Creates the kernel that will manage your application.
		/// </summary>
		/// <returns>The created kernel.</returns>
		private static IKernel CreateKernel()
		{
			var kernel = new StandardKernel();
			try
			{
				kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
				kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

				RegisterServices(kernel);
				return kernel;
			}
			catch
			{
				kernel.Dispose();
				throw;
			}
		}

		/// <summary>
		/// Load your modules or register your services here!
		/// </summary>
		/// <param name="kernel">The kernel.</param>
		private static void RegisterServices(IKernel kernel) {

			Type thisType = typeof(NinjectWebCommon);
			string path = new Uri(Path.GetDirectoryName(thisType.Assembly.CodeBase) ?? "").LocalPath;
			string thisNamespace = thisType.FullName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0]; // FRAGILE: ASSUME: All our code is in this namespace

			kernel.Bind(x => x
				.FromAssembliesInPath(path) // Blows with "not marked as serializable": , a => a.FullName.StartsWith( assemblyPrefix ) )
				.Select(type => type.IsClass && !type.IsAbstract && type.FullName.StartsWith(thisNamespace)) // .SelectAllClasses() wires up everyone else's stuff too
				.BindDefaultInterface()
				.Configure(b => b.InRequestScope())
			);

			kernel.Rebind<IValidator<Customer>>().To<CustomerValidator>();

			// Initialize the service locator
			// https://github.com/ninject/Ninject.Web.Mvc.FluentValidation/blob/master/src/Ninject.Web.Mvc.FluentValidation/NinjectValidatorFactory.cs
			ServiceLocator.Initialize(kernel.GetService, t => ((IList<IBinding>)kernel.GetBindings(t)).Count > 0);

		}
	}
}
