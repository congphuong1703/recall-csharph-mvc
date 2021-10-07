using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using WTAssignment.Data;
using WTAssignment.Models;
namespace WTAssignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AmazonOrdersContext context = new AmazonOrdersContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GraphPage()
        {
            List<int> allYear = this.getAllYear();
            RecallSearch recallSearch = new()
            {
                saleYears = allYear
            };
            return View(recallSearch);
        }

        private List<string> getAllItemName()
        {
            List<string> allItemName = context.ItemsInOrders
                .Select(x => x.Item.ItemName).Distinct()
                .ToList();

            return allItemName;
        }

        private List<int> getAllYear()
        {
            List<int> years = context.CustomerOrders
                   .Select(s => s.OrderDate.Year).Distinct()
                   .ToList();
            return years;
        }

        public IActionResult Details(int? itemId)
        {
            List<RecallDetail> recallDetails = new();
            List<RecallSearch> recallSearches = new();
            if (itemId.HasValue)
            {
                var query = from i in context.Items
                            join iio in context.ItemsInOrders on i.ItemId equals iio.ItemId
                            join ic in context.ItemCategories on i.CategoryId equals ic.CategoryId
                            join co in context.CustomerOrders on iio.OrderNumber equals co.OrderNumber
                            join c in context.Customers on co.CustomerId equals c.CustomerId
                            join a in context.Addresses on c.AddressId equals a.AddressId
                            where i.ItemId == itemId
                            select new
                            { i, iio, ic, co, c, a };

                var recall = (from q in query
                              group new { q.iio.TotalItemCost, q.iio.NumberOf }
                              by new
                              {
                                  q.c.FirstName,
                                  q.c.LastName,
                                  q.c.MainPhoneNumber,
                                  q.c.SecondaryPhoneNumber,
                                  q.c.Email,
                                  q.a.AddressLine,
                                  q.i.ItemName,
                                  q.i.ItemCost,
                                  q.i.ItemDescription,
                                  q.i.ItemImage
                              }
                               into itemGr
                              select new RecallDetail
                              {
                                  totalCost = (decimal)itemGr.Sum(s => s.TotalItemCost),
                                  item = new()
                                  {
                                      ItemName = itemGr.Key.ItemName,
                                      ItemDescription = itemGr.Key.ItemDescription,
                                      ItemCost = itemGr.Key.ItemCost,
                                      ItemImage = itemGr.Key.ItemImage,
                                  },
                                  customer = new()
                                  {
                                      FirstName = itemGr.Key.FirstName,
                                      LastName = itemGr.Key.LastName,
                                      MainPhoneNumber = itemGr.Key.MainPhoneNumber,
                                      SecondaryPhoneNumber = itemGr.Key.SecondaryPhoneNumber,
                                      Email = itemGr.Key.Email,
                                  },
                                  address = new()
                                  {
                                      AddressLine = itemGr.Key.AddressLine
                                  },
                                  untisSold = itemGr.Sum(x => x.NumberOf),
                              }).ToListAsync();
                recallDetails = recall.Result;
            }
            var recallSearch = new RecallSearch
            {
                recallDetails = recallDetails,
                item = recallDetails.First().item
            };
            return View(recallSearch);
        }
        public IActionResult RecallPage(string itemName, int? saleYear)
        {
            List<RecallDetail> recallDetails = new();
            List<RecallSearch> recallSearches = new();
            List<string> itemNames = this.getAllItemName();
            List<int> years = this.getAllYear();

            if (!String.IsNullOrEmpty(itemName))
            {
                var recall = Enumerable.Empty<RecallDetail>().AsQueryable();

                if (saleYear.HasValue)
                {
                    var query = from i in context.Items
                                join iio in context.ItemsInOrders on i.ItemId equals iio.ItemId
                                join ic in context.ItemCategories on i.CategoryId equals ic.CategoryId
                                join co in context.CustomerOrders on iio.OrderNumber equals co.OrderNumber
                                where i.ItemName == itemName && co.OrderDate.Year.Equals(saleYear.Value)
                                select new
                                {
                                    i.ItemId,
                                    i.ItemName,
                                    i.ItemDescription,
                                    iio.NumberOf,
                                    i.ItemCost,
                                    ic.CategoryName,
                                    co.CustomerId
                                };

                    recall = from q in query
                             group new { q.NumberOf, q.CustomerId }
                             by new { q.ItemId, q.ItemName, q.ItemDescription, q.ItemCost, q.CategoryName }
                              into itemGr
                             select new RecallDetail
                             {
                                 totalCustomers = itemGr.Count(),
                                 item = new()
                                 {
                                     ItemName = itemGr.Key.ItemName,
                                     ItemId = itemGr.Key.ItemId,
                                     ItemDescription = itemGr.Key.ItemDescription,
                                     ItemCost = itemGr.Key.ItemCost,
                                 },
                                 category = new()
                                 {
                                     CategoryName = itemGr.Key.CategoryName == null ? "" : itemGr.Key.CategoryName
                                 },
                                 untisSold = itemGr.Sum(x => x.NumberOf),
                             };
                }
                else
                {
                    var query = from i in context.Items
                                join iio in context.ItemsInOrders on i.ItemId equals iio.ItemId
                                join ic in context.ItemCategories on i.CategoryId equals ic.CategoryId
                                join co in context.CustomerOrders on iio.OrderNumber equals co.OrderNumber
                                where i.ItemName == itemName
                                select new
                                {
                                    i.ItemId,
                                    i.ItemName,
                                    i.ItemDescription,
                                    iio.NumberOf,
                                    i.ItemCost,
                                    ic.CategoryName,
                                    co.CustomerId,
                                    co.OrderDate.Year
                                };

                    recall = from q in query
                             group new { q.NumberOf, q.CustomerId }
                             by new { q.ItemId, q.ItemName, q.ItemDescription, q.ItemCost, q.CategoryName ,q.Year}
                              into itemGr
                             select new RecallDetail
                             {
                                 totalCustomers = itemGr.Count(),
                                 item = new()
                                 {
                                     ItemName = itemGr.Key.ItemName,
                                     ItemId = itemGr.Key.ItemId,
                                     ItemDescription = itemGr.Key.ItemDescription,
                                     ItemCost = itemGr.Key.ItemCost,
                                 },
                                 category = new()
                                 {
                                     CategoryName = itemGr.Key.CategoryName == null ? "" : itemGr.Key.CategoryName
                                 },
                                 untisSold = itemGr.Sum(x => x.NumberOf),
                             };
                }

                recallDetails = recall.ToList();
            }


            var recallSearch = new RecallSearch
            {
                saleYears = years,
                recallDetails = recallDetails,
                itemNames = itemNames
            };

            return View(recallSearch);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
