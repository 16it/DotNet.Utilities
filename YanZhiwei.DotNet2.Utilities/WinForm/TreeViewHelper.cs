namespace YanZhiwei.DotNet2.Utilities.WinForm
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    
    /// <summary>
    /// TreeView帮助类
    /// </summary>
    public static class TreeViewHelper
    {
        #region Methods
        
        /// <summary>
        /// 选中节点高亮
        /// <para>eg: treeView1.ApplyNodeHighLight(Color.Red);</para>
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="highLightColor">高亮的颜色</param>
        public static void ApplyNodeHighLight(this TreeView treeView, Brush highLightColor)
        {
            if(treeView.DrawMode != TreeViewDrawMode.OwnerDrawText)
            {
                treeView.DrawMode = TreeViewDrawMode.OwnerDrawText;
            }
            
            if(treeView.HideSelection)
            {
                treeView.HideSelection = false;
            }
            
            treeView.DrawNode += (sender, e) =>
            {
                TreeView _curTreeView = sender as TreeView;
                e.Graphics.FillRectangle(Brushes.White, e.Node.Bounds);
                
                if(e.State == TreeNodeStates.Selected)
                {
                    e.Graphics.FillRectangle(highLightColor, new Rectangle(e.Node.Bounds.Left, e.Node.Bounds.Top, e.Node.Bounds.Width, e.Node.Bounds.Height));
                    e.Graphics.DrawString(e.Node.Text, treeView.Font, Brushes.White, e.Bounds);
                }
                else
                {
                    e.DrawDefault = true;
                }
            };
        }
        
        /// <summary>
        /// 添加右键菜单
        /// <para>eg: treeF18.AttachMenu(contextMenuTree, n => n != null);</para>
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="contextMenu">ContextMenuStrip</param>
        /// <param name="showContextMenuHanlder">显示ContextMenuStrip规则委托</param>
        public static void AttachMenu(this TreeView treeView, ContextMenuStrip contextMenu, Predicate<TreeNode> showContextMenuHanlder)
        {
            treeView.MouseDown += (sender, e) =>
            {
                TreeView _curTree = sender as TreeView;
                
                if(e.Button == MouseButtons.Right)
                {
                    Point _clickPoint = new Point(e.X, e.Y);
                    TreeNode _curNode = _curTree.GetNodeAt(_clickPoint);
                    
                    if(showContextMenuHanlder != null)
                    {
                        if(showContextMenuHanlder(_curNode))
                        {
                            _curTree.SelectedNode = _curNode;
                            _curNode.ContextMenuStrip = contextMenu;
                        }
                    }
                }
            };
        }
        
        /// <summary>
        /// 检查节点是否存在
        /// </summary>
        /// <param name="tree">TreeView</param>
        /// <param name="nodeCompareFactory">节点判断委托</param>
        /// <param name="findedNode">找到节点</param>
        /// <returns>是否存在目标节点</returns>
        public static bool CheckNodeExist(this TreeView tree, Predicate<TreeNode> nodeCompareFactory, out TreeNode findedNode)
        {
            bool _exists = false;
            findedNode = null;
            
            for(int i = 0; i < tree.Nodes.Count; i++)
            {
                TreeNode _curNode = tree.Nodes[i];
                
                if(nodeCompareFactory(_curNode))
                {
                    findedNode = _curNode;
                    _exists = true;
                    break;
                }
                else
                {
                    _exists = CheckNodeExist(tree.Nodes[i], nodeCompareFactory, out findedNode);
                    
                    if(_exists) break;
                }
            }
            
            return _exists;
        }
        
        /// <summary>
        /// 查找子节点是否存在
        /// </summary>
        /// <param name="node">目标节点</param>
        /// <param name="nodeCompareFactory">节点判断委托</param>
        /// <param name="findedNode">找到节点</param>
        /// <returns>是否存在目标节点</returns>
        public static bool CheckNodeExist(this TreeNode node, Predicate<TreeNode> nodeCompareFactory, out TreeNode findedNode)
        {
            findedNode = null;
            bool _result = false;
            
            for(int i = 0; i < node.Nodes.Count; i++)
            {
                TreeNode _curNode = node.Nodes[i];
                
                if(nodeCompareFactory(_curNode))
                {
                    findedNode = _curNode;
                    _result = true;
                    break;
                }
                
                if(!_result && _curNode.Nodes.Count > 0)
                {
                    _result = CheckNodeExist(_curNode, nodeCompareFactory, out findedNode);
                }
            }
            
            return _result;
        }
        
        #endregion Methods
    }
}