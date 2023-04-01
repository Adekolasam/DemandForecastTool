using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemandForecastTool.Context;
using DemandForecastTool.Models;

namespace DemandForecastTool.Controllers
{
    public class DemandController : Controller
    {
        private readonly AppDbContext _context;

        public DemandController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            List<ReadResourceDemandModel> requests = await (from r in _context.ResourceRequests
                                               join e in _context.Environments on r.EnvironmentId equals e.Id
                                               join d in _context.DataCentres on r.DataCentreId equals d.Id
                                               join rt in _context.ResourceTypes on r.ResourceTypeId equals rt.Id
                                               select new ReadResourceDemandModel {
                                                   Id = r.Id,
                                                   RequestedDate = String.Format("{0:dd MMMM, yyyy}", r.RequestedDate),
                                                   Environment = e.Name,
                                                   DataCentre = d.Name,
                                                   ResourceType = rt.Name,
                                                   CTS = r.CTS,
                                                   EIS = r.EIS,
                                                   GMB = r.GMB,
                                                   GMIT = r.GMIT,
                                                   RISK = r.RISK
                                               }).ToListAsync();
              return View(requests);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ResourceRequests == null)
            {
                return NotFound();
            }

            var resourceRequest = await _context.ResourceRequests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resourceRequest == null)
            {
                return NotFound();
            }

            return View(resourceRequest);
        }


        public async Task<IActionResult> Create()
        {
            await SetFormData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnvironmentId,DataCentreId,ResourceTypeId,CTS,EIS,GMB,GMIT,RISK")] ResourceDemandModel resourceRequest)
        {
            if (ModelState.IsValid)
            {
                ResourceRequest request = new ResourceRequest
                {
                    CTS = resourceRequest.CTS,
                    EIS = resourceRequest.EIS,
                    GMB = resourceRequest.GMB,
                    GMIT = resourceRequest.GMIT,
                    RISK = resourceRequest.RISK,
                    RequestedDate = DateTime.UtcNow,
                    DataCentreId = resourceRequest.DataCentreId,
                    EnvironmentId = resourceRequest.EnvironmentId,
                    ResourceTypeId = resourceRequest.ResourceTypeId
                };
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await SetFormData();
            return View(resourceRequest);
        }

        //private async Task<bool> ValidateEnvironmentDemandOnSubmit(ResourceDemandModel resourceRequest)
        //{
        //    var t = await _context.Environments
        //    return true;
        //}

        private async Task SetFormData()
        {
            List<SelectItemModel> Environments = (
                    await _context.Environments.Select(x => new SelectItemModel { Text = x.Name, Value = x.Id }).ToListAsync()
                );
            ViewData["Environment"] = Environments;
            List<SelectItemModel> DataCentre = (
                    await _context.DataCentres.Select(x => new SelectItemModel { Text = x.Name, Value = x.Id }).ToListAsync()
                );
            ViewData["DataCentre"] = DataCentre;
            List<SelectItemModel> ResourceType = (
                    await _context.ResourceTypes.Select(x => new SelectItemModel { Text = x.Name, Value = x.Id }).ToListAsync()
                );
            ViewData["ResourceType"] = ResourceType;
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ResourceRequests == null)
            {
                return NotFound();
            }

            var resourceRequest = await _context.ResourceRequests.FindAsync(id);
            if (resourceRequest == null)
            {
                return NotFound();
            }
            return View(resourceRequest);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CTS,EIS,GMB,GMIT,RISK")] ResourceRequest resourceRequest)
        {
            if (id != resourceRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resourceRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResourceRequestExists(resourceRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(resourceRequest);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ResourceRequests == null)
            {
                return NotFound();
            }

            var resourceRequest = await _context.ResourceRequests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resourceRequest == null)
            {
                return NotFound();
            }

            return View(resourceRequest);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ResourceRequests == null)
            {
                return Problem("Entity set 'AppDbContext.ResourceRequests'  is null.");
            }
            var resourceRequest = await _context.ResourceRequests.FindAsync(id);
            if (resourceRequest != null)
            {
                _context.ResourceRequests.Remove(resourceRequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResourceRequestExists(int id)
        {
          return _context.ResourceRequests.Any(e => e.Id == id);
        }
    }
}
