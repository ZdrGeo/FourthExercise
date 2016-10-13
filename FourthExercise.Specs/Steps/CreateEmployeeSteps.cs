using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

using FourthExercise.Models;
using FourthExercise.Infrastructure;
using FourthExercise.Infrastructure.Repositories;
using FourthExercise.Infrastructure.Entity;
using FourthExercise.Infrastructure.Entity.Mappers;
using FourthExercise.Infrastructure.Entity.Repositories;
using FourthExercise.Services;

namespace FourthExercise.Specs.Steps
{
    [Binding]
    public class CreateEmployeeSteps
    {
        public CreateEmployeeSteps()
        {
            context = new FourthExerciseContext();
            unitOfWorkFactory = new FourthExerciseUnitOfWorkFactory(context);
            jobRoleMapper = new JobRoleMapper();
            employeeMapper = new EmployeeMapper(jobRoleMapper);
            EmployeeRepository employeeRepository = new EmployeeRepository(context, employeeMapper);
            readEmployeeRepository = employeeRepository;
            writeEmployeeRepository = employeeRepository;
            createEmployeeService = new CreateEmployeeService(unitOfWorkFactory, writeEmployeeRepository);
        }

        private FourthExerciseContext context;
        private IUnitOfWorkFactory unitOfWorkFactory;
        private IJobRoleMapper jobRoleMapper;
        private IEmployeeMapper employeeMapper;
        private IReadEmployeeRepository readEmployeeRepository;
        private IWriteEmployeeRepository writeEmployeeRepository;
        private ICreateEmployeeService createEmployeeService;

        private EmployeeModel employeeModel;
        private string name;

        [Given(@"an user has created a new employee")]
        public void GivenAnUserHasCreatedANewEmployee()
        {
            employeeModel = new EmployeeModel();
        }

        [Given(@"he entered all required information")]
        public void GivenHeEnteredAllRequiredInformation()
        {
            name = $"Zdravko_{ Guid.NewGuid() }";

            employeeModel.FirstName = name;
            employeeModel.LastName = "Georgiev";
            employeeModel.Email = "z_georgiev@applss.com";
            employeeModel.JobRoleId = (int)FourthExerciseSeed.JobRoleId.Employee;
            employeeModel.Salary = 4600;
        }

        [When(@"he completes the process")]
        public async Task WhenHeCompletesTheProcess()
        {
            await createEmployeeService.CreateAsync(employeeModel);
        }

        [Then(@"that employee should be present within the system")]
        public async Task ThenThatEmployeeShouldBePresentWithinTheSystem()
        {
            int count = (await readEmployeeRepository.FindWithNameAsync(name)).Count();

            Assert.IsTrue(count > 0);
        }
    }
}
