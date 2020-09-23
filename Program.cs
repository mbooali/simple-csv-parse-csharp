using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CanvasSISCSVImport
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				string initialCSVDirectory = args.Length > 0 ? args[0] : @"C:\Users\Maziar\Desktop\csvs";
				CSVHelper.LoadFiles(initialCSVDirectory);
				Store.SpitOut(initialCSVDirectory);

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}
