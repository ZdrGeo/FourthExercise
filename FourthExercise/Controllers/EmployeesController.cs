using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FourthExercise.Models;
using FourthExercise.DataServices;
using FourthExercise.DataServices.Repositories;

namespace FourthExercise.Controllers
{
    public class EmployeesController : Controller
    {
        public EmployeesController(IUnitOfWorkFactory unitOfWorkFactory, IEmployeeRepository employeeRepository, IJobRoleRepository jobRoleRepository)
        {
            if (unitOfWorkFactory == null) { throw new ArgumentNullException("unitOfWorkFactory"); }
            if (employeeRepository == null) { throw new ArgumentNullException("employeeRepository"); }
            if (jobRoleRepository == null) { throw new ArgumentNullException("jobRoleRepository"); }

            this.unitOfWorkFactory = unitOfWorkFactory;
            this.employeeRepository = employeeRepository;
            this.jobRoleRepository = jobRoleRepository;
        }

        private IUnitOfWorkFactory unitOfWorkFactory;
        private IEmployeeRepository employeeRepository;
        private IJobRoleRepository jobRoleRepository;

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

        // GET: Employees
        public async Task<ActionResult> Index()
        {
            IEnumerable<Employee> employees = new List<Employee>();

            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    employeeRepository.Enlist(uow);
                    employees = await employeeRepository.GetAllAsync();
                    employeeRepository.Delist();
                }
            );

            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            Employee employee = await GetEmployeeAsync(id ?? 0);

            if (employee == null) { return HttpNotFound(); }

            return View(employee);
        }

        // GET: Employees/Create
        public async Task<ActionResult> Create()
        {
            IEnumerable<JobRole> jobRoles = await GetJobRolesAsync();

            ViewBag.JobRoleId = new SelectList(jobRoles, "Id", "Name");

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
                await unitOfWorkFactory.WithAsync(
                    async uow =>
                    {
                        employeeRepository.Enlist(uow);
                        await employeeRepository.AddAsync(employee);
                        await uow.CompleteAsync();
                        employeeRepository.Delist();
                    }
                );

                return RedirectToAction("Index");
            }

            IEnumerable<JobRole> jobRoles = await GetJobRolesAsync();

            ViewBag.JobRoleId = new SelectList(jobRoles, "Id", "Name", employee.JobRoleId);

            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            Employee employee = await GetEmployeeAsync(id ?? 0);

            if (employee == null) { return HttpNotFound(); }

            IEnumerable<JobRole> jobRoles = await GetJobRolesAsync();

            ViewBag.JobRoleId = new SelectList(jobRoles, "Id", "Name", employee.JobRoleId);

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
                await unitOfWorkFactory.WithAsync(
                    async uow =>
                    {
                        employeeRepository.Enlist(uow);
                        await employeeRepository.SetAsync(employee);
                        await uow.CompleteAsync();
                        employeeRepository.Delist();
                    }
                );

                return RedirectToAction("Index");
            }

            IEnumerable<JobRole> jobRoles = await GetJobRolesAsync();

            ViewBag.JobRoleId = new SelectList(jobRoles, "Id", "Name", employee.JobRoleId);

            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            Employee employee = await GetEmployeeAsync(id ?? 0);

            if (employee == null) { return HttpNotFound(); }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    employeeRepository.Enlist(uow);
                    Employee employee = await employeeRepository.GetAsync(id);
                    await employeeRepository.RemoveAsync(employee);
                    await uow.CompleteAsync();
                    employeeRepository.Delist();
                }
            );

            return RedirectToAction("Index");
        }
    }
}
