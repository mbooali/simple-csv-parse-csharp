using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanvasSISCSVImport
{
	class Enrollment
	{
		private string courseId;

		public string CourseId
		{
			get { return courseId; }
			set { courseId = value; }
		}

		private string studentId;

		public string StudentId
		{
			get { return studentId; }
			set { studentId = value; }
		}

		private State enrollmentState;

		public State EnrollmentState
		{
			get { return enrollmentState; }
			set { enrollmentState = value; }
		}


		public Enrollment()
		{ }

		public Enrollment(string courseId, string studentId, State enrollmentState)
		{
			this.CourseId = courseId;
			this.StudentId = studentId;
			this.EnrollmentState = enrollmentState;
		}
	}
}
