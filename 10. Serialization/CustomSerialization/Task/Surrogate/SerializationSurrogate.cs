using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Task.DB;

namespace Task.Surrogate
{
	public class SerializationSurrogate : ISerializationSurrogate
	{
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			var orderDetail = (Order_Detail)obj;
			var dbContext = (context.Context as IObjectContextAdapter).ObjectContext;
			dbContext.LoadProperty(orderDetail, x => x.Order);
			dbContext.LoadProperty(orderDetail, x => x.Product);

			info.AddValue(nameof(orderDetail.OrderID), orderDetail.OrderID);
			info.AddValue(nameof(orderDetail.ProductID), orderDetail.ProductID);
			info.AddValue(nameof(orderDetail.UnitPrice), orderDetail.UnitPrice);
			info.AddValue(nameof(orderDetail.Quantity), orderDetail.Quantity);
			info.AddValue(nameof(orderDetail.Discount), orderDetail.Discount);
			info.AddValue(nameof(orderDetail.Order), orderDetail.Order);
			info.AddValue(nameof(orderDetail.Product), orderDetail.Product);
		}

		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			var orderDetail = (Order_Detail)obj;
			orderDetail.OrderID = info.GetInt32(nameof(orderDetail.OrderID));
			orderDetail.ProductID = info.GetInt32(nameof(orderDetail.ProductID));
			orderDetail.UnitPrice = info.GetDecimal(nameof(orderDetail.UnitPrice));
			orderDetail.Quantity = info.GetInt16(nameof(orderDetail.Quantity));
			orderDetail.Discount = (float)info.GetValue(nameof(orderDetail.Discount), typeof(float));
			orderDetail.Order = (Order)info.GetValue(nameof(orderDetail.Order), typeof(Order));
			orderDetail.Product = (Product)info.GetValue(nameof(orderDetail.Product), typeof(Product));
			return orderDetail;
		}
	}
}
