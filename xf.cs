using System;
using System.IO;
using System.Xml;
using System.Text;
namespace XMLFormatter {
	class XMLFormatter
	{
		public static void Main() {
			bool useTab = false;
			int indent = 2;
			string inputFile = null;
			string outputFile = null;
			bool DoBackup = true;
			int iArg = 0;
			System.Console.WriteLine("XMLFormatter v1.0\nCopyright 2007 by AZ");
			foreach (string item in Environment.GetCommandLineArgs()){
				//Console.WriteLine("{0}: {1}",iArg,item);
				if (iArg++ == 0){
					continue;
				}
				if (item.StartsWith("\\") || item.StartsWith("/") ){
					char a = item[1];
					if (a == 't'){
						useTab = true;
					} else if (a == 'b'){
						DoBackup = false;
					} else if (a == 'i'){
						indent = int.Parse(item.Substring(3));
					}
				} else if (inputFile != null){
					if (outputFile != null){
						Console.WriteLine("ERROR: invalid usage syntax.");
						PrintUsage();
						return;
					} else {
						outputFile = item;
					}
				} else {
					inputFile = item;
				}
			}

			if (inputFile == null || inputFile == String.Empty){
				PrintUsage();
				return;
			}
			
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(inputFile);

			if (outputFile == null || outputFile == String.Empty){
				outputFile = inputFile;
			}
			if (DoBackup){
				string backupFile = Path.ChangeExtension(outputFile,"bak");
				int iBak = 1;
				while (File.Exists(backupFile)){
					backupFile = Path.ChangeExtension(outputFile,iBak.ToString()+".bak");
					iBak++;
				}
				File.Move(outputFile,backupFile);
			}

			XmlTextWriter writer = new XmlTextWriter(outputFile,Encoding.Default);
			writer.IndentChar = (useTab)?'\t':' ';
			writer.Indentation = indent;
			writer.Formatting = System.Xml.Formatting.Indented;
			xDoc.Save(writer);
		}

		static void PrintUsage() {
			Console.WriteLine("Usage: xf.exe [/t] [/b] [/i:N] inputfile.xml [outputfile.xml] \n" +
				"Options:\n"+
				"/t\t - use tabs instead space\n" +
				"/b\t - don't create backup of output file\n" +
				"/i:N\t - specify indent size of N chars (space o tabs)\n\n" +
				"If you don't specify output file, program will create backup of input file and save changes into input file\n" +
				"WARNING! If option /b is specified and output file not specified, input file will be overrited WITHOUT backup.\n\n"+
				"Press Enter for exit");
			Console.ReadLine();
		}
	};
}