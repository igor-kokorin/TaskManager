using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain;
using TaskManager.Web.Models;
using TaskManager.Web.Utilities;

namespace TaskManager.Controllers
{
    public class TasksController : Controller
    {
        private readonly IWorkItemRepository repo;

        public TasksController(IWorkItemRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var items = await repo.GetWorkItems();

            return View(Mappers.Map(items));
        }

        public async Task<IActionResult> Display(int id)
        {
            var item = await repo.GetWorkItem(id);

            if (item == null)
            {
                return NotFound();
            }

            return PartialView("_SingleTaskView", Mappers.Map(item));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await repo.GetWorkItem(id);

            if (item == null)
            {
                return NotFound();
            }

            return View("DeleteConfirmation", Mappers.Map(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitDelete(int id)
        {
            var workItem = await repo.GetWorkItem(id);

            if (workItem == null)
            {
                return NotFound();
            }

            if (!workItem.CanDelete())
            {
                return BadRequest();
            }

            await repo.DeleteWorkItem(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var workItem = await repo.GetWorkItem(id);

            return View(Mappers.MapForUpdate(workItem));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitEdit(int id, WorkItemUpdateDto dto)
        {
            var workItem = await repo.GetWorkItem(id);

            if (workItem == null)
            {
                throw new WorkItemNotFound();
            }

            workItem.SetTitle(dto.Title);
            workItem.SetDescription(dto.Description);
            workItem.SetExecutors(dto.Executors);
            workItem.SetPlannedExecutionTime(WorkItemExecutionTime.FromTimeSpan(dto.PlannedExecutionTime));
            workItem.SetActualExecutionTime(WorkItemExecutionTime.FromTimeSpan(dto.ActualExecutionTime));
            workItem.SetStatus(dto.Status);

            await repo.UpdateWorkItem(workItem);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create(int? parentId)
        {
            return View(new WorkItemCreateDto() { ParentId = parentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitCreate(WorkItemCreateDto dto)
        {
            var workItem = WorkItem.Create(Mappers.Map(dto));

            await repo.CreateWorkItem(workItem);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
