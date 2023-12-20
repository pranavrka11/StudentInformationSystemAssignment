using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Exceptions
{
    internal class DuplicateEnrollmentException:ApplicationException
    {
        public DuplicateEnrollmentException()
        {
            Console.WriteLine("This particular student is already enrolled in this particular course");
        }
    }
}
