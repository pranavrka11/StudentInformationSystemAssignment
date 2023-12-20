using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Entities
{
    internal class Enrollment
    {
        int enrollmentID;
        int studentID;
        int courseID;
        string enrollmentDate;

        public Enrollment(int id, int sid, int cid, string date)
        {
            enrollmentID = id;
            studentID = sid;
            courseID = cid;
            enrollmentDate = date;
        }

        public int EnrollmentID
        {
            get { return enrollmentID; }
            set { enrollmentID = value; }
        }

        public int StudentID
        {
            get { return studentID; }
            set { studentID = value; }
        }

        public string EnrollmentDate
        {
            get { return enrollmentDate; }
            set { enrollmentDate = value; }
        }
    }
}
