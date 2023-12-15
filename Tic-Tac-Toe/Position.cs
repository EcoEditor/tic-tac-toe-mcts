namespace Tic_Tac_Toe
{
	 // TODO: check why not struct
	public class Position
	{
		private int x;
		private int y;

		// TODO: remove
		public Position()
		{
		}

		public Position(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public int X {
			get {
				return x;
			}
			set {
				x = value;
			}
		}

		public int Y {
			get {
				return y;
			}
			set {
				y = value;
			}
		}
	}
}
