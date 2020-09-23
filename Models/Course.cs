using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanvasSISCSVImport
{
	class Course
	{
		private string courseId;

		public string CourseId
		{
			get { return courseId; }
			set { courseId = value; }
		}

		private string courseName;

		public string CourseName
		{
			get { return courseName; }
			set { courseName = value; }
		}

		private State courseState;

		public State CourseState
		{
			get { return courseState; }
			set { courseState = value; }
		}


		public Course()
		{ }

		public Course(string courseId, string courseName, State courseState)
		{
			this.CourseId = courseId;
			this.CourseName = courseName;
			this.CourseState = courseState;
		}


	}


}
