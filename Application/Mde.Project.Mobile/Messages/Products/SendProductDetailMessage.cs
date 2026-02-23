using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mde.Project.Mobile.Models.Products;

namespace Mde.Project.Mobile.Messages.Products
{
    public class SendProductDetailMessage
    {
        public ProductDetailModel ProductDetail { get; set; }

        public SendProductDetailMessage(ProductDetailModel productDetail)
        {
            ProductDetail = productDetail;
        }
    }
}
