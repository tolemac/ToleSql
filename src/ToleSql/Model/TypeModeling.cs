using System;
using System.Collections.Concurrent;

namespace ToleSql.Model
{
    public class TypeModeling
    {
        internal ConcurrentDictionary<Type, TableModel> _modelList =
                            new ConcurrentDictionary<Type, TableModel>();

        public void ResetModeling()
        {
            _modelList.Clear();
        }

        public string DefaultSchema { get; set; }
        public int ModelListCount { get { return _modelList.Count; } }
        public TableModel<TEntity> Model<TEntity>()
        {
            return (TableModel<TEntity>)_modelList.GetOrAdd(typeof(TEntity),
                (type) => new TableModel<TEntity>());
        }
        public TableModel Model(Type modelType)
        {
            return _modelList.GetOrAdd(modelType, (type) => new TableModel(type));
        }

        public bool HasModel<TEntity>()
        {
            return HasModel(typeof(TEntity));
        }
        public bool HasModel(Type modelType)
        {
            return _modelList.ContainsKey(modelType);
        }
        internal TableModel<TEntity> GetModel<TEntity>()
        {
            var result = GetModel(typeof(TEntity));
            return (TableModel<TEntity>)result;
        }
        internal TableModel GetModel(Type modelType)
        {
            var result = _modelList[modelType];
            return result;
        }
    }
}