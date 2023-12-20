using StudentManagementSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Dao
{
    internal interface IStudentRepo
    {
        //Methods for Student Class
        int EnrollInCourse(int courseID, int studentID, DateTime enrollmentDate);//

        int UpdateStudentInfo(int studentID, string fname, string lname, DateTime dob, string email, string phone);//

        int MakePayment(int studentID, int amount, DateTime date);//

        Student DisplayStudentInfo(int studentId);//

        int CreateStudent(int studentID, string fname, string lname, DateTime dob, string email, string phone);//

        List<string> GetEnrolledCourses(int studentID);//

        List<Payment> GetPaymentHistory(int studentID);//

        List<Student> ViewAllStudentsDetails();//



        //Methods for Course class
        int AssignTeacherToCourse(int teacherID, int courseID);//

        int UpdateCourseInfo(int courseID, string name, int credits, int teacherID);//

        int CreateCourse(string name, int credits, int teacherID);//

        Course DisplayCourseInfo(int courseID);//

        Teacher GetTeacher(int courseID);//

        List<Course> GetAllCourses();



        //Methods for Teacher class
        int UpdateTeacherInfo(int teacherID, string firstname, string lastname, string email);//

        int AddTeacher(string firstname, string lastname, string email);//

        Teacher DisplayTeacherInfo(int teacherID);

        List<string> GetAssignedCourses(int teacherID);



        //Methods for Payment class
        Student GetStudentForPayment(int paymentID);//

        double GetPaymentAmount(int paymentID);//

        DateTime GetPaymentDate(int paymentID);//



        //Methods for Enrollment class
        Student GetStudent(int enrollmentID);//

        Course GetCourse(int enrollmentID);//
    }
}
