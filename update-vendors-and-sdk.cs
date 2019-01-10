using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

class Script
{
	static readonly string INIT_CWD = Environment.CurrentDirectory + '\\';

	static public void Main(string[] args)
	{
		if(true)
		{
			// Update C# project (Nuget, dll)
			File.Copy(@"D:\sciter-sdk\bin.osx\sciter-osx-64.dylib", INIT_CWD + @"CS-TemplateMultiPlatform\SciterBootstrap\libsciter-osx-64.dylib", true);

			var dirs_csharp = Directory.EnumerateDirectories(INIT_CWD).Where(dir => dir.Contains("CS-"));
			foreach(var dir_path in dirs_csharp)
			{
				// Nuget update (does not work for TemplateMultiPlatform)
				SpawnProcess("nuget", "update SciterBootstrap.sln", dir_path);

				// Copy DLL 64 bit only (I was copying 32 too till Sciter 3)
				File.Copy(@"D:\sciter-sdk\bin\64\sciter.dll", dir_path + @"\SciterBootstrap\sciter.dll", true);
			}
		}

		// Update D projects (GIT, dll's)
		if(true)
		{
			var dirs_d = Directory.EnumerateDirectories(INIT_CWD).Where(dir => dir.Contains("D-"));
			foreach(var dir_path in dirs_d)
			{
				// GIT update
				SpawnProcess("cmd", "/C call copy-vendor.bat", dir_path + @"\vendor");

				// Copy DLLs
				File.Copy(@"D:\sciter-sdk\bin\32\sciter.dll", dir_path + @"\Debug\sciter.dll", true);
				File.Copy(@"D:\sciter-sdk\bin\32\sciter.dll", dir_path + @"\Release\sciter.dll", true);
			}
		}

		// Update C++ projects (GIT, dll's)
		if(true)
		{
			File.Copy(@"D:\sciter-sdk\bin.osx\sciter-osx-64.dylib", INIT_CWD + @"CPP-Multi\sciter-osx-64.dylib", true);
		
			var dirs_cpp = Directory.EnumerateDirectories(INIT_CWD).Where(dir => dir.Contains("CPP-"));
			foreach(var dir_path in dirs_cpp)
			{
				// Copy \include
				var files = Directory.EnumerateFiles(@"D:\sciter-sdk\include", "*", SearchOption.AllDirectories);
				foreach(var file in files)
				{
					string subfile = file.Substring(@"D:\sciter-sdk\include".Length);
					File.Copy(file, dir_path + @"\SciterBootstrap\vendor\sciter-sdk\include\" + subfile, true);
				}

				// Copy DLLs
				File.Copy(@"D:\sciter-sdk\bin\64\sciter.dll", dir_path + @"\sciter.dll", true);
				//File.Copy(@"D:\Projetos\Libs Shared\sciter-sdk-3\bin\sciter64.dll", dir_path + @"\vendor\sciter-sdk-3\bin\sciter64.dll", true);

				// Copy lib
				//File.Copy(@"D:\sciter-sdk\lib\sciter32.lib", dir_path + @"\vendor\sciter-sdk-3\lib\sciter32.lib", true);
				//File.Copy(@"D:\sciter-sdk\lib\sciter64.lib", dir_path + @"\vendor\sciter-sdk-3\lib\sciter64.lib", true);
			}
		}

		// All: git add/commit/push
		if(true)
		{
			Console.WriteLine();
			Console.WriteLine("#####################################################");
			Console.WriteLine("# PUSH repo: " + INIT_CWD);

			Console.WriteLine("#####################################################");
			var sciter_ver = FileVersionInfo.GetVersionInfo(@"D:\sciter-sdk\bin\32\sciter.dll");
			string sciter_ver_str = string.Format("{0}.{1}.{2}.{3}", sciter_ver.FileMajorPart, sciter_ver.FileMinorPart, sciter_ver.FileBuildPart, sciter_ver.FilePrivatePart);
			string commit_msg = string.Format("Update Boot -- Sciter version " + sciter_ver_str);

			var dirs = Directory.EnumerateDirectories(INIT_CWD);
			SpawnProcess("git", "add .", INIT_CWD);
			SpawnProcess("git", "commit -m\"" + commit_msg + "\"", INIT_CWD, true);
			SpawnProcess("git", "push origin", INIT_CWD);
		}
	}

	static public void SpawnProcess(string exe, string args, string cwd = null, bool ignore_error = false)
	{
		var startInfo = new ProcessStartInfo(exe, args)
		{
			FileName = exe,
			Arguments = args,
			UseShellExecute = false,
			WorkingDirectory = cwd ?? INIT_CWD
		};

		var p = Process.Start(startInfo);
		p.WaitForExit();

		if(p.ExitCode != 0)
		{
			Console.ForegroundColor = ignore_error ? ConsoleColor.Yellow : ConsoleColor.Red;

			string msg = exe + ' ' + args;
			Console.WriteLine();
			Console.WriteLine("-------------------------");
			Console.WriteLine("FAILED: " + msg);
			Console.WriteLine("EXIT CODE: " + p.ExitCode);
			if(!ignore_error)
				Console.WriteLine("Press ENTER to exit");
			Console.WriteLine("-------------------------");
			Console.ResetColor();

			if(!ignore_error)
			{
				Console.ReadLine();
				Environment.Exit(0);
			}
		}

		Console.WriteLine();
	}
}