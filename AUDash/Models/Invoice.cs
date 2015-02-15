using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AUDash.Models
{
    public class Invoice
    {
        public string Project { get; set; }
        public string Partner { get; set; }
        public string Resource { get; set; }
        public string Period { get; set; }
        public string Date { get; set; }
        public string Hours { get; set; }
        public decimal Amount { get; set; }
        public string ATBApproval { get; set; }
        public string ATBApprovalSentOn { get; set; }
        public string InvoiceRaised { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceRaisedOn { get; set; }
        public string Comments { get; set; }
        public string PaymentReceived { get; set; }
    }

    public class RevenueByYear
    {
        public int year { get; set; }
        List<RevenueByMonth> revenue { get; set; }
    }
    public class RevenueByMonth
    {
        public int month { get; set; }
        public double amount { get; set; }
    }
}