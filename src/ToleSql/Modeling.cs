using System;
using System.Collections.Concurrent;
using ToleSql.Model;

namespace ToleSql
{
    public static class Modeling
    {
        internal static ConcurrentDictionary<Type, TableModel> _modelList =
                    new ConcurrentDictionary<Type, TableModel>();

        public static void ResetModeling()
        {
            _modelList.Clear();
        }
        public static TableModel<TEntity> Model<TEntity>()
        {
            return (TableModel<TEntity>)_modelList.GetOrAdd(typeof(TEntity),
                (type) => new TableModel<TEntity>());
        }
        public static TableModel Model(Type modelType)
        {
            return _modelList.GetOrAdd(modelType, (type) => new TableModel(type));
        }

        public static bool HasModel<TEntity>()
        {
            return HasModel(typeof(TEntity));
        }
        public static bool HasModel(Type modelType)
        {
            return _modelList.ContainsKey(modelType);
        }
        internal static TableModel<TEntity> GetModel<TEntity>()
        {
            var result = GetModel(typeof(TEntity));
            return (TableModel<TEntity>)result;
        }
        internal static TableModel GetModel(Type modelType)
        {
            var result = _modelList[modelType];
            return result;
        }
    }
}