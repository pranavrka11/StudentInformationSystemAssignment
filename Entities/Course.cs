using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Entities
{
    internal class Course
    {
        public int courseID;
        public string courseName;
        public int credits;
        public int teacherID;

        public Course()
        {
        }

        public Course(int id, string cname, int credit,  int iid)
        {
            courseID = id;
            courseName = cname;
            credits=credit;
            teacherID= iid;
        }

        public int CourseID
        {
            get { return courseID; }
            set { courseID = value; }
        }

        public string CourseName
        {
            get { return courseName; }
            set { courseName = value; }
        }

        public int Credits
        {
            get { return credits; }
            set { credits = value; }
        }

        public int TeacherID
        {
            get { return teacherID; }
            set { teacherID = value; }
        }

        public override string ToString()
        {
            return $"CourseID: {courseID}\t Course Name: {courseName}\t Credits: {credits}\t TeacherID: {teacherID}";
        }
    }
}
