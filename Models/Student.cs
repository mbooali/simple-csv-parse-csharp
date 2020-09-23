using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanvasSISCSVImport
{
	class Student
	{
		private string studentId;

		public string StudentId
		{
			get { return studentId; }
			set { studentId = value; }
		}

		private string studentName;

		public string StudentName
		{
			get { return studentName; }
			set { studentName = value; }
		}

		private State studentState;

		public State StudentState
		{
			get { return studentState; }
			set { studentState = value; }
		}

		public Student(string studentId, string studentName, State studentState)
		{
			this.StudentId = studentId;
			this.StudentName = studentName;
			this.StudentState = studentState;
		}
	}
}
