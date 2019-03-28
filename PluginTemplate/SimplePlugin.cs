using System;

namespace PluginTemplate
{
    public class SimplePlugin : IPlugin {

		// IPlugin implementation.

		public Scheduler Scheduler {
			set { scheduler = value; }
		}

		public string Name => "Simple Plugin";
		public string Author => "marax27";
		public string Description => "A template plugin for future use.";

		public void Start() {
			scheduler.Request(NotificationFactory.Instance.GetText("Hello world!", "Plugin seems to be working just fine."));
		}

		// Class-specific elements.

		private Scheduler scheduler;
	}
}
