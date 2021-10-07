using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTAssignment.Data;

namespace WTAssignment.Controllers
{
    public class ReportController : Controller
    {
        private readonly AmazonOrdersContext _context;

        public ReportController(AmazonOrdersContext context)
        {
            _context = context;
        }

        [Produces("application/json")]
        // Get /Report/AnnualSalesData?year={xx}
        public IActionResult AnnualSalesData(int? year)
        {
            if (year.HasValue)
            {
                #region FastQuery
                var query = _context.Items
                    .Select(i => new
                    {
                        i.ItemId,
                        i.ItemName,
                        itemCount = _context.ItemsInOrders
                                    .Include(iio => iio.OrderNumberNavigation)
                                    .Where(iio => iio.OrderNumberNavigation.OrderDate.Year == year && iio.ItemId == i.ItemId)
                                    .Sum(io => io.NumberOf)
                    })
                    .OrderBy(i => i.ItemName)
                    .ToList();
                #endregion

                #region SlowQuery
                /*
                var itemsMethod2 = _context.ItemsInOrders
                    .Include(i => i.Item)
                    .Include(i => i.OrderNumberNavigation)
                    .Where(i => i.OrderNumberNavigation.OrderDate.Year == year)
                    .GroupBy(i => new { i.ItemId, i.Item.ItemName })
                    .Select(i => new
                    {
                        i.Key.ItemId,
                        i.Key.ItemName,
                        itemCount = i.Select(IIO => IIO.NumberOf).Sum()
                    })
                     .OrderBy(i => i.ItemName);
                */
                #endregion

                return Json(query.ToList());
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
