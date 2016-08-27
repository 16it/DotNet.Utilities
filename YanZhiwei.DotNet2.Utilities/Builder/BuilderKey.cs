using System;
using System.Collections.Generic;
using System.Data;
using YanZhiwei.DotNet2.Utilities.Enum;

namespace YanZhiwei.DotNet2.Utilities.Builder
{
    /// <summary>
    /// 用于构建缓存的类
    /// </summary>
    internal sealed class BuilderKey
    {
        private readonly IList<string> _dataRecordNames;
        private readonly Type _destinationType;
        private readonly DataBaseType _dbType;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="destinationType">Type</param>
        /// <param name="record">IDataRecord</param>
        /// <param name="dbType">DataBaseType</param>
        public BuilderKey(Type destinationType, IDataRecord record, DataBaseType dbType)
        {
            _destinationType = destinationType;
            _dbType = dbType;
            _dataRecordNames = new List<string>(record.FieldCount);

            for(int i = 0; i < record.FieldCount; i++)
            {
                _dataRecordNames.Add(record.GetName(i));
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = _destinationType.GetHashCode();

            foreach(string name in _dataRecordNames)
            {
                hash = hash * 37 + name.GetHashCode();
            }

            return hash + _dbType.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            BuilderKey builderKey = obj as BuilderKey;

            if(builderKey == null)
                return false;

            if(_dbType != builderKey._dbType)
                return false;

            if(_destinationType != builderKey._destinationType)
                return false;

            if(this._dataRecordNames.Count != builderKey._dataRecordNames.Count)
                return false;

            for(int i = 0; i < _dataRecordNames.Count; i++)
            {
                if(this._dataRecordNames[i] != builderKey._dataRecordNames[i])
                    return false;
            }

            return true;
        }
    }
}