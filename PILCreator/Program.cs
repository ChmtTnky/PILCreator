namespace PILCreator
{
	public static class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine(
					"Takes a folder of PIM files and combines them into a PIL file (alphabetical order)\n" +
					"Alternatively, takes in the file paths of various PIM files and combines them into a PIL file in the order they were entered\n" +
					"Example Args:\n" +
					"PILCreator.exe <Path to Folder>\n" +
					"PILCreator.exe <Path to first PIM file> <Path to second PIM file> ... <Path to final PIM file>\n");
				return;
			}
			if (args.Length == 1)
			{
				if (!Directory.Exists(args[0]))
				{
					Console.WriteLine(
						"Directory does not exist\n");
					return;
				}
				var files = Directory.GetFiles(args[0]);
				for (int i = 0; i < files.Length; i++)
				{
					if (Path.GetExtension(files[i]).ToLower() != ".pim")
						files[i] = string.Empty;
				}
				var file_list = files.ToList();
				file_list.RemoveAll(f => f == string.Empty);
				file_list.OrderBy(file => Path.GetFileName(file));
				if (file_list.Count > 0)
					PILCreator.GeneratePIL(file_list.ToArray());
				return;
			}
			if (args.Length > 1)
			{
				List<string> file_list = new List<string>();
				foreach (var file in args)
				{
					if (File.Exists(file) && Path.GetExtension(file).ToLower() != ".pim")
						file_list.Add(file);
				}
				if (file_list.Count > 0)
					PILCreator.GeneratePIL(file_list.ToArray());
				return;
			}
		}
	}
}