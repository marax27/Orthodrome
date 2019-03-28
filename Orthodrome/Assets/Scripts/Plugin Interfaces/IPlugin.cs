
namespace PluginSystem {

	/// <summary>
	/// The base of any Orthodrome plugin. IPlugin itself offers
	/// little functionality, but every plugin class must implement it.
	/// </summary>
	public interface IPlugin {

		Scheduler Scheduler { set; }

		///<summary>
		/// Method called by PluginManager for each newly created plugin.
		///</summary>
		void Start();

		// Properties.
		string Name { get; }
		string Author { get; }
		string Description { get; }
	}

}
