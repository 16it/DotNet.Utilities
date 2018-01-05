using System;
using System.Linq;
using ZooKeeperNet;
using static ZooKeeperNet.KeeperException;

namespace YanZhiwei.DotNet.Zookeeper.Utilities
{
    /// <summary>
    /// ZookeeperWatcher
    /// </summary>
    /// <seealso cref="ZooKeeperNet.IWatcher" />
    public class ZookeeperWatcher : IWatcher
    {
        /// <summary>
        /// ZooKeeper
        /// </summary>
        public readonly ZooKeeper ZooKeeper;

        /// <summary>
        /// ZooKeeper节点改变事件
        /// </summary>
        public event Action<WatchedEvent, byte[]> OnNodeChangeEvent;

        /// <summary>
        /// ZooKeeper子节点改变事件
        /// </summary>
        public event Action<WatchedEvent, string[]> OnNodeChildrenChangeEvent;

        /// <summary>
        /// ZooKeeper节点异常事件
        /// </summary>
        public event Action<NoNodeException, string> OnNodeExceptionEvent;

        /// <summary>
        /// ZooKeeper节点路径
        /// </summary>
        public readonly string Path;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="zk">ZooKeeper</param>
        /// <param name="path">节点路径</param>
        public ZookeeperWatcher(ZooKeeper zk, string path)
        {
            Path = path;
            ZooKeeper = zk;

            if (zk.Exists(path, false) == null)
            {
                zk.Exists(path, this);
            }
            else
            {
                zk.GetData(path, this, null);
                zk.GetChildren(path, this, null);
            }
        }

        /// <summary>
        /// Processes the specified watched event.
        /// </summary>
        /// <param name="watchedEvent">The watched event.</param>
        public void Process(WatchedEvent watchedEvent)
        {
            try
            {
                switch (watchedEvent.Type)
                {
                    case EventType.None:
                        break;

                    case EventType.NodeCreated:
                        ZooKeeper.GetChildren(watchedEvent.Path, this, null);
                        var _nodeData = ZooKeeper.GetData(watchedEvent.Path, this, null);

                        if (OnNodeChangeEvent != null)
                            OnNodeChangeEvent(watchedEvent, _nodeData);
                        break;

                    case EventType.NodeDeleted:

                        ZooKeeper.Exists(watchedEvent.Path, this);

                        if (OnNodeChangeEvent != null)
                            OnNodeChangeEvent(watchedEvent, null);
                        break;

                    case EventType.NodeChildrenChanged:
                        var _chlidrenNode = ZooKeeper.GetChildren(watchedEvent.Path, this, null);
                        if (OnNodeChildrenChangeEvent != null)
                            OnNodeChildrenChangeEvent(watchedEvent, _chlidrenNode.ToArray());
                        break;

                    default:

                        var _nodeData = ZooKeeper.GetData(watchedEvent.Path, this, null);
                        if (OnNodeChangeEvent != null)
                            OnNodeChangeEvent(watchedEvent, _nodeData);
                        break;
                }
            }
            catch (NoNodeException ex)
            {
                if (OnNodeExceptionEvent != null)
                    OnNodeExceptionEvent(ex, Path);
            }
        }
    }
}