using System;
using ToleSql.Model;

namespace ToleSql
{
    public static class Modeling
    {
        private static TypeModeling _instance = new TypeModeling();
        public static string DefaultSchema { get { return _instance.DefaultSchema; } set { _instance.DefaultSchema = value; } }
        public static void ResetModeling()
        {
            _instance.ResetModeling();
        }
        public static TableModel<TEntity> Model<TEntity>()
        {
            return _instance.Model<TEntity>();
        }
        public static TableModel Model(Type modelType)
        {
            return _instance.Model(modelType);
        }

        public static bool HasModel<TEntity>()
        {
            return _instance.HasModel<TEntity>();
        }
        public static bool HasModel(Type modelType)
        {
            return _instance.HasModel(modelType);
        }
        internal static TableModel<TEntity> GetModel<TEntity>()
        {
            return _instance.GetModel<TEntity>();
        }
        internal static TableModel GetModel(Type modelType)
        {
            return _instance.GetModel(modelType);
        }

        public static int ModelListCount { get { return _instance.ModelListCount; } }
    }
}