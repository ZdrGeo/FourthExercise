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

        private async Task<IEnumerable<JobRoleModel>> GetJobRolesAsync()
        {
            IEnumerable<JobRoleModel> jobRoleModels = new List<JobRoleModel>();

            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    readJobRoleRepository.Enlist(uow);
                    jobRoleModels = await readJobRoleRepository.GetAllAsync();
                    readJobRoleRepository.Delist();
                }
            );

            return jobRoleModels;
        }

        private async Task<IEnumerable<EmployeeModel>> FindEmployeesWithNameAsync(string name)
        {
            IEnumerable<EmployeeModel> employeeModels = new List<EmployeeModel>();

            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    readEmployeeRepository.Enlist(uow);
                    employeeModels = await readEmployeeRepository.FindWithNameAsync(name);
                    readEmployeeRepository.Delist();
                }
            );

            return employeeModels;
        }

        private async Task<EmployeeModel> GetEmployeeAsync(int id)
        {
            EmployeeModel employeeModel = null;

            await unitOfWorkFactory.WithAsync(
                async uow =>
                {
                    readEmployeeRepository.Enlist(uow);
                    employeeModel = await readEmployeeRepository.GetAsync(id);
                    readEmployeeRepository.Delist();
                }
            );

            return employeeModel;
        }

        #endregion

        public async Task<ActionResult> Index(string currentName, string name)
        {
            if (name == null) { name = currentName; }

            IEnumerable<EmployeeModel> employeeModels = await FindEmployeesWithNameAsync(name);

            ViewBag.CurrentName = name;

            return View(employeeModels);
        }

        public async Task<ActionResult> Details(int? id, string currentName)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            EmployeeModel employeeModel = await GetEmployeeAsync(id ?? 0);

            if (employeeModel == null) { return HttpNotFound(); }

            ViewBag.CurrentName = currentName;

            return View(employeeModel);
        }

        public async Task<ActionResult> Create(string currentName)
        {
            IEnumerable<JobRoleModel> jobRoleModels = await GetJobRolesAsync();

            ViewBag.JobRoleId = new SelectList(jobRoleModels, "Id", "Name");
            ViewBag.CurrentName = currentName;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FirstName,LastName,Email,JobRoleId,Salary")] EmployeeModel employeeModel)
        {
            if (ModelState.IsValid)
            {
                await createEmployeeService.CreateAsync(employeeModel);

                return RedirectToAction("Index");
            }

            IEnumerable<JobRoleModel> jobRoleModels = await GetJobRolesAsync();

            ViewBag.JobRoleId = new SelectList(jobRoleModels, "Id", "Name", employeeModel.JobRoleId);

            return View(employeeModel);
        }

        public async Task<ActionResult> Edit(int? id, string currentName)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            EmployeeModel employeeModel = await GetEmployeeAsync(id ?? 0);

            if (employeeModel == null) { return HttpNotFound(); }

            IEnumerable<JobRoleModel> jobRoleModels = await GetJobRolesAsync();

            ViewBag.JobRoleId = new SelectList(jobRoleModels, "Id", "Name", employeeModel.JobRoleId);
            ViewBag.CurrentName = currentName;

            return View(employeeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FirstName,LastName,Email,JobRoleId,Salary")] EmployeeModel employeeModel)
        {
            if (ModelState.IsValid)
            {
                await changeEmployeeService.ChangeAsync(employeeModel);

                return RedirectToAction("Index");
            }

            IEnumerable<JobRoleModel> jobRoleModels = await GetJobRolesAsync();

            ViewBag.JobRoleId = new SelectList(jobRoleModels, "Id", "Name", employeeModel.JobRoleId);

            return View(employeeModel);
        }

        public async Task<ActionResult> Delete(int? id, string currentName)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            EmployeeModel employeeModel = await GetEmployeeAsync(id ?? 0);

            if (employeeModel == null) { return HttpNotFound(); }

            ViewBag.CurrentName = currentName;

            return View(employeeModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EmployeeModel employeeModel = await GetEmployeeAsync(id);

            await deleteEmployeeService.DeleteAsync(employeeModel);

            return RedirectToAction("Index");
        }
    }
}
