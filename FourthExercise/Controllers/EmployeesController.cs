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
using FourthExercise.DataServices;
using FourthExercise.DataServices.Repositories;

namespace FourthExercise.Controllers
{
    public class EmployeesController : Controller
    {
        public EmployeesController(
            IUnitOfWorkFactory unitOfWorkFactory,
            IJobRoleRepository jobRoleRepository,
            IEmployeeRepository employeeRepository,
            ICreateEmployeeService createEmployeeService,
            IChangeEmployeeService changeEmployeeService,
            IDeleteEmployeeService deleteEmployeeService
        )
        {
            if (unitOfWorkFactory == null) { throw new ArgumentNullException("unitOfWorkFactory"); }
            if (jobRoleRepository == null) { throw new ArgumentNullException("jobRoleRepository"); }
            if (employeeRepository == null) { throw new ArgumentNullException("employeeRepository"); }
            if (createEmployeeService == null) { throw new ArgumentNullException("createEmployeeService"); }
            if (changeEmployeeService == null) { throw new ArgumentNullException("changeEmployeeService"); }
            if (deleteEmployeeService == null) { throw new ArgumentNullException("deleteEmployeeService"); }

            this.unitOfWorkFactory = unitOfWorkFactory;
            this.jobRoleRepository = jobRoleRepository;
            this.employeeRepository = employeeRepository;
            this.createEmployeeService = createEmployeeService;
            this.changeEmployeeService = changeEmployeeService;
            this.deleteEmployeeService = deleteEmployeeService;
        }

        private IUnitOfWorkFactory unitOfWorkFactory;
        private IJobRoleRepository jobRoleRepository;
        private IEmployeeRepository employeeRepository;
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
                    jobRoleRepository.Enlist(uow);
                    jobRoles = await jobRoleRepository.GetAllAsync();
                    jobRoleRepository.Delist();
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
                    employeeRepository.Enlist(uow);
                    employees = await employeeRepository.FindWithNameAsync(name);
                    employeeRepository.Delist();
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
                    employeeRepository.Enlist(uow);
                    employee = await employeeRepository.GetAsync(id);
                    employeeRepository.Delist();
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

        // GET: Employees/Details/5
        public async Task<ActionResult> Details(int? id, string currentName)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            Employee employee = await GetEmployeeAsync(id ?? 0);

            if (employee == null) { return HttpNotFound(); }

            ViewBag.CurrentName = currentName;

            return View(employee);
        }

        // GET: Employees/Create
        public async Task<ActionResult> Create(string currentName)
        {
            IEnumerable<JobRole> jobRoles = await GetJobRolesAsync();

            ViewBag.JobRoleId = new SelectList(jobRoles, "Id", "Name");

            ViewBag.CurrentName = currentName;

            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Employees/Edit/5
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

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Employees/Delete/5
        public async Task<ActionResult> Delete(int? id, string currentName)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            Employee employee = await GetEmployeeAsync(id ?? 0);

            if (employee == null) { return HttpNotFound(); }

            ViewBag.CurrentName = currentName;

            return View(employee);
        }

        // POST: Employees/Delete/5
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
