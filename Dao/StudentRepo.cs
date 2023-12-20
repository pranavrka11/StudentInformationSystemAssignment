using StudentManagementSystem.Entities;
using StudentManagementSystem.Exceptions;
using StudentManagementSystem.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StudentManagementSystem.Dao
{
    internal class StudentRepo : IStudentRepo
    {
        SqlCommand cmd = null;

        public StudentRepo() 
        {
            cmd = new SqlCommand();
        }

        //Implementation for Student class methods
        public int EnrollInCourse(int courseID, int studentID, DateTime enrollmentDate)
        {
            using(SqlConnection conn=DbConnUtil.returnConnection())
            {
                List<int> courseIDList = new List<int>();
                cmd.CommandText = "select course_id from Enrollments where student_id=@stid";
                cmd.Parameters.AddWithValue("@stid", studentID);
                cmd.Connection = conn;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    courseIDList.Add((int)reader["course_id"]);
                }
                conn.Close();

                bool isEnrolled = false;
                foreach(int c in courseIDList)
                {
                    if(c==courseID)
                        isEnrolled = true;
                }

                if (isEnrolled)
                {
                    cmd.CommandText = "insert into Enrollments(student_id, course_id, enrollment_date) values(@sid, @cid, @edate)";
                    cmd.Parameters.AddWithValue("@sid", studentID);
                    cmd.Parameters.AddWithValue("@cid", courseID);
                    cmd.Parameters.AddWithValue("@edate", enrollmentDate);

                    cmd.Connection = conn;
                    conn.Open();

                    int enrollStatus = cmd.ExecuteNonQuery();

                    return enrollStatus;
                }
                else
                    throw new DuplicateEnrollmentException();
            }
        }

        public int UpdateStudentInfo(int studentID, string fname, string lname, DateTime dob, string email, string phone)
        {
            using(SqlConnection conn = DbConnUtil.returnConnection())
            {
                cmd.CommandText = "update Students set first_name=@fname, last_name=@lname, date_of_birth=@dob, email=@email, phone=@phone where Student_id=@sid";
                cmd.Parameters.AddWithValue("@sid", studentID);
                cmd.Parameters.AddWithValue("@fname", fname);
                cmd.Parameters.AddWithValue("@lname", lname);
                cmd.Parameters.AddWithValue("@dob", dob);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phone", phone);

                cmd.Connection = conn;
                conn.Open();

                int updateStudentStatus= cmd.ExecuteNonQuery();
                return updateStudentStatus;
            }
        }

        public List<Student> ViewAllStudentsDetails()
        {
            List<Student> allStudents = new List<Student>();
            using(SqlConnection conn = DbConnUtil.returnConnection())
            {
                cmd.CommandText = "select * from Students";
                cmd.Connection = conn;
                conn.Open();

                SqlDataReader r=cmd.ExecuteReader();
                while (r.Read())
                {
                    Student s= new Student();
                    s.studentID = (int)r["Student_id"];
                    s.firstName = (string)r["first_name"];
                    s.lastName = (string)r["last_name"];
                    s.DOB = (DateTime)r["date_of_birth"];
                    s.email = (string)r["email"];
                    s.phone = (string)r["phone"];
                    allStudents.Add(s);
                }
            }

            return allStudents;
        }

        public Student DisplayStudentInfo(int studentId)
        {
            Student s= new Student();
            using(SqlConnection conn = DbConnUtil.returnConnection())
            {
                cmd.CommandText = "select * from Students where Student_id=@sid";
                cmd.Parameters.AddWithValue("@sid", studentId);

                cmd.Connection = conn;
                conn.Open();

                SqlDataReader r=cmd.ExecuteReader();
                while (r.Read())
                {
                    s.studentID = (int)r["Student_id"];
                    s.firstName = (string)r["first_name"];
                    s.lastName = (string)r["last_name"];
                    s.DOB = (DateTime)r["date_of_birth"];
                    s.email = (string)r["email"];
                    s.phone = (string)r["phone"];
                }

                if (s.studentID == studentId)
                    return s;
                else
                    throw new StudentNotFoundException($"Student with ID {studentId} does not exist");
            }
        }

        public int CreateStudent(int studentID, string fname, string lname, DateTime dob, string email, string phone)
        {
            using(SqlConnection  conn = DbConnUtil.returnConnection())
            {
                cmd.CommandText = "insert into Students values(@sid, @fname, @lname, @dob, @email, @phone)";
                cmd.Parameters.AddWithValue("@sid", studentID);
                cmd.Parameters.AddWithValue("@fname", fname);
                cmd.Parameters.AddWithValue("@lname", lname);
                cmd.Parameters.AddWithValue("@dob", dob);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phone", phone);

                cmd.Connection=conn;
                conn.Open();

                int createStudentStatus = cmd.ExecuteNonQuery();

                return createStudentStatus;
            }
        }

        public List<string> GetEnrolledCourses(int studentID)
        {
            List<String> enrolledCourses = new List<String>();
            using(SqlConnection  conn = DbConnUtil.returnConnection())
            {
                cmd.CommandText = "select c.course_name from Students s join Enrollments e on s.Student_id=e.student_id join Courses c on e.course_id=c.course_id where s.Student_id=@sid";
                cmd.Parameters.AddWithValue("@sid", studentID);

                cmd.Connection = conn;
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    enrolledCourses.Add((string)reader["course_name"]);
                }
            }

            if (enrolledCourses != null)
                return enrolledCourses;
            else
                throw new StudentNotFoundException($"Student with ID {studentID} does not exist");
        }

        public List<Payment> GetPaymentHistory(int studentID)
        {
            List<Payment> paymentHistory = new List<Payment>();
            using(SqlConnection conn = DbConnUtil.returnConnection())
            {
                cmd.CommandText = "select * from Payments where student_id=@sid";
                cmd.Parameters.AddWithValue("@sid", studentID);

                cmd.Connection = conn;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    Payment p = new Payment();
                    p.paymentID = (int)reader["payment_id"];
                    p.studentID = (int)reader["student_id"];
                    p.paymentAmount = Convert.ToDouble(reader["amount"]);
                    p.paymentDate = (DateTime)reader["payment_date"];
                    paymentHistory.Add(p);
                }
            }

            return paymentHistory;
        }

        public int MakePayment(int studentID, int amount, DateTime date)
        {
            using(SqlConnection conn=DbConnUtil.returnConnection())
            {
                cmd.CommandText = "insert into Payments(student_id, amount, payment_date) values(@sid, @amt, @pdate)";
                cmd.Parameters.AddWithValue("@sid", studentID);
                cmd.Parameters.AddWithValue("@amt", amount);
                cmd.Parameters.AddWithValue("@pdate", date);

                cmd.Connection = conn;
                conn.Open();

                int makePaymentStatus = cmd.ExecuteNonQuery();

                return makePaymentStatus;
            }
        }


        //Implementation for Course class methods
        public int AssignTeacherToCourse(int teacherID, int courseID)
        {
            using(SqlConnection conn = DbConnUtil.returnConnection())
            {
                cmd.CommandText = "update Courses set teacher_id=@tid where course_id=@cid";
                cmd.Parameters.AddWithValue("@tid", teacherID);
                cmd.Parameters.AddWithValue("@cid", courseID);

                cmd.Connection = conn;
                conn.Open();

                int assignStatus= cmd.ExecuteNonQuery();

                return assignStatus;
            }
        }

        public int UpdateCourseInfo(int courseID, string name, int credits, int teacherID)
        {
            using(SqlConnection conn = DbConnUtil.returnConnection())
            {
                cmd.CommandText = "update Courses set course_name=@cname, credits=@cr, teacher_id=@tid where course_id=@cid";
                cmd.Parameters.AddWithValue("@tid", teacherID);
                cmd.Parameters.AddWithValue("@cid", courseID);
                cmd.Parameters.AddWithValue("@cname", name);
                cmd.Parameters.AddWithValue("@cr", credits);

                cmd.Connection = conn;
                conn.Open();

                int updateStatus = cmd.ExecuteNonQuery();

                if (updateStatus > 0)
                    return updateStatus;
                else
                    throw new CourseNotFoundException($"Course with ID {courseID} does not exist");
            }
        }

        public int CreateCourse(string name, int credits, int teacherID)
        {
            using (SqlConnection conn = DbConnUtil.returnConnection())
            {
                cmd.CommandText = "insert into Courses(course_name, credits, teacher_id) values(@name, @cr, @tid)";
                cmd.Parameters.AddWithValue("@tid", teacherID);
                cmd.Parameters.AddWithValue("@cname", name);
                cmd.Parameters.AddWithValue("@cr", credits);

                cmd.Connection = conn;
                conn.Open();

                int updateStatus = cmd.ExecuteNonQuery();

                return updateStatus;
            }
        }

        public Course DisplayCourseInfo(int courseID)
        {
            Course displayCourse = new Course();
            using(SqlConnection conn = DbConnUtil.returnConnection())
            {
                cmd.CommandText = "select * from Courses where Course_id=@cid";
                cmd.Parameters.AddWithValue("@cid", courseID);

                cmd.Connection = conn;
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    displayCourse.courseID = (int)reader["course_id"];
                    displayCourse.courseName = (string)reader["course_name"];
                    displayCourse.credits = (int)reader["credits"];
                    displayCourse.teacherID = (int)reader["teacher_id"];
                }
            }

            if (displayCourse != null)
                return displayCourse;
            else
                throw new CourseNotFoundException($"Course with ID {courseID} does not exist");
        }

        public Teacher GetTeacher(int courseID)
        {
            Teacher courseTeacher = new Teacher();
            using(SqlConnection conn=DbConnUtil.returnConnection())
            {
                cmd.CommandText = "select t.teacher_id, t.first_name, t.last_name, t.email from Courses c join Teachers t on c.teacher_id=t.teacher_id where c.course_id=@cid";
                cmd.Parameters.AddWithValue("@cid", courseID);

                cmd.Connection = conn;
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    courseTeacher.teacherID = (int)reader["teacher_id"];
                    courseTeacher.firstName = (string)reader["first_name"];
                    courseTeacher.lastName = (string)reader["last_name"];
                    courseTeacher.email = (string)reader["email"];
                }
            }

            if (courseTeacher != null)
                return courseTeacher;
            else
                throw new CourseNotFoundException($"Course with ID {courseID} does not exist");
        }

        public List<Course> GetAllCourses()
        {
            List<Course> allCourses= new List<Course>();
            using(SqlConnection conn = DbConnUtil.returnConnection())
            {
                cmd.CommandText = "select * from Courses";

                cmd.Connection = conn;
                conn.Open();

                SqlDataReader reader=cmd.ExecuteReader();
                while(reader.Read())
                {
                    Course c= new Course();
                    c.courseID = (int)reader["course_id"];
                    c.courseName = (string)reader["course_name"];
                    c.credits = (int)reader["credits"];
                    c.teacherID = (int)reader["teacher_id"];
                    allCourses.Add(c);
                }
            }

            return allCourses;
        }


        //Implementation for Tacher class methods
        public int UpdateTeacherInfo(int teacherID, string firstname, string lastname, string email)
        {
            using(SqlConnection con=DbConnUtil.returnConnection())
            {
                cmd.CommandText = "update Teachers set first_name=@fname, last_name=@lname, email=@email where teacher_id=@tid";
                cmd.Parameters.AddWithValue("@fname", firstname);
                cmd.Parameters.AddWithValue("@lname", lastname);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@tid", teacherID);

                cmd.Connection = con;
                con.Open();

                int updateTeacherStatus = cmd.ExecuteNonQuery();

                if (updateTeacherStatus > 0)
                    return updateTeacherStatus;
                else
                    throw new TeacherNotFoundException($"Teacher with ID {teacherID} does not exist");
            }
        }

        public int AddTeacher(string firstname, string lastname, string email)
        {
            using(SqlConnection conn=DbConnUtil.returnConnection())
            {
                cmd.CommandText = "insert into Teachers(first_name, last_name, email) values(@fname, @lname, @email)";
                cmd.Parameters.AddWithValue("@fname", firstname);
                cmd.Parameters.AddWithValue("@lname", lastname);
                cmd.Parameters.AddWithValue("@email", email);

                cmd.Connection = conn;
                conn.Open();

                int addTeacherStatus= cmd.ExecuteNonQuery();

                return addTeacherStatus;
            }
        }

        public Teacher DisplayTeacherInfo(int teacherID)
        {
            Teacher displayTeacher=new Teacher();
            using(SqlConnection conn = DbConnUtil.returnConnection())
            {
                cmd.CommandText = "select * from Teachers where teacher_id=@tid";
                cmd.Parameters.AddWithValue("@tid", teacherID);

                cmd.Connection = conn;
                conn.Open();

                SqlDataReader r = cmd.ExecuteReader();
                while(r.Read())
                {
                    displayTeacher.teacherID = (int)r["teacher_id"];
                    displayTeacher.firstName = (string)r["first_name"];
                    displayTeacher.lastName = (string)r["last_name"];
                    displayTeacher.email = (string)r["email"];
                }
            }

            if (displayTeacher != null)
                return displayTeacher;
            else
                throw new TeacherNotFoundException($"Teacher with ID {teacherID} does not exist");
        }

        public List<string> GetAssignedCourses(int teacherID)
        {
            List<string> assignedCourses=new List<string>();
            using(SqlConnection conn=DbConnUtil.returnConnection())
            {
                cmd.CommandText = "select c.course_name from Courses c join Teachers t on c.teacher_id=t.teacher_id where t.teacher_id=@tid";
                cmd.Parameters.AddWithValue("@tid", teacherID);

                cmd.Connection = conn;
                conn.Open();

                SqlDataReader r = cmd.ExecuteReader();
                while(r.Read())
                {
                    assignedCourses.Add((string)r["course_name"]);
                }
            }

            if (assignedCourses != null)
                return assignedCourses;
            else
                throw new TeacherNotFoundException($"Teacher with ID {teacherID} does not exist");
        }


        //Implementation for Payment class methods
        public Student GetStudentForPayment(int paymentID)
        {
            Student s= new Student();
            using(SqlConnection conn = DbConnUtil.returnConnection())
            {
                cmd.CommandText = "select s.Student_id, s.first_name, s.last_name, s.date_of_birth, s.email, s.phone from Payments p join Students s on p.student_id=s.Student_id where p.payment_id=@pid";
                cmd.Parameters.AddWithValue("@pid", paymentID);

                cmd.Connection = conn;
                conn.Open();

                SqlDataReader r= cmd.ExecuteReader();
                while(r.Read())
                {
                    s.studentID = (int)r["Student_id"];
                    s.firstName = (string)r["first_name"];
                    s.lastName = (string)r["last_name"];
                    s.DOB = (DateTime)r["date_of_birth"];
                    s.email = (string)r["email"];
                    s.phone = (string)r["phone"];
                }
            }

            return s;
        }

        public double GetPaymentAmount(int paymentID)
        {
            using(SqlConnection con=DbConnUtil.returnConnection())
            {
                cmd.CommandText = "select amount from Payments where payment_id=@pid";
                cmd.Parameters.AddWithValue("@pid", paymentID);

                cmd.Connection = con;
                con.Open();

                double paymentAmount = 0;

                SqlDataReader reader= cmd.ExecuteReader();
                while (reader.Read())
                {
                    paymentAmount = Convert.ToDouble(reader["amount"]);
                }

                return paymentAmount;
            }
        }

        public DateTime GetPaymentDate(int paymentID)
        {
            using(SqlConnection conn=DbConnUtil.returnConnection())
            {
                cmd.CommandText = "select payment_date from Payments where payment_id=@pid";
                cmd.Parameters.AddWithValue("@pid", paymentID);

                cmd.Connection = conn;
                conn.Open();

                DateTime paymentDate = new DateTime();

                SqlDataReader reader= cmd.ExecuteReader();
                while (reader.Read())
                {
                    paymentDate = (DateTime)reader["payment_date"];
                }

                return paymentDate;
            }
        }


        //Implementations for Enrollment class methods

        public Course GetCourse(int enrollmentID)
        {
            Course c= new Course();
            using(SqlConnection conn = DbConnUtil.returnConnection())
            {
                cmd.CommandText = "select c.course_id, c.course_name, c.credits, c.teacher_id from Enrollments e join Courses c on e.course_id=c.course_id where e.enrollment_id=@eid";
                cmd.Parameters.AddWithValue("@eid", enrollmentID);

                cmd.Connection = conn;
                conn.Open();

                SqlDataReader r= cmd.ExecuteReader();
                while (r.Read())
                {
                    c.courseID = (int)r["course_id"];
                    c.courseName = (string)r["course_name"];
                    c.credits = (int)r["credits"];
                    c.teacherID = (int)r["teacher_id"];
                }
            }

            return c;
        }

        public Student GetStudent(int enrollmentID)
        {
            Student s= new Student();
            using(SqlConnection conn=DbConnUtil.returnConnection())
            {
                cmd.CommandText = "select s.Student_id, s.first_name, s.last_name, s.date_of_birth, s.email, s.phone from Enrollments e join Students s on e.student_id=s.Student_id where e.enrollment_id=@eid";
                cmd.Parameters.AddWithValue("@eid", enrollmentID);

                cmd.Connection = conn;
                conn.Open();

                SqlDataReader r= cmd.ExecuteReader();
                while (r.Read())
                {
                    s.studentID = (int)r["Student_id"];
                    s.firstName = (string)r["first_name"];
                    s.lastName = (string)r["last_name"];
                    s.DOB = (DateTime)r["date_of_birth"];
                    s.email = (string)r["email"];
                    s.phone = (string)r["phone"];
                }
            }

            return s;
        }
    }
}
