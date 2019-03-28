using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System;
using UnityEngine;

/// <summary>
/// Loads plugins from drive and allows them interact with the program.
/// </summary>
public class PluginManager : MonoBehaviour
{
	List<IPlugin> plugins = new List<IPlugin>();

	public Scheduler scheduler;

	void Start() {
		Assembly assembly = Assembly.GetExecutingAssembly();
		string location = Path.GetDirectoryName(assembly.Location);
		// print("Searching for plugins at " + location);

		LoadPlugins(location);
		InitializePlugins();
    }

	void LoadPlugins(string directoryPath) {
		plugins.Clear();

		// Process single DLL.
		// TODO: if it's supposed to be multiplatform, not only DLLs may be here.
		foreach(var dll in Directory.GetFiles(directoryPath, "*.dll")) {
			try {
				var asm = Assembly.LoadFrom(dll);

				// Process single type.
				foreach(var type in asm.GetTypes()) {
					if(type.GetInterface("IPlugin") == typeof(IPlugin)) { 
						var plugin = Activator.CreateInstance(type) as IPlugin;
						plugins.Add(plugin);
					}
				}
			}catch(Exception) {
				// TODO: exception checking
				continue;
			}
		}
	}

	void InitializePlugins() {
		foreach(IPlugin plugin in plugins) {
			plugin.Scheduler = scheduler;
			plugin.Start();
			// print("Processed " + plugin.Name);
		}
	}
}
