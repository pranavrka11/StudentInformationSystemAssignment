

using StudentManagementSystem.Dao;
using StudentManagementSystem.Entities;
using StudentManagementSystem.Exceptions;

int loopChoice = 1;
while(loopChoice == 1) 
{
    Console.WriteLine("\n--------Welcome to Student Management System--------");
    Console.WriteLine("Here are your choices:");
    Console.WriteLine("1. Student Management");
    Console.WriteLine("2. Course Management");
    Console.WriteLine("3. Enrollment Management");
    Console.WriteLine("4. Teacher Management");
    Console.WriteLine("5. Payment Management");
    Console.WriteLine("6. Exit");
    Console.Write("\nPlease enter your choice: ");
    int mainChoice=int.Parse(Console.ReadLine());
    switch(mainChoice)
    {
        //Student Management Portal
        case 1:
            Console.WriteLine("\nWelcome to Student Management portal");
            Console.WriteLine("1. Add a new student");
            Console.WriteLine("2. Enroll a student in a course");
            Console.WriteLine("3. Get information of a specific student");
            Console.WriteLine("4. Update the information of an existing student");
            Console.WriteLine("5. View all the courses a particular student has enrolled in");
            Console.WriteLine("6. Get Payment history of a student");
            Console.WriteLine("7. View all students' info");
            Console.WriteLine("8. Go back to main menu");
            Console.Write("\nEnter your choice: ");
            int studentChoice=int.Parse(Console.ReadLine());
            switch(studentChoice)
            {
                case 1:
                    Console.WriteLine("\nTo add a new student to the system, please enter the following details:");
                    Console.Write("ID: ");
                    int sid=int.Parse(Console.ReadLine());
                    Console.Write("First name: ");
                    string fname=Console.ReadLine();
                    Console.Write("Last name: ");
                    string lname=Console.ReadLine();
                    Console.Write("Date of Birth: ");
                    DateTime dob=DateTime.Parse(Console.ReadLine());
                    Console.Write("Email: ");
                    string email=Console.ReadLine();
                    Console.Write("Phone: ");
                    string phone=Console.ReadLine();

                    Student s1 = new Student() {studentID=sid, firstName=fname, lastName=lname, DOB=dob, email=email, phone=phone };
                    IStudentRepo addStudent = new StudentRepo();
                    int addStudentStatus = addStudent.CreateStudent(sid, fname, lname, dob, email, phone);
                    if(addStudentStatus>1)
                        Console.WriteLine("\nSuccessfully added student record!");
                    else
                        Console.WriteLine("\nSomething went wrong");
                    break;

                case 2:
                    Console.WriteLine("\nHere are all the courses you can enroll a student:\n");
                    //Code to retrieve all courses
                    IStudentRepo enrollStudent = new StudentRepo();
                    List<Course> allCourses = enrollStudent.GetAllCourses();
                    foreach(var v in allCourses)
                    {
                        Console.WriteLine(v);
                    }
                    int credits = 0;
                    Console.Write("\nPlease enter the ID of the course you would like to enroll in: ");
                    int cid=int.Parse(Console.ReadLine());
                    Console.Write("Enter the credits associated with the course(8 or 10): ");
                    //while(credits != 8 || credits != 10)
                    credits = int.Parse(Console.ReadLine());
                    Console.Write("Enter your student ID: ");
                    int studentid=int.Parse(Console.ReadLine());
                    DateTime enrolmentdate = DateTime.Now;
                    int paymentAmount = 0;
                    if(credits==10)
                    {
                        paymentAmount = 5500;
                        Console.WriteLine("\nThe course you wish to enroll is a major course with 10 credits");
                        Console.WriteLine("Your total enrollment amount standard for a major course is: Rs. 5500");
                        Console.Write("\nDo you wish to proceed with enrollment by making the payment?(press 1 for yes, 0 for no): ");
                    }
                    else
                    {
                        paymentAmount = 5000;
                        Console.WriteLine("\nThe course you wish to enroll is a minor course with 8 credits");
                        Console.WriteLine("Your total enrollment amount standard for a minor course is: Rs. 5000");
                        Console.Write("\nDo you wish to proceed with enrollment by making the payment?(press 1 for yes, 0 for no): ");
                    }
                    int enChoice=int.Parse(Console.ReadLine());
                    if(enChoice==1) 
                    {
                        try
                        {
                            int enrollmentStat = enrollStudent.EnrollInCourse(cid, studentid, enrolmentdate);
                            int paymentStat = enrollStudent.MakePayment(studentid, paymentAmount, enrolmentdate);
                            if (enrollmentStat > 1 && paymentStat > 1)
                            {
                                Console.WriteLine("\nEnrollment completed successfully!");
                                Console.WriteLine("Generating the payment...");
                                Thread.Sleep(3000);
                                Console.WriteLine("\nPayment successful");
                                Console.WriteLine("\n We wish you all the best!");
                            }
                            else
                                Console.WriteLine("Something went wrong. Please try again");
                        }
                        catch(DuplicateEnrollmentException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    break;

                case 3:
                    Console.Write("\nEnter the ID of the student you wish to fetch details: ");
                    int sdid=int.Parse(Console.ReadLine());
                    IStudentRepo studentDetails=new StudentRepo();
                    try
                    {
                        Student s = studentDetails.DisplayStudentInfo(sdid);
                        Console.WriteLine(s);
                    }
                    catch(StudentNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;

                case 4:
                    Console.Write("\nEnter the ID of the student to update information: ");
                    int updateid=int.Parse(Console.ReadLine());
                    Console.WriteLine($"Enter the new details of the student with ID {updateid}");
                    Console.Write("\nFirst Name: ");
                    Console.Write("First name: ");
                    string ufname = Console.ReadLine();
                    Console.Write("Last name: ");
                    string ulname = Console.ReadLine();
                    Console.Write("Date of Birth: ");
                    DateTime udob = DateTime.Parse(Console.ReadLine());
                    Console.Write("Email: ");
                    string uemail = Console.ReadLine();
                    Console.Write("Phone: ");
                    string uphone = Console.ReadLine();
                    IStudentRepo updateStudent = new StudentRepo();
                    int updateStatus = updateStudent.UpdateStudentInfo(updateid, ufname, ulname, udob, uemail, uphone);
                    if (updateStatus > 1)
                        Console.WriteLine("\nUpdate student information successfully!");
                    else
                        Console.WriteLine("\nSomething went wrong");
                    break;

                case 5:
                    Console.Write("\nEnter the ID of the student to view all the courses he/she is enrolled in: ");
                    int studentEnID=int.Parse(Console.ReadLine());
                    IStudentRepo getEnrollments = new StudentRepo();
                    try
                    {
                        List<String> EnrolledCourses = getEnrollments.GetEnrolledCourses(studentEnID);
                        foreach (var course in EnrolledCourses)
                        {
                            Console.Write($" {course} ");
                        }
                        Console.WriteLine();
                    }
                    catch(StudentNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;

                case 6:
                    Console.Write("\nEnter ID of the student to view payment history: ");
                    int studentPayID=int.Parse(Console.ReadLine());
                    IStudentRepo getPayments = new StudentRepo();
                    List<Payment> paymentHistory=getPayments.GetPaymentHistory(studentPayID);
                    foreach(var p in paymentHistory)
                    {
                        Console.WriteLine(p);
                    }
                    break;

                case 7:
                    Console.WriteLine("\nHere are the details of all the students:\n");
                    IStudentRepo viewAllStudents = new StudentRepo();
                    List<Student> allStudents = viewAllStudents.ViewAllStudentsDetails();
                    foreach (var v in allStudents)
                    {
                        Console.WriteLine(v);
                    }
                    break;

                default:
                    Console.WriteLine("\nPlease enter a valid choice");
                    break;
            }
            break;

        //Course Management Portal
        case 2:
            Console.WriteLine("\nWelcome to Course Management portal");
            Console.WriteLine("1. Assign new teacher to a course");
            Console.WriteLine("2. Update the information of a course");
            Console.WriteLine("3. Add a new course");
            Console.WriteLine("4. Display information of a particular course");
            Console.WriteLine("5. Get Faculty details for a particular course");
            Console.Write("\nEnter your choice: ");
            int courseChoice = int.Parse(Console.ReadLine());
            switch(courseChoice)
            {
                case 1:
                    Console.Write("\nEnter the ID of the course you wish to update the faculty: ");
                    int updateFacultyCourseid=int.Parse(Console.ReadLine());
                    Console.Write("Enter the ID of the new faculty you wish to assign to this course: ");
                    int newFacultyID=int.Parse(Console.ReadLine());

                    IStudentRepo changeFaculty=new StudentRepo();
                    int updateFacultyStat = changeFaculty.AssignTeacherToCourse(newFacultyID, updateFacultyCourseid);

                    if (updateFacultyStat > 0)
                        Console.WriteLine($"\nUpdated the faculty for course with ID {updateFacultyCourseid} successfully!");
                    else
                        Console.WriteLine("\nSomething went wrong");
                    break;
                case 2:
                    Console.Write("\nEnter the ID of the course you wish to update the faculty: ");
                    int updateCourseid = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter the new information for the course:");
                    Console.Write("Name of the course: ");
                    string nname = Console.ReadLine();
                    Console.Write("Total credits for the course: ");
                    int ncredits=int.Parse(Console.ReadLine());
                    Console.Write("ID of the Faculty in Charge: ");
                    int nteacherid=int.Parse(Console.ReadLine());
                    IStudentRepo updateCourse=new StudentRepo();
                    int updateCourseStat=updateCourse.UpdateCourseInfo(updateCourseid, nname, ncredits, nteacherid);
                    if (updateCourseStat > 0)
                        Console.WriteLine($"\nSuccessfully updated the information of the course with ID {updateCourseid}");
                    else
                        Console.WriteLine("\nSomething went wrong");
                    break;
                case 3:
                    Console.WriteLine("\nEnter the following information to add a new course to system:");
                    Console.Write("Name of the course: ");
                    string cname = Console.ReadLine();
                    Console.Write("Total credits for the course: ");
                    int ccredits = int.Parse(Console.ReadLine());
                    Console.Write("ID of the Faculty in Charge: ");
                    int cteacherid = int.Parse(Console.ReadLine());
                    IStudentRepo addCourse=new StudentRepo();
                    int addCourseStat=addCourse.CreateCourse(cname, ccredits, cteacherid);
                    if(addCourseStat>0)
                        Console.WriteLine("\nSuccessfully added the course to the system!");
                    else
                        Console.WriteLine("\nSomething went wrong");
                    break;
                case 4:
                    Console.Write("Enter the ID of the course you wish to fetch information: ");
                    int getCourseID=int.Parse(Console.ReadLine());
                    IStudentRepo getCourse=new StudentRepo();
                    Course c=getCourse.DisplayCourseInfo(getCourseID);
                    Console.WriteLine(c);
                    break;
                case 5:
                    Console.Write("\nEnter the ID of the course for which you wish to fetch faculty details: ");
                    int facultyCourseID=int.Parse(Console.ReadLine());
                    IStudentRepo getFaculty=new StudentRepo();
                    Teacher t=getFaculty.GetTeacher(facultyCourseID);
                    Console.WriteLine(t);
                    break;

                default:
                    Console.WriteLine("\nPlease enter a valid choice");
                    break;
            }
            break;

        //Enrollment Management Portal
        case 3:
            Console.WriteLine("\nWelcome to Enrollment Management portal");
            Console.WriteLine("1. Get Details of a particular student from Enrollments");
            Console.WriteLine("2. Get Details of a particular Course from Enrollments");
            Console.Write("\nEnter your choice: ");
            int enrollmentChoice = int.Parse(Console.ReadLine());

            switch(enrollmentChoice)
            {
                case 1:
                    Console.Write("\nEnter the Enrollment ID to fetch the details of the associated Student: ");
                    int enrollmentStudentID=int.Parse(Console.ReadLine());
                    IStudentRepo enrollmentStudent=new StudentRepo();
                    Console.WriteLine();
                    Student enrolledStudent = enrollmentStudent.GetStudent(enrollmentStudentID);
                    Console.WriteLine(enrolledStudent);
                    break;

                case 2:
                    Console.Write("\nEnter the Enrollment ID to fetch the details of the associated Course: ");
                    int enrollmentCourseID=int.Parse(Console.ReadLine());
                    IStudentRepo enrollmentCourse=new StudentRepo();
                    Console.WriteLine();
                    Course enrolledCourse=enrollmentCourse.GetCourse(enrollmentCourseID);
                    Console.WriteLine(enrolledCourse);
                    break;

                default:
                    Console.WriteLine("\nPlease enter a valid choice");
                    break;
            }
            break;

        //Teacher Management Portal
        case 4:
            Console.WriteLine("\nWelcome to Teacher Management portal");
            Console.WriteLine("1. Add a new teacher");
            Console.WriteLine("2. Update the information of a teacher");
            Console.WriteLine("3. Get information of a specific Teacher");
            Console.WriteLine("4. View all the courses assigned to a partucular teacher");
            Console.Write("\nEnter your choice: ");
            int teacherChoice = int.Parse(Console.ReadLine());

            switch(teacherChoice)
            {
                case 1:
                    Console.WriteLine("\nTo add a new teacher, please enter the following information:");
                    Console.Write("\nFirst Name: ");
                    string fName=Console.ReadLine();
                    Console.Write("Last Name: ");
                    string lName=Console.ReadLine();
                    Console.Write("Email: ");
                    string email=Console.ReadLine();

                    IStudentRepo addTeacher=new StudentRepo();
                    int addTeacherStat=addTeacher.AddTeacher(fName, lName, email);
                    if(addTeacherStat>0)
                        Console.WriteLine("\nAdded teacher data successfully!");
                    else
                        Console.WriteLine("\nSomething went wrong");
                    break;

                case 2:
                    Console.Write("\nEnter the ID of the teacher you wish to update information: ");
                    int updateTeacherID=int.Parse(Console.ReadLine());
                    Console.WriteLine($"Enter the new details of the teacher with ID {updateTeacherID}");
                    Console.Write("\nFirst Name: ");
                    string nfName = Console.ReadLine();
                    Console.Write("Last Name: ");
                    string nlName = Console.ReadLine();
                    Console.Write("Email: ");
                    string nemail = Console.ReadLine();

                    IStudentRepo updateTeacher = new StudentRepo();
                    try
                    {
                        int updateTeacherStat = updateTeacher.UpdateTeacherInfo(updateTeacherID, nfName, nlName, nemail);
                        if (updateTeacherStat > 0)
                            Console.WriteLine("\nUpdated teacher info successfully!");
                        else
                            Console.WriteLine("\nSomething went wrong");
                    }
                    catch(TeacherNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
                
                case 3:
                    Console.Write("\nEnter the ID of the teacher you wish to fetch information: ");
                    int getTeacherID = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    IStudentRepo getTeacher = new StudentRepo();
                    try
                    {
                        Teacher teacherData = getTeacher.DisplayTeacherInfo(getTeacherID);
                        Console.WriteLine(teacherData);
                    }
                    catch(TeacherNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;

                case 4:
                    Console.Write("\nEnter the ID of the teacher to get names of courses they are assigned to: ");
                    int getCourseID = int.Parse(Console.ReadLine());
                    IStudentRepo getCourses = new StudentRepo();
                    try
                    {
                        List<String> assignedCourses = getCourses.GetAssignedCourses(getCourseID);
                        foreach (String c in assignedCourses)
                        {
                            Console.WriteLine($" {c} ");
                        }
                    }
                    catch(TeacherNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;

                default:
                    Console.WriteLine("\nPlease enter a valid choice");
                    break;
            }
            break;

        //Payment Management Portal
        case 5:
            Console.WriteLine("\nWelcome to Payment Management portal");
            Console.WriteLine("1. Get details of a student associated to a particular payment");
            Console.WriteLine("2. View payment amount for a particular payment");
            Console.WriteLine("3. Get the date of payment for a specific record");
            Console.Write("\nEnter your choice: ");
            int paymentChoice = int.Parse(Console.ReadLine());

            switch(paymentChoice)
            {
                case 1:
                    Console.Write("\nEnter the ID of the payment to fetch the details of the student associated with it: ");
                    int paymentStudentID=int.Parse(Console.ReadLine());
                    IStudentRepo paymentStudent = new StudentRepo();
                    Student s = paymentStudent.GetStudentForPayment(paymentStudentID);
                    Console.WriteLine(s);
                    break;

                case 2:
                    Console.Write("\nEnter the ID of the payment to retrieve it's payment amount: ");
                    int amountPaymentID=int.Parse(Console.ReadLine());
                    IStudentRepo paymentAmount=new StudentRepo();
                    double amount = paymentAmount.GetPaymentAmount(amountPaymentID);
                    Console.WriteLine($"\nThe payment amount for payment with ID {amountPaymentID} is: Rs. {amount}");
                    break;

                case 3:
                    Console.Write("Enter the ID of the payment to retrieve it's date of payment: ");
                    int paymentDateID=int.Parse(Console.ReadLine());
                    IStudentRepo paymentDate=new StudentRepo();
                    DateTime d=paymentDate.GetPaymentDate(paymentDateID);
                    Console.WriteLine($"\nThe payment with ID {paymentDateID} was made on: {d}");
                    break;

                default:
                    Console.WriteLine("\nPlease enter a valid choice");
                    break;
            }
            break;

        //To come out of the loop i.e to close the app
        case 6:
            loopChoice = 0;
            break;

        default:
            Console.WriteLine("\nPlease enter a valid choice");
            break;
    }
}
