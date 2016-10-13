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
using FourthExercise.Infrastructure.Entity.Repositories;
using FourthExercise.Services;

namespace FourthExercise.Specs.Steps
{
    public enum SeededJobRoleId
    {
        Employee = 1,
        Manager = 2,
        ManagersManager = 3
    }

    [Binding]
    public class CreateEmployeeSteps
    {
        private const string firstName = "Zdravko";

        private IUnitOfWorkFactory unitOfWorkFactory;
        private IReadEmployeeRepository readEmployeeRepository;
        private IWriteEmployeeRepository writeEmployeeRepository;
        private ICreateEmployeeService createEmployeeService;

        private EmployeeModel employeeModel;

        [Given(@"an user has created a new employee")]
        public void GivenAnUserHasCreatedANewEmployee()
        {
            FourthExerciseContext context = new FourthExerciseContext();

            unitOfWorkFactory = new FourthExerciseUnitOfWorkFactory(context);
            readEmployeeRepository = new EmployeeRepository(context);
            writeEmployeeRepository = new EmployeeRepository(context);
            createEmployeeService = new CreateEmployeeService(unitOfWorkFactory, writeEmployeeRepository);

            employeeModel = new EmployeeModel();
        }

        [Given(@"he entered all required information")]
        public void GivenHeEnteredAllRequiredInformation()
        {
            employeeModel.FirstName = firstName;
            employeeModel.LastName = "Georgiev";
            employeeModel.Email = "z_georgiev@applss.com";
            employeeModel.JobRoleId = (int)SeededJobRoleId.Employee;
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
            int count = (await readEmployeeRepository.FindWithNameAsync(firstName)).Count();

            Assert.IsTrue(count > 0);
        }
    }
}
