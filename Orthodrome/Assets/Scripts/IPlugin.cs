using System;

/// <summary>
/// The base of any Orthodrome plugin. IPlugin itself offers
/// little functionality, but every plugin class must implement it.
/// </summary>
public interface IPlugin {

	NotificationFactory Factory { set; }
	Scheduler Scheduler { set; }

	// Properties.
	string Name { get; }
	string Author { get; }
	string Description { get; }

}
