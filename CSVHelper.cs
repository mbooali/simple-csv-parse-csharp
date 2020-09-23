using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CanvasSISCSVImport
{
	enum State { Active, Deleted };

	class CSVHelper
	{
		enum CSVFileType { Students, Courses, Enrollments };

		public static void LoadFiles(string directory)
		{
			try
			{
				List<string> csvFiles = Directory.GetFiles(directory).ToList().Where(p => p.EndsWith(".csv")).ToList();

				if (csvFiles.Count() < 1)
					throw new Exception("No CSV file is there");

				foreach (string csvFile in csvFiles)
				{
					ParseCSV(csvFile);
				}
			}
			catch (DirectoryNotFoundException ex)
			{
				Console.WriteLine(ex.ToString());
			}
			catch (Exception)
			{
				throw;
			}
		}

		private static void ParseCSV(string filepath)
		{
			StreamReader sr = new StreamReader(File.OpenRead(filepath));

			string rawHeader = sr.ReadLine(), rawLine = "";
			List<string> parsedHeader = ParseCSVLine(rawHeader);

			CSVFileType csvFileType = checkFileType(parsedHeader);

			if (csvFileType == CSVFileType.Courses)
				ImportCoursesFile(sr, rawLine, parsedHeader);
			else if (csvFileType == CSVFileType.Students)
				ImportStudentsFile(sr, rawLine, parsedHeader);
			else if (csvFileType == CSVFileType.Enrollments)
				ImportEnrollmentsFile(sr, rawLine, parsedHeader);

			sr.Close();
		}

		private static void ImportCoursesFile(StreamReader sr, string rawLine, List<string> parsedHeader)
		{
			List<string> parsedLine = null;
			int course_id_index = parsedHeader.IndexOf("course_id");
			int course_name_index = parsedHeader.IndexOf("course_name");
			int course_state_index = parsedHeader.IndexOf("state");

			while (!sr.EndOfStream)
			{
				rawLine = sr.ReadLine();
				parsedLine = ParseCSVLine(rawLine);
				if (!Store.Courses.ContainsKey(parsedLine[course_id_index]))
				{
					Store.Courses.Add(parsedLine[course_id_index],
						new Course(
							parsedLine[course_id_index],
							parsedLine[course_name_index],
							parsedLine[course_state_index] == "active" ? State.Active : State.Deleted));
				}
				else
				{
					Store.Courses[parsedLine[course_id_index]].CourseName = parsedLine[course_name_index];
					Store.Courses[parsedLine[course_id_index]].CourseState = parsedLine[course_state_index] == "active" ? State.Active : State.Deleted;
				}
			}
		}

		private static void ImportStudentsFile(StreamReader sr, string rawLine, List<string> parsedHeader)
		{
			List<string> parsedLine = null;
			int user_id_index = parsedHeader.IndexOf("user_id");
			int user_name_index = parsedHeader.IndexOf("user_name");
			int user_state_index = parsedHeader.IndexOf("state");

			while (!sr.EndOfStream)
			{
				rawLine = sr.ReadLine();
				parsedLine = ParseCSVLine(rawLine);
				if (!Store.Students.ContainsKey(parsedLine[user_id_index]))
				{
					Store.Students.Add(parsedLine[user_id_index],
						new Student(
							parsedLine[user_id_index],
							parsedLine[user_name_index],
							parsedLine[user_state_index] == "active" ? State.Active : State.Deleted));
				}
				else
				{
					Store.Students[parsedLine[user_id_index]].StudentName = parsedLine[user_name_index];
					Store.Students[parsedLine[user_id_index]].StudentState = parsedLine[user_state_index] == "active" ? State.Active : State.Deleted;
				}
			}
		}

		private static void ImportEnrollmentsFile(StreamReader sr, string rawLine, List<string> parsedHeader)
		{
			List<string> parsedLine = null;
			int course_id_index = parsedHeader.IndexOf("course_id");
			int user_id_index = parsedHeader.IndexOf("user_id");
			int enrollment_state_index = parsedHeader.IndexOf("state");

			while (!sr.EndOfStream)
			{
				rawLine = sr.ReadLine();
				parsedLine = ParseCSVLine(rawLine);
				if (!Store.Enrollments.ContainsKey(parsedLine[course_id_index] + parsedLine[user_id_index]))
				{
					Store.Enrollments.Add(parsedLine[course_id_index] + parsedLine[user_id_index],
						new Enrollment(
							parsedLine[course_id_index],
							parsedLine[user_id_index],
							parsedLine[enrollment_state_index] == "active" ? State.Active : State.Deleted));
				}
				else
				{
					Store.Enrollments[parsedLine[course_id_index] + parsedLine[user_id_index]].EnrollmentState =
						parsedLine[enrollment_state_index] == "active" ? State.Active : State.Deleted;
				}
			}
		}

		private static CSVFileType checkFileType(List<string> parsedHeader)
		{
			if (parsedHeader.Contains("course_id") && parsedHeader.Contains("user_id"))
				return CSVFileType.Enrollments;
			if (parsedHeader.Contains("course_id") && parsedHeader.Contains("course_name"))
				return CSVFileType.Courses;
			if (parsedHeader.Contains("user_id") && parsedHeader.Contains("user_name"))
				return CSVFileType.Students;
			throw new Exception("Invalid File Header!");
		}

		private static List<string> ParseCSVLine(string s)
		{
			s += ',';
			List<string> fields = new List<string>();

			int i = 0, j = 0, n = s.Length;
			bool openQuote = false;

			for (j = 0; j < n; j++)
			{
				if (s[j] == '"' && !openQuote)
				{
					i++;
					openQuote = true;
				}
				else if (s[j] == '"' && openQuote)
				{
					fields.Add(s.Substring(i, j - i));
					j += 2;
					i = j;
					openQuote = false;
				}
				else if (s[j] == ',' && !openQuote)
				{
					fields.Add(s.Substring(i, j - i));
					i = j + 1;
				}
			}
			return fields;
		}

	}
}
