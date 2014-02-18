namespace Commonality.Main.DataStructures
{
    /// <summary>
    /// A tree node to be used in a ternary tree (each node can have 3 children).
    /// </summary>
    internal class Node
    {
        internal char Char;
        internal bool IsEndOfWord;
        internal Node Left, Middle, Right;
    }
}