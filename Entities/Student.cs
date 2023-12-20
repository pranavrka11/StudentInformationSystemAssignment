using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Entities
{
    internal class Student
    {
        public int studentID;
        public string firstName;
        public string lastName;
        public DateTime DOB;
        public string email;
        public string phone;

        public Student()
        {
        }

        public Student(int ID, string fname, string lname, DateTime dob, string emailid, string phonenumber)
        {
            studentID = ID;
            firstName = fname;
            lastName = lname;
            DOB = dob;
            email = emailid;
            phone = phonenumber;
        }

        

        public int StudentID
        {
            get { return studentID; }
            set { studentID = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public DateTime dob
        {
            get { return DOB; }
            set { DOB = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public override string ToString()
        {
            return $"Student ID: {studentID}\t Student Name: {firstName} {lastName}\t Date of Birth: {DOB}\t Email: {email}\t Phone: {phone}";
        }
    }
}
