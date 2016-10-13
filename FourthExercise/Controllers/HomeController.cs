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
    public class HomeController : Controller
    {
        public HomeController(
            IReadJobRoleRepository readJobRoleRepository,
            IReadEmployeeRepository readEmployeeRepository,
            ICreateEmployeeService createEmployeeService,
            IChangeEmployeeService changeEmployeeService,
            IDeleteEmployeeService deleteEmployeeService
        )
        {
            if (readJobRoleRepository == null) { throw new ArgumentNullException("readJobRoleRepository"); }
            if (readEmployeeRepository == null) { throw new ArgumentNullException("readEmployeeRepository"); }
            if (createEmployeeService == null) { throw new ArgumentNullException("createEmployeeService"); }
            if (changeEmployeeService == null) { throw new ArgumentNullException("changeEmployeeService"); }
            if (deleteEmployeeService == null) { throw new ArgumentNullException("deleteEmployeeService"); }

            this.readJobRoleRepository = readJobRoleRepository;
            this.readEmployeeRepository = readEmployeeRepository;
            this.createEmployeeService = createEmployeeService;
            this.changeEmployeeService = changeEmployeeService;
            this.deleteEmployeeService = deleteEmployeeService;
        }

        private IReadJobRoleRepository readJobRoleRepository;
        private IReadEmployeeRepository readEmployeeRepository;
        private ICreateEmployeeService createEmployeeService;
        private IChangeEmployeeService changeEmployeeService;
        private IDeleteEmployeeService deleteEmployeeService;

        public async Task<ActionResult> Index(string currentName, string name)
        {
            if (name == null) { name = currentName; }

            IEnumerable<EmployeeModel> employeeModels = await readEmployeeRepository.FindWithNameAsync(name);

            ViewBag.CurrentName = name;

            return View(employeeModels);
        }

        public async Task<ActionResult> Details(int? id, string currentName)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            EmployeeModel employeeModel = await readEmployeeRepository.GetAsync(id ?? 0);

            if (employeeModel == null) { return HttpNotFound(); }

            ViewBag.CurrentName = currentName;

            return View(employeeModel);
        }

        public async Task<ActionResult> Create(string currentName)
        {
            IEnumerable<JobRoleModel> jobRoleModels = await readJobRoleRepository.GetAllAsync();

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

            IEnumerable<JobRoleModel> jobRoleModels = await readJobRoleRepository.GetAllAsync();

            ViewBag.JobRoleId = new SelectList(jobRoleModels, "Id", "Name", employeeModel.JobRoleId);

            return View(employeeModel);
        }

        public async Task<ActionResult> Edit(int? id, string currentName)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            EmployeeModel employeeModel = await readEmployeeRepository.GetAsync(id ?? 0);

            if (employeeModel == null) { return HttpNotFound(); }

            IEnumerable<JobRoleModel> jobRoleModels = await readJobRoleRepository.GetAllAsync();

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

            IEnumerable<JobRoleModel> jobRoleModels = await readJobRoleRepository.GetAllAsync();

            ViewBag.JobRoleId = new SelectList(jobRoleModels, "Id", "Name", employeeModel.JobRoleId);

            return View(employeeModel);
        }

        public async Task<ActionResult> Delete(int? id, string currentName)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            EmployeeModel employeeModel = await readEmployeeRepository.GetAsync(id ?? 0);

            if (employeeModel == null) { return HttpNotFound(); }

            ViewBag.CurrentName = currentName;

            return View(employeeModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EmployeeModel employeeModel = await readEmployeeRepository.GetAsync(id);

            await deleteEmployeeService.DeleteAsync(employeeModel);

            return RedirectToAction("Index");
        }
    }
}