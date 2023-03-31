namespace Tics {
	public abstract class Tween {
		public string Name { get; private set; }
		
		public abstract void Kill();
		public abstract void Update(float progress);

		public void SetName(string name) {
			Name = name;
		}
	}
}