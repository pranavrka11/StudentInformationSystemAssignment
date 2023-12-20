using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Entities
{
    internal class Teacher
    {
        public int teacherID;
        public string firstName;
        public string lastName;
        public string email;

        public Teacher()
        {
        }

        public Teacher(int id, string fname, string lname, string emailid)
        {
            teacherID = id;
            firstName = fname;
            lastName = lname;
            email = emailid;
        }

        public int TeacherID
        {
            get { return teacherID; }
            set { teacherID = value; }
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

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public override string ToString()
        {
            return $"TeacherID: {teacherID}\t Teacher Name: {firstName} {lastName}\t Email: {email}";
        }
    }
}
