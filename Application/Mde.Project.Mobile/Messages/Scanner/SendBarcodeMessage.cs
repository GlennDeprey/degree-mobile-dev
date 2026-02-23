using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Messages.Scanner
{
    public class SendBarcodeMessage
    {
        public string Barcode { get; set; }
        public SendBarcodeMessage(string barcode)
        {
            Barcode = barcode;
        }
    }
}
