using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;

using FourthExercise.Models;
using FourthExercise.Services;
using FourthExercise.Infrastructure;
using FourthExercise.Infrastructure.Repositories;

namespace FourthExercise.Controllers
{
    public class EmployeesController : Controller
    {
        public EmployeesController(
            IUnitOfWorkFactory unitOfWorkFactory,
            IReadJobRoleRepository readJobRoleRepository,
            IReadEmployeeRepository readEmployeeRepository,
            ICreateEmployeeService createEmployeeService,
            IChangeEmployeeService changeEmployeeService,
            IDeleteEmployeeService deleteEmployeeService
        )
        {
            if (unitOfWorkFactory == null) { throw new ArgumentNullException("unitOfWorkFactory"); }
            if (readJobRoleRepository == null) { throw new ArgumentNullException("readJobRoleRepository"); }
            if (readEmployeeRepository == null) { throw new ArgumentNullException("readEmployeeRepository"); }
            if (createEmployeeService == null) { throw new ArgumentNullException("createEmployeeService"); }
            if (changeEmployeeService == null) { throw new ArgumentNullException("changeEmployeeService"); }
            if (deleteEmployeeService == null) { throw new ArgumentNullException("deleteEmployeeService"); }

            this.unitOfWorkFactory = unitOfWorkFactory;
            this.readJobRoleRepository = readJobRoleRepository;
            this.readEmployeeRepository = readEmployeeRepository;
            this.createEmployeeService = createEmployeeService;
            this.changeEmployeeService = changeEmployeeService;
            this.deleteEmployeeService = deleteEmployeeService;
        }

        private IUnitOfWorkFactory unitOfWorkFactory;
        private IReadJobRoleRepository readJobRoleRepository;
        private IReadEmployeeRepository readEmployeeRepository;
        private ICreateEmployeeService createEmployeeService;
        private IChangeEmployeeService changeEmployeeService;
        private IDeleteEmployeeService deleteEmployeeService;

        #region

        private async Task<IEnumerable<JobRole>> GetJobRolesAsync()
        {
            IEnumerable<JobRole> jobRoles = new List<JobRole>();

            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    readJobRoleRepository.Enlist(uow);
                    jobRoles = await readJobRoleRepository.GetAllAsync();
                    readJobRoleRepository.Delist();
                }
            );

            return jobRoles;
        }

        private async Task<IEnumerable<Employee>> FindEmployeesWithNameAsync(string name)
        {
            IEnumerable<Employee> employees = new List<Employee>();

            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    readEmployeeRepository.Enlist(uow);
                    employees = await readEmployeeRepository.FindWithNameAsync(name);
                    readEmployeeRepository.Delist();
                }
            );

            return employees;
        }

        private async Task<Employee> GetEmployeeAsync(int id)
        {
            Employee employee = null;

            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    readEmployeeRepository.Enlist(uow);
                    employee = await readEmployeeRepository.GetAsync(id);
                    readEmployeeRepository.Delist();
                }
            );

            return employee;
        }

        #endregion

        public async Task<ActionResult> Index(string currentName, string name)
        {
            if (name == null) { name = currentName; }

            IEnumerable<Employee> employees = await FindEmployeesWithNameAsync(name);

            ViewBag.CurrentName = name;

            return View(employees);
        }

        public async Task<ActionResult> Details(int? id, string currentName)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            Employee employee = await GetEmployeeAsync(id ?? 0);

            if (employee == null) { return HttpNotFound(); }

            ViewBag.CurrentName = currentName;

            return View(employee);
        }

        public async Task<ActionResult> Create(string currentName)
        {
            IEnumerable<JobRole> jobRoles = await GetJobRolesAsync();

            ViewBag.JobRoleId = new SelectList(jobRoles, "Id", "Name");

            ViewBag.CurrentName = currentName;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FirstName,LastName,Email,JobRoleId,Salary")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                await createEmployeeService.CreateAsync(employee);

                return RedirectToAction("Index");
            }

            IEnumerable<JobRole> jobRoles = await GetJobRolesAsync();

            ViewBag.JobRoleId = new SelectList(jobRoles, "Id", "Name", employee.JobRoleId);

            return View(employee);
        }

        public async Task<ActionResult> Edit(int? id, string currentName)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            Employee employee = await GetEmployeeAsync(id ?? 0);

            if (employee == null) { return HttpNotFound(); }

            IEnumerable<JobRole> jobRoles = await GetJobRolesAsync();

            ViewBag.JobRoleId = new SelectList(jobRoles, "Id", "Name", employee.JobRoleId);
            ViewBag.CurrentName = currentName;

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FirstName,LastName,Email,JobRoleId,Salary")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                await changeEmployeeService.ChangeAsync(employee);

                return RedirectToAction("Index");
            }

            IEnumerable<JobRole> jobRoles = await GetJobRolesAsync();

            ViewBag.JobRoleId = new SelectList(jobRoles, "Id", "Name", employee.JobRoleId);

            return View(employee);
        }

        public async Task<ActionResult> Delete(int? id, string currentName)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            Employee employee = await GetEmployeeAsync(id ?? 0);

            if (employee == null) { return HttpNotFound(); }

            ViewBag.CurrentName = currentName;

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Employee employee = await GetEmployeeAsync(id);

            await deleteEmployeeService.DeleteAsync(employee);

            return RedirectToAction("Index");
        }
    }
}
