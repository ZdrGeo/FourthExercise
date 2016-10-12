using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

using FourthExercise.Services;
using FourthExercise.DataServices;
using FourthExercise.DataServices.Repositories;
using FourthExercise.DataServices.Entity.Repositories;
using FourthExercise.DataServices.Entity;

namespace FourthExercise
{
    public class AutofacConfig
    {
        public static void RegisterDependencies()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterControllers(typeof(MvcApplication).Assembly);
            containerBuilder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            containerBuilder.RegisterModelBinderProvider();
            containerBuilder.RegisterModule<AutofacWebTypesModule>();
            containerBuilder.RegisterSource(new ViewRegistrationSource());
            containerBuilder.RegisterFilterProvider();

            containerBuilder.RegisterType<FourthExerciseUnitOfWorkFactory>().As<IUnitOfWorkFactory>();
            containerBuilder.RegisterType<JobRoleRepository>().As<IJobRoleRepository>();
            containerBuilder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>();
            containerBuilder.RegisterType<CreateEmployeeService>().As<ICreateEmployeeService>();
            containerBuilder.RegisterType<ChangeEmployeeService>().As<IChangeEmployeeService>();
            containerBuilder.RegisterType<DeleteEmployeeService>().As<IDeleteEmployeeService>();

            IContainer container = containerBuilder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
