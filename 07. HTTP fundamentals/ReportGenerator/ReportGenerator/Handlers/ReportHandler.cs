using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Task.DB;

namespace ReportGenerator.Handlers
{
	public class ReportHandler : IHttpHandler
	{
		private const string CUSTOMER = "customer";
		private const string CUSTOMERID = "customerID";
		private const string ORDERS = "orders";
		private const string ORDER = "order";
		private const string DATETO = "dateTo";
		private const string DATEFROM = "dateFrom";
		private const string SHIPNAME = "shipName";
		private const string SHIPCOUNTRY = "shipCountry";
		private const string TAKE = "take";
		private const string SKIP = "skip";

		public bool IsReusable { get => false; }

		public void ProcessRequest(HttpContext context)
		{
			string customerID = Convert.ToString(context.Request.Params.Get(CUSTOMER));
			DateTime dateTo = Convert.ToDateTime(context.Request.Params.Get(DATETO));
			DateTime dateFrom = Convert.ToDateTime(context.Request.Params.Get(DATEFROM));
			int take = Convert.ToInt32(context.Request.Params.Get(TAKE));
			int skip = Convert.ToInt32(context.Request.Params.Get(SKIP));

			using (var connection = new Northwind())
			{
				List<Order> orders = connection.Orders.ToList();

				if (!string.IsNullOrWhiteSpace(customerID))
					orders = orders.Where(x => x.CustomerID == customerID).ToList();

				if (dateFrom != DateTime.MinValue)
					orders = orders.Where(x => x.RequiredDate >= dateFrom).ToList();

				if (dateTo != DateTime.MinValue)
					orders = orders.Where(x => x.RequiredDate <= dateTo).ToList();

				if (skip != 0)
					orders = orders.Skip(skip).ToList();

				if (take != 0)
					orders = orders.Take(take).ToList();

				if (context.Request.Headers.Get(1).Contains("xml"))
					GenerateXmlFile(orders.ToList(), context);

				else
					GenerateExcelFile(orders.ToList(), context);

			}
		}
		private MemoryStream GetMemoryStream(XLWorkbook excelWorkbook)
		{
			MemoryStream stream = new MemoryStream();
			excelWorkbook.SaveAs(stream);
			stream.Position = 0;
			return stream;
		}

		private MemoryStream GetMemoryStream(XDocument xdoc)
		{
			MemoryStream stream = new MemoryStream();
			xdoc.Save(stream);
			stream.Position = 0;
			return stream;
		}

		private void GenerateXmlFile(List<Order> orders, HttpContext httpContext)
		{
			XDocument file = new XDocument();
			XElement xmlOrders = new XElement(ORDERS);

			foreach (var order in orders)
			{
				XElement orderElement = new XElement(ORDER);

				XAttribute customerId = new XAttribute(CUSTOMERID, order.CustomerID);
				XElement shipName = new XElement(SHIPNAME, order.ShipName);
				XElement shipCountry = new XElement(SHIPCOUNTRY, order.ShipCountry);

				orderElement.Add(customerId);				
				orderElement.Add(shipCountry);
				orderElement.Add(shipName);

				xmlOrders.Add(orderElement);
			}

			file.Add(xmlOrders);

			string name = httpContext.Server.UrlEncode("Report" + "_" + DateTime.Now.ToShortDateString() + ".xml");
			MemoryStream stream = GetMemoryStream(file);
			httpContext.Response.Clear();
			httpContext.Response.Buffer = true;
			httpContext.Response.AddHeader("content", "filename" + name);
			httpContext.Response.ContentType = "text/xml";
			httpContext.Response.BinaryWrite(stream.ToArray());
			httpContext.Response.End();
		}

		private void GenerateExcelFile(List<Order> orders, HttpContext httpContext)
		{
			int i = 2;

			using (XLWorkbook excel = new XLWorkbook())
			{
				var worksheet = excel.Worksheets.Add("Sheet1");

				worksheet.Cell("A" + 1).Value = CUSTOMERID;
				worksheet.Cell("B" + 1).Value = SHIPCOUNTRY;
				worksheet.Cell("C" + 1).Value = SHIPNAME;
				

				foreach (Order order in orders)
				{
					worksheet.Cell("A" + i).Value = order.CustomerID;
					worksheet.Cell("B" + i).Value = order.ShipCountry;
					worksheet.Cell("C" + i).Value = order.ShipName;					
					i++;
				}

				string name = httpContext.Server.UrlEncode("Report" + "_" + DateTime.Now.ToShortDateString() + ".xlsx");
				MemoryStream stream = GetMemoryStream(excel);
				httpContext.Response.Clear();
				httpContext.Response.Buffer = true;
				httpContext.Response.AddHeader("content", "filename" + name);
				httpContext.Response.ContentType = "application/vnd.ms-excel";
				httpContext.Response.BinaryWrite(stream.ToArray());
				httpContext.Response.End();
			}
		}
	}
}