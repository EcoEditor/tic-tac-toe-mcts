namespace Tic_Tac_Toe {
	/// <summary>
	/// The node object keeps track of the state
	/// </summary>
	public class MonteCarloTreeNode {

		private Random random = new Random();
		private GameState state;
		MonteCarloTreeNode? parent;
		private List<MonteCarloTreeNode> children;

		public MonteCarloTreeNode() {
			state = new GameState();
			children = new List<MonteCarloTreeNode>();
		}

		public MonteCarloTreeNode(GameState state) {
			this.state = state;
			children = new List<MonteCarloTreeNode>();
		}

		public MonteCarloTreeNode(GameState state, MonteCarloTreeNode parent, List<MonteCarloTreeNode> children) {
			this.state = state;
			this.parent = parent;
			this.children = children;
		}

        public MonteCarloTreeNode(MonteCarloTreeNode node) {
            children = new List<MonteCarloTreeNode>();

			this.state = new GameState(node.state);
			if (node.parent != null) {

				this.parent = node.parent;
			}


			foreach (var child in children) {
				children.Add(new MonteCarloTreeNode(node));
			}
        }

        public void AddChild(MonteCarloTreeNode node) {
			children.Add(node);
		}

		// Child scores based on times a child node was visited
		public MonteCarloTreeNode GetRandomChild() {
			var randomIndex = random.Next(children.Count);
			return children[randomIndex];
		}

		public MonteCarloTreeNode? GetChildWithMaxScore() {
			if (children.Count > 0) {
				var highestScoreChild = children.OrderByDescending(child => child.State.VisitCount).FirstOrDefault();
				if (highestScoreChild != null) {
					return highestScoreChild;
				}
			}

			return null;
        }

		public GameState State => state;

		public List<MonteCarloTreeNode> Children => children;

		public MonteCarloTreeNode? Parent {
			get {
				return parent;
			}
			set {
				parent = value;
			}
		}
	}
}