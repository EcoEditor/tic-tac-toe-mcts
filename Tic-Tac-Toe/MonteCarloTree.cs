namespace Tic_Tac_Toe
{
	public class MonteCarloTree
	{
		MonteCarloTreeNode root;

		public MonteCarloTree()
		{
			root = new MonteCarloTreeNode();
		}

		public MonteCarloTree(MonteCarloTreeNode root) {
			this.root = root;
		}

        public void AddNode(MonteCarloTreeNode parent, MonteCarloTreeNode child) {
			parent.Children.Add(child);
        }


        public MonteCarloTreeNode Root => root;
	}
}