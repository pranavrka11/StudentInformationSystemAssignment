using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Entities
{
    internal class Payment
    {
        public int paymentID;
        public int studentID;
        public double paymentAmount;
        public DateTime paymentDate;


        public Payment()
        {
        }

        public Payment(int id, int sid, double amount)
        {
            paymentID = id;
            studentID = sid;
            paymentAmount = amount;
        }

        public int PaymentID
        {
            get { return paymentID; }
            set { paymentID = value; }
        }

        public int StudentID
        {
            get { return studentID; }
            set { studentID = value; }
        }

        public double PaymentAmount
        {
            get { return paymentAmount; }
            set { paymentAmount = value; }
        }

        public DateTime PaymentDate
        {
            get { return paymentDate; }
            set { paymentDate = value; }
        }

        public override string ToString()
        {
            return $"PaymentID: {paymentID}\t StudentID: {studentID}\t Amount: {PaymentAmount}\t Date of Payment: {paymentDate}";
        }
    }
}
