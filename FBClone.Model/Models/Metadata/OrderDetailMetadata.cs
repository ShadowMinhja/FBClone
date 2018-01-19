using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FBClone.Model
{
    [MetadataType(typeof(OrderDetailMetadata))]
    public partial class OrderDetail
    {
        private sealed class OrderDetailMetadata
        {
            [JsonIgnore]
            public Order Order { get; set; } // FK_dbo.OrderDetails_Order_OrderId
        }
    }
}