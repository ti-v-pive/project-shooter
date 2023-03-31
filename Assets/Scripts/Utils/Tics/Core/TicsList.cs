namespace Tics {
	public class TicsList {
		public Tic First { get; private set; }
		private Tic Last { get; set; }

		public bool Empty => First == null;

		public Tic Take() {
			var tic = First;
			Remove(tic);
			return tic;
		}
		
		public void Add(Tic tic) {
			if (First == null)
			{
				First = tic;
				Last = tic;
			}
			else
			{
				tic.Prev = Last;
				Last.Next = tic;
				Last = tic;
			}
		}

		public void Remove(Tic tic) {
			if (First == tic) {
				First = tic.Next;
			}

			if (Last == tic) {
				Last = tic.Prev;
			}
			
			if (tic.Prev != null) {
				tic.Prev.Next = tic.Next;
			}

			if (tic.Next != null) {
				tic.Next.Prev = tic.Prev;
			}
			
			tic.Prev = null;
			tic.Next = null;
		}
	}
}