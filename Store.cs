using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CanvasSISCSVImport
{
	class Store
	{
		private static Dictionary<string, Enrollment> enrollments = new Dictionary<string, Enrollment>();
		private static Dictionary<string, Student> students = new Dictionary<string, Student>();
		private static Dictionary<string, Course> courses = new Dictionary<string, Course>();

		internal static Dictionary<string, Student> Students
		{
			get
			{
				return students;
			}

			set
			{
				students = value;
			}
		}

		internal static Dictionary<string, Course> Courses
		{
			get
			{
				return courses;
			}

			set
			{
				courses = value;
			}
		}

		internal static Dictionary<string, Enrollment> Enrollments
		{
			get
			{
				return enrollments;
			}

			set
			{
				enrollments = value;
			}
		}

		public static void SpitOut(string outputPath = "")
		{
			var activeCourses = Courses.Values.Where(p => p.CourseState == State.Active);
			var activeStudents = Students.Values.Where(p => p.StudentState == State.Active);
			var activeEnrollments = Enrollments.Values.Where(p => p.EnrollmentState == State.Active);

			var result = from course in activeCourses
						 join enrollment in activeEnrollments on course.CourseId equals enrollment.CourseId
						 select new { course.CourseId, enrollment.StudentId } into course_enrollment
						 join student in activeStudents on course_enrollment.StudentId equals student.StudentId
						 select new { course_enrollment.CourseId, student.StudentId, student.StudentName };

			StreamWriter sw = new StreamWriter(File.OpenWrite(outputPath + Path.DirectorySeparatorChar + "output.txt"));

			foreach (var activeCourse in activeCourses)
			{
				string courseHeader = activeCourse.CourseName + "\t" + activeCourse.CourseId + Environment.NewLine, tempStudentTuple = "";
				Console.WriteLine(courseHeader);
				sw.WriteLine(courseHeader);
				var studentsOfThisCourse = result.Where(p => p.CourseId == activeCourse.CourseId);
				foreach (var student in studentsOfThisCourse)
				{
					tempStudentTuple = student.StudentName + "\t\t" + student.StudentId;
					Console.WriteLine(tempStudentTuple);
					sw.WriteLine(tempStudentTuple);
				}

				Console.WriteLine(Environment.NewLine + "_____________________________________________________");
				sw.WriteLine(Environment.NewLine + "_____________________________________________________");
			}

			sw.Flush();
			sw.Close();

			return;
		}


	}
}
