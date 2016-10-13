using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

using FourthExercise.Services;
using FourthExercise.Infrastructure;
using FourthExercise.Infrastructure.Repositories;
using FourthExercise.Infrastructure.Entity;
using FourthExercise.Infrastructure.Entity.Mappers;
using FourthExercise.Infrastructure.Entity.Repositories;

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

            containerBuilder.RegisterType<FourthExerciseContext>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FourthExerciseUnitOfWorkFactory>().As<IUnitOfWorkFactory>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<JobRoleMapper>().As<IJobRoleMapper>().SingleInstance();
            containerBuilder.RegisterType<EmployeeMapper>().As<IEmployeeMapper>().SingleInstance();
            containerBuilder.RegisterType<JobRoleRepository>().As<IReadJobRoleRepository>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<EmployeeRepository>().As<IReadEmployeeRepository>().As<IWriteEmployeeRepository>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<CreateEmployeeService>().As<ICreateEmployeeService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ChangeEmployeeService>().As<IChangeEmployeeService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DeleteEmployeeService>().As<IDeleteEmployeeService>().InstancePerLifetimeScope();

            IContainer container = containerBuilder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
